using FluentValidation;
using LinkShortener.Application.DTOs;
using LinkShortener.Domain.Services;

namespace LinkShortener.Application.Validators;

/// <summary>
/// CreateUserDto için validator
/// </summary>
public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    private readonly IUserService _userService;

    public CreateUserDtoValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Kullanıcı adı boş olamaz")
            .Must(username => _userService.ValidateUsername(username))
            .WithMessage("Geçersiz kullanıcı adı formatı");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-posta adresi boş olamaz")
            .Must(email => _userService.ValidateEmail(email))
            .WithMessage("Geçersiz e-posta adresi formatı");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz")
            .MustAsync(async (password, cancellation) => await _userService.ValidatePassword(password))
            .WithMessage("Şifre en az 8 karakter uzunluğunda olmalı ve en az bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Şifre tekrarı boş olamaz")
            .Equal(x => x.Password).WithMessage("Şifreler eşleşmiyor");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz")
            .MaximumLength(50).WithMessage("Ad en fazla 50 karakter olabilir");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz")
            .MaximumLength(50).WithMessage("Soyad en fazla 50 karakter olabilir");
    }
} 