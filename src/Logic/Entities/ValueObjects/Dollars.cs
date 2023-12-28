namespace Logic.Entities.ValueObjects;

public class Dollars : ValueObject<Dollars>
{
    private const decimal MaxValue = 1_000_000;
    public decimal Value { get; }

    private Dollars(decimal value)
    {
        Value = value;
    }

    public static Result<Dollars> Create(decimal dollarAmount)
    {
        if (dollarAmount < 0)
            return Result.Fail<Dollars>("Dollars cannot be negative");

        if (dollarAmount > MaxValue)
            return Result.Fail<Dollars>($"Dollars cannot be greater than {MaxValue}");

        if (dollarAmount % 0.01m > 0)
            return Result.Fail<Dollars>("Dollars cannot contain part of a penny");

        return Result.Ok(new Dollars(dollarAmount));
    }

    public static implicit operator decimal(Dollars dollars)
    {
        return dollars.Value;
    }

    public static Dollars Of(decimal value)
    {
        return new Dollars(value);
    }

    public static Dollars operator +(Dollars dollars1, Dollars dollars2)
    {
        return new Dollars(dollars1.Value + dollars2.Value);
    }

    public static Dollars operator -(Dollars dollars1, Dollars dollars2)
    {
        return new Dollars(dollars1.Value - dollars2.Value);
    }

    public static Dollars operator *(Dollars dollars, decimal multiplier)
    {
        return new Dollars(dollars.Value * multiplier);
    }

    protected override bool EqualsCore(Dollars other)
    {
        return Value == other.Value;
    }

    protected override int GetHashCodeCore()
    {
        return Value.GetHashCode();
    }
}