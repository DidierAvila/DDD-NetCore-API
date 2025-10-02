using System.Text.Json;

namespace Platform.Domain.DTOs.Auth
{
    public class UserTypePortalConfigDto
    {
        public Guid Id { get; set; }
        public Guid UserTypeId { get; set; }
        public string? CustomLabel { get; set; }
        public string? CustomIcon { get; set; }
        public string? CustomRoute { get; set; }
        public string Theme { get; set; } = "default";
        public string? DefaultLandingPage { get; set; }
        public string? LogoUrl { get; set; }
        public string Language { get; set; } = "es";
        /// <summary>
        /// Configuración adicional del portal en formato JSON. Puede incluir configuración de menús:
        /// { "menus": [{ "menuId": "guid", "label": "string", "icon": "string", "route": "string", "order": number, "status": boolean }] }
        /// </summary>
        public JsonElement? AdditionalConfig { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateUserTypePortalConfigDto
    {
        public Guid UserTypeId { get; set; }
        public bool Status { get; set; } = true;
        public string Theme { get; set; } = "default";
        /// <summary>
        /// Configuración adicional del portal. Puede incluir configuración de menús:
        /// { "menus": [{ "menuId": "guid", "label": "string", "icon": "string", "route": "string", "order": number, "status": boolean }] }
        /// </summary>
        public Dictionary<string, object>? AdditionalConfig { get; set; }
        public string? DefaultLandingPage { get; set; }
        public string? LogoUrl { get; set; }
        public string Language { get; set; } = "es";
    }

    public class UpdateUserTypePortalConfigDto
    {
        public bool Status { get; set; }
        public string Theme { get; set; } = "default";
        /// <summary>
        /// Configuración adicional del portal. Puede incluir configuración de menús:
        /// { "menus": [{ "menuId": "guid", "label": "string", "icon": "string", "route": "string", "order": number, "status": boolean }] }
        /// </summary>
        public Dictionary<string, object>? AdditionalConfig { get; set; }
        public string? DefaultLandingPage { get; set; }
        public string? LogoUrl { get; set; }
        public string Language { get; set; } = "es";
    }

    public class SimpleMenuConfigDto
    {
        public Guid MenuId { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool Status { get; set; }
    }
}
