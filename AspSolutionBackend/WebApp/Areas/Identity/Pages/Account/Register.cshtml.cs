using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Domain.Identity;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

#pragma warning disable 1591

namespace WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty] public InputModel Input { get; set; } = null!;

        public string? ReturnUrl { get; set; }

        public IList<AuthenticationScheme>? ExternalLogins { get; set; }

        public class InputModel
        {
            // 2 diff error messages
            [Required(ErrorMessageResourceName = "RequiredFieldForm", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [EmailAddress(ErrorMessageResourceName = "RequiredEmailValidation", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [Display(ResourceType = typeof(Resources.Identity.AppUser), Name = nameof(Email))]
            public string Email { get; set; } = null!;

            [Required(ErrorMessageResourceName = "RequiredFieldForm", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "FieldLength", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [DataType(DataType.Password)]
            [Display(ResourceType = typeof(Resources.Identity.AppUser), Name = nameof(Password))]
            public string Password { get; set; } = null!;

            [DataType(DataType.Password)]
            [Required(ErrorMessageResourceName = "RequiredFieldForm", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [Display(ResourceType = typeof(Resources.Identity.AppUser), Name = nameof(ConfirmPassword))]
            [Compare(nameof(Password), ErrorMessageResourceName = "ComparisonMatch", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            public string ConfirmPassword { get; set; } = null!;

            [Required(ErrorMessageResourceName = "RequiredFieldForm", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [StringLength(128, MinimumLength = 1, ErrorMessageResourceName = "FieldLength", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [Display(ResourceType = typeof(Resources.Identity.AppUser), Name = nameof(FirstName))]
            public String FirstName { get; set; } = null!;

            [Required(ErrorMessageResourceName = "RequiredFieldForm", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [StringLength(128, MinimumLength = 1, ErrorMessageResourceName = "FieldLength", ErrorMessageResourceType = typeof(Resources.Common.CommonForm))]
            [Display(ResourceType = typeof(Resources.Identity.AppUser), Name = nameof(LastName))]
            public String LastName { get; set; } = null!;
        }

        public async Task OnGetAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new {area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl},
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new {email = Input.Email, returnUrl = returnUrl});
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}