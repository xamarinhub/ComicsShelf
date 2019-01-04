﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.OneDrive.Files;

namespace ComicsShelf.Library.Implementation
{
   internal partial class OneDrive : ILibraryService
   {

      Xamarin.OneDrive.Connector Connector { get; set; }
      public OneDrive()
      {
         var clientID = System.Environment.GetEnvironmentVariable("ComicsShelfApplicationID");
         this.Connector = new Xamarin.OneDrive.Connector(clientID, "User.Read", "Files.Read");
      }

      public async Task ExtractCoverAsync(Helpers.Database.Library library, Helpers.Database.ComicFile comicFile)
      {
         try
         {
            var downloadUrl = await this.Connector.GetDownloadUrlAsync(new FileData { id = comicFile.Key });
            using (var zipStream = new System.IO.Compression.HttpZipStream(downloadUrl))
            {
               if (comicFile.StreamSize > 0) { zipStream.SetContentLength(comicFile.StreamSize); }
               var entryList = await zipStream.GetEntriesAsync();
               var entry = entryList
                  .Where(x =>
                     x.FileName.ToLower().EndsWith(".jpg") ||
                     x.FileName.ToLower().EndsWith(".jpeg") ||
                     x.FileName.ToLower().EndsWith(".png"))
                  .OrderBy(x => x.FileName)
                  .FirstOrDefault();
               await zipStream.ExtractAsync(entry, async (entryStream) => {

                  using (var thumbnailFile = new System.IO.FileStream(comicFile.CoverPath, System.IO.FileMode.CreateNew, System.IO.FileAccess.Write))
                  {
                     await entryStream.CopyToAsync(thumbnailFile);
                     await thumbnailFile.FlushAsync();
                     thumbnailFile.Close();
                     thumbnailFile.Dispose();
                  }

               });
            }
         }
         catch (Exception) { throw; }
      }

      public async Task ExtractPagesAsync(Helpers.Database.Library library, Views.File.FileData fileData)
      {
      }

   }
}