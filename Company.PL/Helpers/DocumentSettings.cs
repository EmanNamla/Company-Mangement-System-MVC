using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Company.PL.Helpers
{
    public static class DocumentSettings
    {
        public static string UploadFile(IFormFile file,string FolderName)
        {
          
            //1.
            string FolderPath=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files", FolderName);

            //2.
            string FileName=$"{ Guid.NewGuid()}{file.FileName}";

            //3.
            string FilePath=Path.Combine(FolderPath,FileName);

            //4.
            using var fs=new FileStream(FilePath,FileMode.Create);
            file.CopyTo(fs);

            
            //5.
           return FileName;
            
        }


        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName,FileName   );
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }



    }
}
