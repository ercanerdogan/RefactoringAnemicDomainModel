using Logic.Entities.ValueObjects;
using Newtonsoft.Json;

namespace Logic.Entities;

public abstract class Movie : Entity
{
    public virtual string Name { get; protected set; }
    protected virtual LicensingModel LicensingModel { get; set; }

    public abstract ExpirationDate GetExpirationDate();

    protected abstract Dollars GetBasePrice();

    public virtual Dollars CalculatePrice(CustomerStatus status)
    {
        var modifier = 1 - status.GetDiscount();
        return GetBasePrice() * modifier;
    }
}

public class TwoDaysMovie : Movie
{
    public override ExpirationDate GetExpirationDate()
    {
        return (ExpirationDate)DateTime.UtcNow.AddDays(2);
    }

    protected override Dollars GetBasePrice()
    {
        return Dollars.Of(4);
    }
}

public class LifeLongMovie : Movie
{
    public override ExpirationDate GetExpirationDate()
    {
        return ExpirationDate.Infinite;
    }

    protected override Dollars GetBasePrice()
    {
        return Dollars.Of(8);
    }
}