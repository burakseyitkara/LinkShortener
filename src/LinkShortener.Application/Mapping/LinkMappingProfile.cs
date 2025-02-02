using AutoMapper;
using LinkShortener.Application.DTOs;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Application.Mapping;

/// <summary>
/// Link entity'si için AutoMapper profili
/// </summary>
public class LinkMappingProfile : Profile
{
    public LinkMappingProfile()
    {
        CreateMap<Link, LinkDto>();
        CreateMap<CreateLinkDto, Link>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.ClickCount, opt => opt.MapFrom(src => 0L));

        CreateMap<LinkClick, LinkClickDto>();
        CreateMap<CreateLinkClickDto, LinkClick>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
    }
} 