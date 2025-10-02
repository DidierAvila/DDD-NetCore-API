using AutoMapper;
using Platform.Domain.DTOs.App;
using Platform.Domain.Entities.App;

namespace Platform.Application.Mappings.App
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            // Entity to DTO mappings
            CreateMap<Service, ServiceDto>();

            CreateMap<Service, ServiceSummaryDto>();

            CreateMap<Service, ServiceDropdownDto>();

            // DTO to Entity mappings
            CreateMap<CreateServiceDto, Service>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateServiceDto, Service>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}