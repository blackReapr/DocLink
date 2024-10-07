using Microsoft.AspNetCore.Http;

namespace DocLink.Application.Extensions
{
    public static partial class Extension
    {
        public static bool IsImage(this IFormFile formFile) => formFile.ContentType.Contains("image");
        public static bool DoesSizeExceed(this IFormFile formFile, int size) => formFile.Length / 1024 > size;
        public static async Task<string> SaveFileAsync(this IFormFile formFile)
        {
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", filename);
            using FileStream fileStream = new(filePath, FileMode.Create);
            await formFile.CopyToAsync(fileStream);
            return filename;
        }
    }
}
