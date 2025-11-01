using System.ComponentModel.DataAnnotations;

namespace Platform.Domain.Utils
{
    /// <summary>
    /// UserTypeEnum
    /// </summary>
    public enum EnumUserType
    {
        [Display(Name = "Consultor")]
        Consultant,
        [Display(Name = "Asistente")]
        Assistant,
        [Display(Name = "Proveedor")]
        Supplier,
        [Display(Name = "Cliente")]
        Client
    }
}
