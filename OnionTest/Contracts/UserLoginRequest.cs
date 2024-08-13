using System.ComponentModel.DataAnnotations;

namespace OnionTest.Contracts
{
    public record UserLoginRequest(
        [Required]string email,
        [Required]string password);

}
