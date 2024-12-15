namespace ImdbApp;

public class ImdbIdComboItem
{
    public uint Value { get; set; }

    public override string ToString() =>
        Value.ToString("n0");

    public bool IsSameAs(ImdbIdComboItem other) =>
        other.Value == Value;
}