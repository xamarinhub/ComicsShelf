﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicsShelf.Engine
{
   internal class Cover : BaseEngine
   {

      #region Execute
      public static async void Execute()
      {
         try
         {
            Console.WriteLine("Cover Engine Start");
            using (var engine = new Cover())
            {
               await engine.ExtractComicData();
               Xamarin.Forms.Device.BeginInvokeOnMainThread(() => Statistics.Execute());
            }
            Console.WriteLine("Cover Engine Finish");
         }
         catch (Exception ex) { await App.Message.Show(ex.ToString()); }
      }
      #endregion

      #region ExtractComicData
      private async Task ExtractComicData()
      {
         try
         {
            this.Notify(R.Strings.STARTUP_EXTRACTING_COMICS_COVER_MESSAGE);

            // LOOP THROUGH FILES
            var filesQuantity = App.RootFolder.Files.Count;
            for (int fileIndex = 0; fileIndex < filesQuantity; fileIndex++)
            {
               var file = App.RootFolder.Files[fileIndex];
               var progress = ((double)fileIndex / (double)filesQuantity);
               this.Notify(file.Text, progress);

               if ((progress % (double)10) == 0)
               { Xamarin.Forms.Device.BeginInvokeOnMainThread(() => Statistics.Execute()); }

               if (!await this.DataAlreadyExists(file))
               {
                  await Task.Run(() => this.ExtractFileData(file));
                  await Task.Run(() => this.ExtractFolderData(file));
               }
            }

         }
         catch (Exception ex) { throw; }
      }
      #endregion

      #region DataAlreadyExists
      private async Task< bool> DataAlreadyExists(File.FileData file)
      {
         if (file.PersistentData != null && string.IsNullOrEmpty(file.PersistentData.ReleaseDate)) { return false; }
         if (await Task.Run(() => System.IO.File.Exists(file.CoverPath))) { return true; }
         if (file.FullPath.ToLower().EndsWith(".cbr")) { return true; }
         return false;
      }
      #endregion

      #region ExtractFileData
      private async Task ExtractFileData(File.FileData file)
      {
         try
         {

            // OPEN ZIP ARCHIVE
            using (var zipArchive = await this.FileSystem.GetZipArchive(App.Settings, file))
            {

               // LOOP THROUGH ENTIES LOOKING FOR THE FIRST IMAGE
               var zipEntries = zipArchive.Entries
                  .Where(x => x.Name.ToLower().EndsWith(".jpg"))
                  .OrderBy(x => x.Name)
                  .ToList();
               var zipEntry = zipEntries.FirstOrDefault();
               using (System.IO.Stream zipStream = zipEntry.Open())
               {
                  if (file.PersistentData != null && string.IsNullOrEmpty(file.PersistentData.ReleaseDate))
                  {
                     file.PersistentData.ReleaseDate = App.Database.GetDate(zipEntry.LastWriteTime.DateTime.ToLocalTime());
                     App.Database.Update(file.PersistentData);
                  }
                  await this.FileSystem.Thumbnail(zipStream, file.CoverPath);
               }
            }

         }
         catch (Exception ex) { throw; }
         finally { GC.Collect(); }
      }
      #endregion

      #region ExtractFolderData
      private async Task ExtractFolderData(File.FileData file)
      {
         try
         {
            /*

            // APPLY COVER PATH TO THE FOLDER STRUCTURE
            var parentFolder = this.ComicFoldersDictionary[file.PersistentData.ParentPath];
            while (parentFolder != null)
            {
               if (!string.IsNullOrEmpty(parentFolder.PersistentData.CoverPath)) { break; }

               parentFolder.CoverPath = file.CoverPath;
               parentFolder.PersistentData.CoverPath = file.CoverPath;
               await Task.Run(() => App.Database.Update(parentFolder.PersistentData));

               if (string.IsNullOrEmpty(parentFolder.PersistentData.ParentPath)) { break; }
               parentFolder = this.ComicFoldersDictionary[parentFolder.PersistentData.ParentPath];
            }

            */
         }
         catch (Exception ex) { throw; }
      }
      #endregion

   }
}