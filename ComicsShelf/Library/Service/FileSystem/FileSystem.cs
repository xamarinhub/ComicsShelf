﻿using System.Threading.Tasks;

namespace ComicsShelf.Library.Implementation
{
   internal partial class FileSystemService : ILibraryService
   {

      Helpers.iFileSystem FileSystem { get; set; }
      public FileSystemService()
      {
         this.FileSystem = Helpers.FileSystem.Get();
      }

      public async Task ExtractCoverAsync(Helpers.Database.Library library, Helpers.Database.ComicFile comicFile)
      {
         await this.FileSystem.CoverExtract(App.Settings, App.Database, comicFile);
      }

      public async Task ExtractPagesAsync(Helpers.Database.Library library, Views.File.FileData fileData)
      {
         await this.FileSystem.PagesExtract(App.Settings, fileData);
      }

   }
}