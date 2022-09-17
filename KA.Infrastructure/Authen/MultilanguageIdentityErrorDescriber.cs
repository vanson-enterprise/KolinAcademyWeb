using KA.Infrastructure.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;


namespace KA.Infrastructure.Authen
{
    public class MultilanguageIdentityErrorDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public MultilanguageIdentityErrorDescriber(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError DuplicateUserName(string userName)
        {

            return new IdentityError()
            {
                Code = nameof(DuplicateUserName),
                Description = _localizer["DuplicateUserName", userName]
            };
        }
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = nameof(InvalidUserName),
                Description = _localizer["InvalidUserName", userName]
            };
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = nameof(DuplicateEmail),
                Description = _localizer["DuplicateEmail", email]
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordRequiresDigit),
                Description = string.Format(_localizer["PasswordRequiresDigit"])
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordRequiresDigit),
                Description = string.Format(_localizer["PasswordRequiresLower"])
            };
        }
    }
}
