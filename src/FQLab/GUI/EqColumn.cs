using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

namespace FQLab;

public class EqColumn : View
{
    public EqRanges Range { get; }

    public int Value { get; private set; }

    private View _valueDisplay;

    public event Action? ValChanged;

    public EqColumn(EqRanges range)
    {
        Range = range;
        Width = 5;
        Height = Dim.Fill();

        var label = new Label()
        {
            Text = Range.ToString(),
            X = Pos.Center(),
            Y = 0
        };

        var plus = new Button()
        {
            Text = "[ + ]",
            X = Pos.Center(),
            Y = Pos.Bottom(label),
            ShadowStyle = ShadowStyle.None
        };

        _valueDisplay = new Label()
        {
            Text = $"{Value}",
            X = Pos.Center(),
            Y = Pos.Bottom(plus) + 1,
        };

        var minus = new Button()
        {
            Text = "[ - ]",
            X = Pos.Center(),
            Y = Pos.Bottom(_valueDisplay) + 1,
            ShadowStyle = ShadowStyle.None
        };

        plus.Accepting += (s, e) =>
        {
            ChangeValue(1);
            e.Handled = true;
        };

        minus.Accepting += (s, e) =>
        {
            ChangeValue(-1);
            e.Handled = true;
        };

        Add(label, plus, _valueDisplay, minus);

    }

    private void ChangeValue(int amount)
    {
        Value = Math.Clamp(Value + amount, -5, 5);
        _valueDisplay.Text = $"{Value}";
        ValChanged?.Invoke();
    }
}