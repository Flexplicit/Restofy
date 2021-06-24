using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PublicApiDTO.v1.v1.AuthenticationDTO;
using DomainDTO = Domain.Identity;


namespace WebApp.ApiControllers.Identity
{
    /// <summary>
    ///  Current api handles logging in and making new requests
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<DomainDTO.AppUser> _userManager;
        private readonly SignInManager<DomainDTO.AppUser> _signInManager;
        private readonly ILogger<AccountController>? _logger;
        private readonly IConfiguration _cfg;

        /// <summary>
        /// Constructor for AccountManagement
        /// </summary>
        /// <param name="userManager">Injected Via DI</param>
        /// <param name="signInManager">Injected Via DI</param>
        /// <param name="logger">Injected Via DI</param>
        /// <param name="cfg">Injected Via DI</param>
        public AccountController(UserManager<DomainDTO.AppUser> userManager,
            SignInManager<DomainDTO.AppUser> signInManager,
            ILogger<AccountController>? logger, IConfiguration cfg)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _cfg = cfg;
        }

        /// <summary>
        /// Logs in the user via the post request
        /// </summary>
        /// <param name="loginDTO">Object needed to validate the login</param>
        /// <returns>Object with a jwt token</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(JwtLogin), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadLoginMessageDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] Login loginDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BadLoginMessageDTO("No input"));
            }

            var appUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (appUser == null)
            {
                _logger.LogWarning("Bad WebApi Login to {User} with password {Password}", loginDTO.Email,
                    loginDTO.Password);
                return BadRequest(new BadLoginMessageDTO("Bad user login. Wrong username or password"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginDTO.Password, false);
            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
                var roles = await _userManager.GetRolesAsync(appUser);
                var jwtToken = GetCurrentUserJwt(claimsPrincipal);

                _logger.LogWarning("Web API login successful with {User}", loginDTO.Email);
                return Ok(new JwtLogin
                    {Roles = roles, Firstname = appUser.FirstName, Lastname = appUser.LastName, Token = jwtToken});
            }

            _logger.LogWarning("Very Bad WebApi Login to {User} with password {Password}", loginDTO.Email,
                loginDTO.Password);
            return BadRequest(new BadLoginMessageDTO("Bad user login. Wrong username or password"));
        }

        /// <summary>
        /// Registers an user with posted object's details
        /// </summary>
        /// <param name="registerDTO">Object with fields that are necessary for registering a new user</param>
        /// <returns>Object with a jwt token</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(JwtLogin), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadLoginMessageDTO), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] Register registerDTO)
        {
            var appUser = await _userManager.FindByEmailAsync(registerDTO.Email);
            if (appUser != null)
            {
                _logger.LogWarning("Creating an account that already exists {User}", registerDTO.Email);
                return BadRequest(new BadLoginMessageDTO("Account already exists"));
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("All fields are required ");
                return BadRequest(new BadLoginMessageDTO("Missing fields"));
            }

            var newAppUser = new DomainDTO.AppUser
            {
                UserName = registerDTO.Email,
                Email = registerDTO.Email,
                FirstName = registerDTO.Firstname,
                LastName = registerDTO.Lastname
            };
            var res = _userManager.CreateAsync(newAppUser, registerDTO.Password).Result;
            if (!res.Succeeded)
            {
                return BadRequest(new BadLoginMessageDTO(res.Errors.Select(x => x.Code).ToArray()));
            }

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(newAppUser);

            var jwtToken = GetCurrentUserJwt(claimsPrincipal);

            _logger.LogWarning("Web API login successful with {User}", registerDTO.Email);
            return Ok(new JwtLogin
            {
                Roles = new List<string>(),
                Firstname = newAppUser.FirstName,
                Lastname = newAppUser.LastName,
                Token = jwtToken
            });
        }

        private string GetCurrentUserJwt(ClaimsPrincipal claimsPrincipal)
        {
            return Extensions.Base.IdentityExtensions.GenerateJwt(
                claimsPrincipal.Claims,
                _cfg["Jwt:JwtKey"],
                _cfg["Jwt:JwtIssuer"],
                _cfg["Jwt:JwtIssuer"],
                DateTime.Now.AddDays(_cfg.GetValue<int>("Jwt:ValidDayCount")));
        }
    }
}