using System.ComponentModel.DataAnnotations;

namespace Platform.Domain.Utils
{
    public static class UtilsEnum
    {
        public static string GetDisplayName(this Enum value)
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());

            // Si no hay información de miembro, devuelve el nombre de la constante (string)
            if (memberInfo.Length == 0) 
                return value.ToString();

            // Busca el atributo [Display]
            var displayAttribute = Attribute.GetCustomAttribute(memberInfo[0], typeof(DisplayAttribute)) as DisplayAttribute;

            // Devuelve el 'Name' si está definido, de lo contrario, devuelve el nombre de la constante.
            return displayAttribute?.Name ?? value.ToString();
        }
    }
}
