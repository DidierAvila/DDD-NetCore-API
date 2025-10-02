using AutoMapper;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Mappings.Auth
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            // Entity to DTO mappings
            CreateMap<Permission, PermissionDto>();

            CreateMap<Permission, PermissionSummaryDto>()
                .ForMember(dest => dest.RoleCount, opt => opt.MapFrom(src => src.Roles.Count));

            CreateMap<Permission, PermissionListResponseDto>()
                .ForMember(dest => dest.RoleCount, opt => opt.MapFrom(src => src.Roles.Count));

            CreateMap<Permission, PermissionWithRolesDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));

            CreateMap<Permission, PermissionDropdownDto>();

            // DTO to Entity mappings
            CreateMap<CreatePermissionDto, Permission>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<UpdatePermissionDto, Permission>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RolePermissions, opt => opt.Ignore())
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.MenuPermissions, opt => opt.Ignore())
                .ForMember(dest => dest.Menus, opt => opt.Ignore());
                // AutoMapper mapea automáticamente: Name, Description
        }
    }
}
