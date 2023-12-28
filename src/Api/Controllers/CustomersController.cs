using Logic.Dtos;
using Logic.Entities;
using Logic.Entities.ValueObjects;
using Logic.Repositories;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
public class CustomersController : Controller
{
    private readonly MovieRepository _movieRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly CustomerService _customerService;

    public CustomersController(MovieRepository movieRepository,
        CustomerRepository customerRepository,
        CustomerService customerService)
    {
        _customerRepository = customerRepository;
        _movieRepository = movieRepository;
        _customerService = customerService;
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
            Status = customer.Status.ToString(),
            StatusExpirationDate = customer.StatusExpirationDate,
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

        return Json(dto);
    }

    [HttpGet]
    public JsonResult GetList()
    {
        var customers = _customerRepository.GetList();

        var dto = customers.Select(x => new CustomerInListDto
        {
            Id = x.Id,
            Name = x.Name.Value,
            Email = x.Email.Value,
            Status = x.Status.ToString(),
            StatusExpirationDate = x.StatusExpirationDate
        }).ToList();

        return Json(dto);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateCustomerDto item)
    {
        try
        {
            var customerNameResult = CustomerName.Create(item.Name);
            var emailResult = Email.Create(item.Email);

            var result = Result.Combine(customerNameResult, emailResult);

            if (result.IsFailure) return BadRequest(result.Error);

            if (_customerRepository.GetByEmail(emailResult.Value) != null)
                return BadRequest("Email is already in use: " + item.Email);

            var customer = new Customer
            {
                Name = customerNameResult.Value,
                Email = emailResult.Value,
                MoneySpent = Dollars.Of(0),
                Status = CustomerStatus.Regular
            };

            _customerRepository.Add(customer);
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
    {
        try
        {
            var customerNameResult = CustomerName.Create(item.Name);

            if (customerNameResult.IsFailure) return BadRequest(customerNameResult.Error);

            var customer = _customerRepository.GetById(id);
            if (customer == null) return BadRequest("Invalid customer id: " + id);

            customer.Name = customerNameResult.Value;
            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPost]
    [Route("{id}/movies")]
    public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
    {
        try
        {
            var movie = _movieRepository.GetById(movieId);
            if (movie == null) return BadRequest("Invalid movie id: " + movieId);

            var customer = _customerRepository.GetById(id);
            if (customer == null) return BadRequest("Invalid customer id: " + id);

            if (customer.PurchasedMovies.Any(x => x.MovieId == movie.Id && !x.ExpirationDate.IsExpired))
                return BadRequest("The movie is already purchased: " + movie.Name);

            _customerService.PurchaseMovie(customer, movie);

            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPost]
    [Route("{id}/promotion")]
    public IActionResult PromoteCustomer(long id)
    {
        try
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null) return BadRequest("Invalid customer id: " + id);

            if (customer is { Status: CustomerStatus.Advanced, StatusExpirationDate.IsExpired: false })
                return BadRequest("The customer already has the Advanced status");

            var success = _customerService.PromoteCustomer(customer);
            if (!success) return BadRequest("Cannot promote the customer");

            _customerRepository.SaveChanges();

            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }
}