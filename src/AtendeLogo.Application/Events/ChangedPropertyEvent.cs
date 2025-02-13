namespace AtendeLogo.Application.Events;

public record ChangedPropertyEvent(
    string PropertyName,
    object? OldValue,
    object? NewValue) : IChangedPropertyEvent
{
    public sealed override string ToString()
        => $"{PropertyName}: {OldValue} -> {NewValue}";
}

public record PropertyValueEvent(
    string PropertyName,
    object? Value) : IPropertyValueEvent;
