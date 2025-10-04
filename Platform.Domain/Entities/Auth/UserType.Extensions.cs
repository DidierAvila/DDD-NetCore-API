using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Platform.Domain.Entities.Auth
{
    public partial class UserType
    {
        private Dictionary<string, object>? _additionalData;

        /// <summary>
        /// Obtiene los datos adicionales deserializados del campo AdditionalConfig
        /// </summary>
        [NotMapped]
        public Dictionary<string, object> AdditionalData
        {
            get
            {
                if (_additionalData == null)
                {
                    try
                    {
                        _additionalData = string.IsNullOrWhiteSpace(AdditionalConfig) 
                            ? new Dictionary<string, object>() 
                            : JsonSerializer.Deserialize<Dictionary<string, object>>(AdditionalConfig) ?? new Dictionary<string, object>();
                    }
                    catch (JsonException)
                    {
                        _additionalData = new Dictionary<string, object>();
                    }
                }
                return _additionalData;
            }
            set
            {
                _additionalData = value;
                AdditionalConfig = JsonSerializer.Serialize(value ?? new Dictionary<string, object>());
            }
        }

        /// <summary>
        /// Verifica si existe un valor en los datos adicionales
        /// </summary>
        public bool HasAdditionalValue(string key)
        {
            return AdditionalData.ContainsKey(key);
        }

        /// <summary>
        /// Obtiene un valor de los datos adicionales
        /// </summary>
        public T? GetAdditionalValue<T>(string key)
        {
            if (!HasAdditionalValue(key))
                return default;

            var value = AdditionalData[key];
            
            if (value is JsonElement jsonElement)
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(jsonElement.GetRawText());
                }
                catch
                {
                    return default;
                }
            }
            
            try
            {
                return (T)value;
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Establece un valor en los datos adicionales
        /// </summary>
        public void SetAdditionalValue<T>(string key, T value)
        {
            AdditionalData[key] = value ?? (object)string.Empty;
            AdditionalConfig = JsonSerializer.Serialize(AdditionalData);
        }
    }
}