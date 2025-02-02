using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrHub.Application.Helpers
{
    public static class FileHelper
    {
        public static async Task<bool> SaveFileAsync(string directoryPath, string fileName, byte[] fileContent)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
            {
                return false;
            }

            await File.WriteAllBytesAsync(filePath, fileContent);
            return true;
        }
    }
}
