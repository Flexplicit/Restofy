using System.ComponentModel.DataAnnotations;

namespace PublicApiDTO.v1.v1.AuthenticationDTO
{
    public class Register
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        [MinLength(2)]
        public string Firstname { get; set; } = default!;
        [MinLength(2)]
        public string Lastname { get; set; } = default!;
    }
}