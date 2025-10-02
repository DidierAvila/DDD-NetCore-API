using AutoMapper;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Mappings.Auth
{
    public class MenuPermissionProfile : Profile
    {
        public MenuPermissionProfile()
        {
            // Entity to DTO mappings
            CreateMap<MenuPermission, MenuPermissionDto>();

            // DTO to Entity mappings
            CreateMap<CreateMenuPermissionDto, MenuPermission>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Menu, opt => opt.Ignore())
                .ForMember(dest => dest.Permission, opt => opt.Ignore());
        }
    }
}
