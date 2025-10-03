namespace Platform.Domain.DTOs.Auth
{
    /// <summary>
    /// DTO optimizado para dropdowns/listas desplegables de usuarios (máximo rendimiento)
    /// </summary>
    public class UserDropdownDto
    {
        public Guid Value { get; set; }
        public string Label { get; set; } = null!;
    }
}