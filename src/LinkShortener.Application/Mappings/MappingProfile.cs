using AutoMapper;
using LinkShortener.Application.DTOs;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Application.Mappings;

/// <summary>
/// AutoMapper profil sınıfı
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Link eşleştirmeleri
        CreateMap<Link, LinkDto>();
        CreateMap<CreateLinkDto, Link>()
            .ForMember(dest => dest.ShortCode, opt => opt.MapFrom(src => src.CustomShortCode))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow));

        // Kullanıcı eşleştirmeleri
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => System.DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.IsEmailVerified, opt => opt.MapFrom(src => false));

        // Link tıklanma kaydı eşleştirmeleri
        CreateMap<LinkClick, LinkClickDto>();
    }
} 