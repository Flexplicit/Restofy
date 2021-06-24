using Microsoft.AspNetCore.Identity;
#pragma warning disable 1591

namespace WebApp.Areas.IdentityErrors
{
    /// <summary>
    /// Overrides original ASP.NET common Identity errors so we could modify them according to resources(localization).
    /// </summary>
    public  class LocalizedCustomIdentityErrorDescriber : Microsoft.AspNetCore.Identity.IdentityErrorDescriber
    { public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"An unknown failure has occurred." }; }
    public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Optimistic concurrency failure, object has been modified." }; }
    public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = Resources.Identity.IdentityForm.LoginRelatedValidation.InvalidToken }; }
    public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = Resources.Identity.IdentityForm.LoginRelatedValidation.LoginAlreadyAssociated }; }
    public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = Resources.Identity.IdentityForm.LoginRelatedValidation.InvalidUserName }; }
    public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = Resources.Identity.IdentityForm.LoginRelatedValidation.InvalidEmail  }; }
    
    public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = Resources.Identity.IdentityForm.LoginRelatedValidation.DuplicateUserName }; }
    public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description =  Resources.Identity.IdentityForm.LoginRelatedValidation.DuplicateEmail  }; }
    public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = Resources.Identity.IdentityForm.UserAndRole.InvalidRoleName  }; }
    public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = Resources.Identity.IdentityForm.UserAndRole.DuplicateRoleName }; }
    
    public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = Resources.Identity.IdentityForm.UserAndRole.UserAlreadyHasPassword }; }
    public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = Resources.Identity.IdentityForm.UserAndRole.UserLockoutNotEnabled }; }
    public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = Resources.Identity.IdentityForm.UserAndRole.UserAlreadyInRole}; }
    public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = Resources.Identity.IdentityForm.UserAndRole.UserNotInRole}; }
    
    public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = Resources.Identity.IdentityForm.PasswordValidation.PasswordMisMatch }; }
    public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = Resources.Identity.IdentityForm.PasswordValidation.PasswordLength}; }
    public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = Resources.Identity.IdentityForm.PasswordValidation.PasswordRequiresNonAlphanumeric}; }
    public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = Resources.Identity.IdentityForm.PasswordValidation.PasswordRequiresDigit}; }
    public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = Resources.Identity.IdentityForm.PasswordValidation.PasswordRequiresLower}; }
    public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = Resources.Identity.IdentityForm.PasswordValidation.PasswordRequiresUpper}; }
    }
}