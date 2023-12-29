﻿using Logic.Entities.ValueObjects;

namespace Logic.Entities;

public class Customer : Entity
{
    protected Customer()
    {
        _purchasedMovies = new List<PurchasedMovie>();
        
    }
    public Customer(CustomerName name, Email email) : this()
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
        protected set => _moneySpent = value;
    }

    private readonly IList<PurchasedMovie> _purchasedMovies;
    public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();

    public virtual void AddPurchasedMovie(Movie movie, ExpirationDate expirationDate, Dollars price)
    {
        var purchasedMovie = new PurchasedMovie
        {
            MovieId = movie.Id,
            CustomerId = Id,
            ExpirationDate = expirationDate,
            Price = price,
            PurchaseDate = DateTime.UtcNow
        };

        _purchasedMovies.Add(purchasedMovie);
        MoneySpent += price;
    }
}