# TripleG3.Cis.Wpf

`TripleG3.Cis.Wpf` contains small WPF/MVVM helpers for applications that use the `TripleG3.Cis` Command Immutable State pattern. It adapts CIS state services to common WPF binding primitives such as `INotifyPropertyChanged` and `ICommand`.

The library targets `net10.0-windows` and is intended for WPF applications.

## What It Provides

- `ViewModel`: a minimal `INotifyPropertyChanged` base class.
- `BindingCommand`: an `ICommand` implementation backed by execute and can-execute delegates.
- `BindingCommand<T>`: a typed command wrapper for command parameters.
- `StateServiceViewModel<T>`: a view-model base that mirrors an `IStateService<T>` value into a bindable `Value` property.
- `StateBindingCommand<T>`: an `ICommand` that updates an `IStateService<T>` by calling `SetAsync`.
- `StateBindingCommandParameter<T>`: a small record that groups the service, value factory, cancellation token factory, and notification source needed by `StateBindingCommand<T>`.
- `EmptyNotifyPropertyChanged`: a no-op notifier for places that need an `INotifyPropertyChanged` instance but should not raise events.

## Project Layout

```text
src/TripleG3.Cis.Wpf/             Library source
tests/TripleG3.Cis.Wpf.Tests/     Unit tests
```

## Requirements

- .NET 10 SDK
- Windows, because the library targets WPF through `net10.0-windows`
- `TripleG3.Cis`, referenced by the library project

To build and test the repository:

```powershell
dotnet build TripleG3.Cis.Wpf.slnx
dotnet test TripleG3.Cis.Wpf.slnx
```

## Referencing The Library

Inside this repository, reference the project directly:

```xml
<ProjectReference Include="..\path\to\TripleG3.Cis.Wpf.csproj" />
```

If you consume it from a package feed, add the package instead:

```powershell
dotnet add package TripleG3.Cis.Wpf
```

## Basic Command Usage

Use `BindingCommand` when you need a normal WPF command that is not responsible for running a CIS state transition.

```csharp
using TripleG3.Cis.Wpf;

public sealed class SaveViewModel : ViewModel
{
	private string name = string.Empty;

	public SaveViewModel()
	{
		SaveCommand = new BindingCommand(Save, CanSave, this);
	}

	public string Name
	{
		get => name;
		set
		{
			if (value == name) return;

			name = value;
			OnPropertyChanged();
		}
	}

	public BindingCommand SaveCommand { get; }

	private bool CanSave() => !string.IsNullOrWhiteSpace(Name);

	private void Save()
	{
		// Save the current Name value.
	}
}
```

When `Name` changes, `OnPropertyChanged` is raised. `BindingCommand` listens to that notification source and raises `CanExecuteChanged`, so WPF can requery whether the command is enabled.

## CIS State Command Usage

Use `StateBindingCommand<T>` when the command should update an `IStateService<T>` by calling `SetAsync`.

```csharp
using TripleG3.Cis;
using TripleG3.Cis.Wpf;

public sealed class MessageViewModel : StateServiceViewModel<string>
{
	public MessageViewModel(IStateService<string> messageStateService)
		: base(messageStateService)
	{
		StateBindingCommandParameter<string> commandParameter = new(
			messageStateService,
			CreateMessageAsync,
			GetCancellationToken,
			this);

		CreateMessageCommand = new StateBindingCommand<string>(commandParameter);
	}

	public StateBindingCommand<string> CreateMessageCommand { get; }

	private async ValueTask<string> CreateMessageAsync(CancellationToken cancellationToken)
	{
		await Task.Delay(250, cancellationToken);
		return string.IsNullOrWhiteSpace(Value) ? string.Empty : Value.Trim();
	}
}
```

`StateBindingCommand<T>` uses the supplied `StateValueFactory<T>` to produce the next state value. The command returns `false` from `CanExecute` while the state service status is `StateStatus.Busy`.

The value factory should produce the next value. It should not call `SetAsync` on the same state service, because `StateBindingCommand<T>` already owns that state transition.

## WPF Binding Example

Bind to the view model in XAML like any other WPF view model:

```xml
<StackPanel>
	<TextBox Text="{Binding Value, UpdateSourceTrigger=PropertyChanged}" />
	<Button Command="{Binding CreateMessageCommand}" Content="Create" />
	<TextBlock Text="{Binding Value}" />
</StackPanel>
```

`StateServiceViewModel<T>.Value` is initialized from the service state and updates whenever the service raises `StateChanged`. If a screen needs separate input and output values, add additional properties to the derived view model.

## Dependency Injection Example

Register your CIS state services and view models with the application's service container:

```csharp
using Microsoft.Extensions.DependencyInjection;
using TripleG3.Cis;

services.AddSingleton<IStateService<string>, StateService<string>>();
services.AddTransient<MessageViewModel>();
```

For application-specific behavior, create a named interface and implementation around `StateService<T>`, then inject that service into the view model.

## Choosing A Command Type

- Use `BindingCommand` for normal synchronous WPF actions or for actions that already manage their own async workflow.
- Use `BindingCommand<T>` when the command parameter has a meaningful type.
- Use `StateBindingCommand<T>` when the command should call `IStateService<T>.SetAsync` and update CIS state.

## Example Types

The repository includes `ExampleStringService` and `ExampleViewModel` as a compact demonstration of the library's pieces. Treat them as sample code, not as a full application architecture.

## Testing

The test project uses xUnit and covers the command, notification, view-model, and state-binding helpers. Run it with:

```powershell
dotnet test tests/TripleG3.Cis.Wpf.Tests/TripleG3.Cis.Wpf.Tests.csproj
```
