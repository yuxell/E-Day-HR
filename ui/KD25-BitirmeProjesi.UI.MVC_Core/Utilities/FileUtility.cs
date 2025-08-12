namespace KD25_BitirmeProjesi.UI.MVC_Core.Utilities
{
    public static class FileUtility
    {
        public static string GetGuidID()
        {
            return Guid.NewGuid().ToString();
        }

        public static async Task<string> SaveFileAsync(IFormFile file)
        {
            string fileName = GetGuidID() + "_" + file.FileName;
            FileStream fileStream = new FileStream("wwwroot/img/" + fileName, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Close();
            return fileName;
        }

        public static void DeleteFile(string fileName)
        {
            string filePath = Path.Combine("wwwroot/img", fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static async Task<string> SaveFolderAsync(IFormFile file)
        {

            // Dosya adı benzersiz hale getirilir
            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName); // Orijinal uzantıyı koruyun

            // wwwroot/folders/ dizinine kaydetmek için tam yol oluşturulur
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "folders");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string filePath = Path.Combine(folderPath, fileName);

            // Dosyayı wwwroot/folders dizinine kaydeder
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Yalnızca dosya adını döndürüyoruz (Veritabanında veya dosya yolu için kullanılacak)
            return fileName;


        }

        public static void DeleteFolder(string fileName)
        {
            string filePath = Path.Combine("wwwroot/folder", fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
