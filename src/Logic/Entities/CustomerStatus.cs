using Logic.Entities.ValueObjects;

namespace Logic.Entities;

public class CustomerStatus : ValueObject<CustomerStatus>
{
    public static readonly CustomerStatus Regular = new(CustomerStatusType.Regular, ExpirationDate.Infinite);

    public virtual CustomerStatusType Type { get; }

    private readonly DateTime? _expirationDate;

    public ExpirationDate ExpirationDate => (ExpirationDate)_expirationDate;

    public bool IsAdvanced => Type == CustomerStatusType.Advanced && !ExpirationDate.IsExpired;

    private CustomerStatus()
    {
        
    }

    private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate) :this()
    {
        Type = type;
        _expirationDate = expirationDate ?? throw new ArgumentNullException(nameof(expirationDate));
    }

    public CustomerStatus Promote()
    {
        return new CustomerStatus(CustomerStatusType.Advanced, (ExpirationDate)DateTime.UtcNow.AddYears(1));
    }

    public decimal GetDiscount()
    {
        return IsAdvanced ? 0.25m : 0;
    }

    protected override bool EqualsCore(CustomerStatus other)
    {
        return Type == other.Type && ExpirationDate == other.ExpirationDate;
    }

    protected override int GetHashCodeCore()
    {
        return Type.GetHashCode() ^ ExpirationDate.GetHashCode();
        //HashCode.Combine(Type, ExpirationDate);
    }
}

public enum CustomerStatusType
{
    Regular = 1,
    Advanced = 2
}
