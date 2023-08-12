using Entity.DTOs;
using FluentValidation;

namespace Business.Validation
{
    public class UserForRegisterValidator : AbstractValidator<UserForRegisterDto>
    {
        public UserForRegisterValidator()
        {
            RuleFor(user => user.Login)
                .MaximumLength(100)
                .WithMessage("Kullanıcı adı max 100 karakterden oluşabilir.");

            RuleFor(user => user.Password)
                .MinimumLength(8)
                .WithMessage("Şifre En Az 8 karakter olmadılır.");
        }
    }
}
