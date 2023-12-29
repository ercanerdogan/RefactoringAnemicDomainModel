using Logic.Entities;
using Logic.Entities.ValueObjects;

namespace Logic.Services;

public class CustomerService
{
    private readonly MovieService _movieService;

    public CustomerService(MovieService movieService)
    {
        _movieService = movieService;
    }

    private Dollars CalculatePrice(CustomerStatus status, ExpirationDate statusExpirationDate,
        LicensingModel licensingModel)
    {
        var price = licensingModel switch
        {
            LicensingModel.TwoDays => Dollars.Of(4),
            LicensingModel.LifeLong => Dollars.Of(8),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (status == CustomerStatus.Advanced && !statusExpirationDate.IsExpired) price = price * 0.75m;

        return price;
    }

    public void PurchaseMovie(Customer customer, Movie movie)
    {
        var expirationDate = _movieService.GetExpirationDate(movie.LicensingModel);
        var price = CalculatePrice(customer.Status, customer.StatusExpirationDate, movie.LicensingModel);
        customer.AddPurchasedMovie(movie, expirationDate, price);
    }

    public bool PromoteCustomer(Customer customer)
    {
        // at least 2 active movies during the last 30 days
        if (customer.PurchasedMovies.Count(x =>
                x.ExpirationDate == ExpirationDate.Infinite || x.ExpirationDate.Date >= DateTime.UtcNow.AddDays(-30)) <
            2)
            return false;

        // at least 100 dollars spent during the last year
        if (customer.PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
            return false;

        customer.Status = CustomerStatus.Advanced;
        customer.StatusExpirationDate = (ExpirationDate)DateTime.UtcNow.AddYears(1);

        return true;
    }
}