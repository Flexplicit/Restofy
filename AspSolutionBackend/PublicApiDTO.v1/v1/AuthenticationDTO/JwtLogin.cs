using System.Collections.Generic;

namespace PublicApiDTO.v1.v1.AuthenticationDTO
{
    public class JwtLogin
    {
        public string Token { get; set; } = default!;
        public string Firstname { get; set; } = default!;
        public string Lastname { get; set; } = default!;
        public IList<string> Roles { get; set; } = default!;
    }
}