﻿using ComicsShelf.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsShelf.Droid
{
   partial class FileSystem
   {

      public async Task<Folder[]> GetFolderChilds(Folder folder)
      {
         var folderChilds = await Task.FromResult(System.IO.Directory.GetDirectories(folder.Path));
         var result = folderChilds
            .Select(path => new Folder
            {
               Name = System.IO.Path.GetFileNameWithoutExtension(path),
               Path = path
            })
            .ToArray();
         return result;
      }

   }
}