using System.ComponentModel.DataAnnotations;

namespace Logic.Dtos;

public class CreateCustomerDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name is too long")]
    public string Name { get; set; }

    [Required]
    [RegularExpression(@"^(.+)@(.+)$", ErrorMessage = "Email is invalid")]
    public string Email { get; set; }
}