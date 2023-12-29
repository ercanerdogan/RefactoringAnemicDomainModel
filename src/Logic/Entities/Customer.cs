using Logic.Entities.ValueObjects;

namespace Logic.Entities;

public class Customer : Entity
{
    protected Customer()
    {
        
    }
    public Customer(CustomerName name, Email email)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _email = email ?? throw new ArgumentNullException(nameof(email));
        MoneySpent = Dollars.Of(0);
        Status = CustomerStatus.Regular;
        StatusExpirationDate = null;
    }

    private string _name;

    public virtual CustomerName Name
    {
        get => (CustomerName)_name;
        set => _name = value;
    }

    private string _email;

    public virtual Email Email
    {
        get => (Email)_email;
        set => _email = value;
    }

    public virtual CustomerStatus Status { get; set; }

    private DateTime? _statusExpirationDate;

    public virtual ExpirationDate StatusExpirationDate
    {
        get => (ExpirationDate)_statusExpirationDate;
        set => _statusExpirationDate = value;
    }

    private decimal _moneySpent;

    public virtual Dollars MoneySpent
    {
        get => Dollars.Of(_moneySpent);
        set => _moneySpent = value;
    }

    public virtual IList<PurchasedMovie> PurchasedMovies { get; set; }
}