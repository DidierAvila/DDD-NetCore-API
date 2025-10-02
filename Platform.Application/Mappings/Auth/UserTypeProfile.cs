using AutoMapper;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Mappings.Auth
{
    public class UserTypeProfile : Profile
    {
        public UserTypeProfile()
        {
            // Entity to DTO mappings
            CreateMap<UserType, UserTypeDto>();
            
            CreateMap<UserType, UserTypeDropdownDto>();

            CreateMap<UserType, UserTypeSummaryDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

            CreateMap<UserType, UserTypeListResponseDto>()
                .ForMember(dest => dest.UserCount, opt => opt.MapFrom(src => src.Users.Count));

            // DTO to Entity mappings
            CreateMap<CreateUserTypeDto, UserType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            // AutoMapper mapea automáticamente: Name, Description, Status
            // AutoMapper ignora automáticamente: Users

            CreateMap<UpdateUserTypeDto, UserType>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));
            // AutoMapper mapea automáticamente: Name, Description, Status
        }
    }
}
