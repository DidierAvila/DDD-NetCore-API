using AutoMapper;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;

namespace Platform.Application.Mappings.Auth
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            // Entity to DTO mappings
            CreateMap<Menu, MenuDto>()
                .ForMember(dest => dest.Children, opt => opt.Ignore())
                .ForMember(dest => dest.Permissions, opt => opt.Ignore());

            CreateMap<Menu, MenuBasicDto>();

            CreateMap<Menu, MenuTreeDto>()
                .ForMember(dest => dest.Children, opt => opt.Ignore());

            // DTO to Entity mappings
            CreateMap<CreateMenuDto, Menu>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.MenuPermissions, opt => opt.Ignore());

            CreateMap<UpdateMenuDto, Menu>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.MenuPermissions, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
