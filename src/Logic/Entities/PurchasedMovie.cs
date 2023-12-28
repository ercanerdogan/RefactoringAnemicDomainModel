﻿using Logic.Entities.ValueObjects;

namespace Logic.Entities;

public class PurchasedMovie : Entity
{
    public virtual long MovieId { get; set; }
    public virtual Movie Movie { get; set; }
    public virtual long CustomerId { get; set; }

    private decimal _price;

    public virtual Dollars Price
    {
        get => Dollars.Of(_price);
        set => _price = value;
    }

    public virtual DateTime PurchaseDate { get; set; }

    private DateTime? _expirationDate;

    public virtual ExpirationDate ExpirationDate
    {
        get => (ExpirationDate)_expirationDate;
        set => _expirationDate = value;
    }
}