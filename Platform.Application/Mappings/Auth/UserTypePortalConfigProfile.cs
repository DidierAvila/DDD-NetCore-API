using AutoMapper;
using Platform.Domain.DTOs.Auth;
using Platform.Domain.Entities.Auth;
using System.Text.Json;

namespace Platform.Application.Mappings.Auth
{
    public class UserTypePortalConfigProfile : Profile
    {
        public UserTypePortalConfigProfile()
        {
            // Entity to DTO mappings
            CreateMap<UserTypePortalConfig, UserTypePortalConfigDto>()
                .ForMember(dest => dest.AdditionalConfig, opt => opt.MapFrom(src => 
                     ParseAdditionalConfig(src.AdditionalConfig)));

            // DTO to Entity mappings
            CreateMap<CreateUserTypePortalConfigDto, UserTypePortalConfig>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            CreateMap<UpdateUserTypePortalConfigDto, UserTypePortalConfig>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            // Nota: AdditionalConfig se deserializa automáticamente desde JSON
            // y contiene toda la configuración personalizable incluyendo menús
        }

        private static JsonElement? ParseAdditionalConfig(string? additionalConfig)
        {
            if (string.IsNullOrEmpty(additionalConfig))
                return null;

            try
            {
                using var document = JsonDocument.Parse(additionalConfig);
                return document.RootElement.Clone();
            }
            catch
            {
                return null;
            }
        }
    }
}
