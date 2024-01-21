using Api.Controllers;
using Logic.Common;
using Logic.Customers;
using Logic.Movies;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Customers;

[Route("api/[controller]")]
public class CustomersController : BaseController
{
    private readonly MovieRepository _movieRepository;
    private readonly CustomerRepository _customerRepository;

    public CustomersController(UnitOfWork unitOfWork,
        MovieRepository movieRepository,
        CustomerRepository customerRepository) : base(unitOfWork)
    {
        _customerRepository = customerRepository;
        _movieRepository = movieRepository;
    }

    [HttpGet]
    [Route("{id}")]
    public IActionResult Get(long id)
    {
        var customer = _customerRepository.GetById(id);
        if (customer == null) return NotFound();

        var dto = new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name.Value,
            Email = customer.Email.Value,
            MoneySpent = customer.MoneySpent,
            Status = customer.Status.Type.ToString(),
            StatusExpirationDate = customer.Status.ExpirationDate,
            PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto
            {
                Price = x.Price,
                PurchaseDate = x.PurchaseDate,
                ExpirationDate = x.ExpirationDate,
                Movie = new MovieDto
                {
                    Id = x.Movie.Id,
                    Name = x.Movie.Name
                }
            }).ToList()
        };

        return Ok(dto);
    }

    [HttpGet]
    public IActionResult GetList()
    {
        var customers = _customerRepository.GetList();

        var dto = customers.Select(x => new CustomerInListDto
        {
            Id = x.Id,
            Name = x.Name.Value,
            Email = x.Email.Value,
            Status = x.Status.Type.ToString(),
            StatusExpirationDate = x.Status.ExpirationDate
        }).ToList();

        return Ok(dto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateCustomerDto item)
    {
        var customerNameResult = CustomerName.Create(item.Name);
        var emailResult = Email.Create(item.Email);

        var result = Result.Combine(customerNameResult, emailResult);

        if (result.IsFailure) return Error(result.Error);

        if (_customerRepository.GetByEmail(emailResult.Value) != null)
            return Error("Email is already in use: " + item.Email);

        var customer = new Customer(customerNameResult.Value, emailResult.Value);

        _customerRepository.Add(customer);

        return Ok();
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
    {
        var customerNameResult = CustomerName.Create(item.Name);

        if (customerNameResult.IsFailure) return BadRequest(customerNameResult.Error);

        var customer = _customerRepository.GetById(id);
        if (customer == null) return BadRequest("Invalid customer id: " + id);

        customer.Name = customerNameResult.Value;

        return Ok();
    }

    [HttpPost]
    [Route("{id}/movies")]
    public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
    {
        var movie = _movieRepository.GetById(movieId);
        if (movie == null) return Error("Invalid movie id: " + movieId);

        var customer = _customerRepository.GetById(id);
        if (customer == null) return Error("Invalid customer id: " + id);

        if (customer.HasPurchasedMovie(movie))
        {
            return Error("The movie is already purchased: " + movie.Name);
        }

        customer.PurchaseMovie(movie);

        return Ok();
    }

    [HttpPost]
    [Route("{id}/promotion")]
    public IActionResult PromoteCustomer(long id)
    {
        var customer = _customerRepository.GetById(id);

        if (customer == null) return BadRequest("Invalid customer id: " + id);

        var checkPromotion = customer.CanPromote();

        if (checkPromotion.IsFailure)
            return Error(checkPromotion.Error);

        customer.Promote();

        return Ok();
    }
}