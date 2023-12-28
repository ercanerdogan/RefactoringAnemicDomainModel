namespace Logic.Entities.ValueObjects;

public class ExpirationDate : ValueObject<ExpirationDate>
{
    public DateTime? Date { get; }
    public static readonly ExpirationDate Infinite = new(null);
    public bool IsExpired => this != Infinite && Date < DateTime.Now;


    private ExpirationDate(DateTime? date)
    {
        Date = date;
    }

    public static Result<ExpirationDate> Create(DateTime date)
    {
        return Result.Ok(new ExpirationDate(date));
    }

    public static explicit operator ExpirationDate(DateTime? date)
    {
        return date.HasValue ? Create(date.Value).Value : Infinite;
    }

    public static implicit operator DateTime?(ExpirationDate date)
    {
        return date.Date;
    }

    protected override bool EqualsCore(ExpirationDate other)
    {
        return Date == other.Date;
    }

    protected override int GetHashCodeCore()
    {
        return Date.GetHashCode();
    }
}