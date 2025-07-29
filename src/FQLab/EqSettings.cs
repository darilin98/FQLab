namespace FQLab;

public readonly struct EqSettings
{
    public int Lows { get; init; }
    public int Mids { get; init; }
    public int Highs { get; init; }

    
    public float this[EqRanges range] =>
        range switch
        {
            EqRanges.Lows => ToCoefficient(Lows),
            EqRanges.Mids => ToCoefficient(Mids),
            EqRanges.Highs => ToCoefficient(Highs),
            _ => throw new ArgumentOutOfRangeException()
        };

    public override string ToString()
    {
        return $"Low: {Lows} | Mid: {Mids} | High: {Highs}";
    }
        
    private float ToCoefficient(int setting)
    {
        static float Map(int val) => MathF.Pow(2f, val / 5f);
        return Map(setting);
    }
}

public enum EqRanges
{
    Lows = 0,
    Mids,
    Highs,
}