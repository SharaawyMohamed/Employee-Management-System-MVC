using Microsoft.AspNetCore.Http;
using System;
using System.IO;
namespace Demo.PL.Utility
{
	public static class DocumentSetting
	{
		// Upload 
		public static string UploadFile(IFormFile file, string FolderName)
		{
			// to upload file we have 5 steps

			// 1. Get located folder path
			//string FolderPath = Directory.GetCurrentDirectory() + " \\wwwroot\\Files" + FolderName;
			string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

			// 2. Get File Name and Make It Unique
			//string FileType = file.Name;// get type for this file
			string FileName = $"{Guid.NewGuid()}{file.FileName}";// get file name

			// 3. Get File Path[FolderName + FileName]
			string FilePath = Path.Combine(FolderPath, FileName);

			// 4. Save file as Stream     {Stream}--> as video combine of multiple images
			// FS ---> FileStream
			using (var FS = new FileStream(FilePath, FileMode.Create))// Syntax Shoger
			{
				file.CopyTo(FS);
			}

			// 5. Return FileName
			return FileName;
		}
		// Delete
		public static void DeleteFile(string FolderName,string FileName)
		{
			//1. Get File Path
			string FilePath= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files",FolderName, FileName);
			
			//2. Check if file Exist or not
			if(File.Exists(FilePath))
			{
			   File.Delete(FilePath);//(File.Path) take path and delete it
			}
		}
	}
}
