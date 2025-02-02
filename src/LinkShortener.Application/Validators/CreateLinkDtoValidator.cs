using FluentValidation;
using LinkShortener.Application.DTOs;
using LinkShortener.Domain.Services;

namespace LinkShortener.Application.Validators;

/// <summary>
/// CreateLinkDto için validator
/// </summary>
public class CreateLinkDtoValidator : AbstractValidator<CreateLinkDto>
{
    private readonly ILinkService _linkService;

    public CreateLinkDtoValidator(ILinkService linkService)
    {
        _linkService = linkService;

        RuleFor(x => x.OriginalUrl)
            .NotEmpty().WithMessage("Orijinal URL boş olamaz")
            .Must(url => _linkService.ValidateUrl(url)).WithMessage("Geçersiz URL formatı");

        RuleFor(x => x.Title)
            .MaximumLength(100).WithMessage("Başlık en fazla 100 karakter olabilir");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");

        RuleFor(x => x.CustomShortCode)
            .MustAsync(async (shortCode, cancellation) =>
            {
                if (string.IsNullOrEmpty(shortCode))
                    return true;

                return await _linkService.ValidateShortCode(shortCode);
            })
            .WithMessage("Geçersiz kısa kod formatı veya bu kod zaten kullanımda");

        RuleFor(x => x.ExpiresAt)
            .Must(expiresAt => !expiresAt.HasValue || expiresAt.Value > DateTime.UtcNow)
            .WithMessage("Son kullanma tarihi gelecekte bir tarih olmalıdır");
    }
} 