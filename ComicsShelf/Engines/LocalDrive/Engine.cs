﻿using ComicsShelf.Libraries;
using System;
using System.Threading.Tasks;

namespace ComicsShelf.Engines.LocalDrive
{
   internal partial class LocalDriveEngine : IEngine
   {

      public Task<bool> AddLibrary(LibraryModel library)
      {
         throw new NotImplementedException();
      }

      public Task<bool> RemoveLibrary(LibraryModel library)
      {
         throw new NotImplementedException();
      }

      public Task<bool> Validate(LibraryModel library)
      {
         throw new NotImplementedException();
      }

   }
}
