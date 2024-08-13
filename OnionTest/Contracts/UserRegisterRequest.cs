using System.ComponentModel.DataAnnotations;

namespace OnionTest.Contracts
{
    public record UserRegisterRequest(
        [Required] string username,
        [Required] string email,
        [Required] string password);

}
