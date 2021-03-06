﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace ComicsShelf.Droid
{
   partial class FileSystem
   {

      public async Task SaveThumbnail(Stream imageStream, string imagePath)
      {
         try
         {
            // LOAD IMAGE
            using (var originalBitmap = await Android.Graphics.BitmapFactory.DecodeStreamAsync(imageStream))
            {
               if (originalBitmap == null) { return; }

               // DEFINE SIZE
               double imageHeight = 450; double imageWidth = 150;
               double scaleFactor = (double)imageHeight / (double)originalBitmap.Height;
               imageHeight = originalBitmap.Height * scaleFactor;
               imageWidth = originalBitmap.Width * scaleFactor;

               // INITIALIZE THUMBNAIL STREAM
               using (var thumbnailFileStream = new FileStream(imagePath, FileMode.Create, FileAccess.Write))
               {

                  // SCALE BITMAP
                  using (var thumbnailBitmap = Android.Graphics.Bitmap.CreateScaledBitmap(originalBitmap, (int)imageWidth, (int)imageHeight, false))
                  {
                     await thumbnailBitmap.CompressAsync(Android.Graphics.Bitmap.CompressFormat.Jpeg, 70, thumbnailFileStream);
                     await thumbnailFileStream.FlushAsync();
                  }
                  thumbnailFileStream.Close();

               }

            }
         }
         catch (Exception) { throw; }
      }

   }
}