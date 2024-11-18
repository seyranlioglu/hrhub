using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HrHub.Abstraction.Extensions
{
    public static class ValidationResultExtensions
    {
        public static string MessagesToJson(this IEnumerable<(bool IsValid, string ErrorMessage)> validationResults)
        {
            // Hataları filtrele ve JSON formatına dönüştür
            var errors = new List<object>();

            foreach (var result in validationResults)
            {
                if (!result.IsValid)
                {
                    // Hata mesajını JSON formatına ekle
                    var error = JsonSerializer.Deserialize<Dictionary<string, string>>(result.ErrorMessage);
                    errors.Add(error);
                }
            }

            // Hataları JSON formatına serialize et
            return JsonSerializer.Serialize(errors, new JsonSerializerOptions
            {
                WriteIndented = true // Daha okunabilir format için.
            });
        }
    }
}
