using AutoMapper;
using LinkShortener.Application.DTOs;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Application.Mapping;

/// <summary>
/// User entity'si için AutoMapper profili
/// </summary>
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.IsEmailVerified, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
} 