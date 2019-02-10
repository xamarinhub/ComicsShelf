﻿using System;
using System.Threading.Tasks;
using Xamarin.OneDrive.Profile;

namespace ComicsShelf.Libraries.Implementation
{
   partial class OneDrive
   {

      public async Task<bool> Validate(Library library)
      {
         try
         {
            library.Available = await this.Connector.ConnectAsync();
         }
         catch (Exception ex) { Engine.AppCenter.TrackEvent("OneDrive.Validate", ex); library.Available = false; }
         return library.Available;
      }

      public async Task<bool> AddLibrary(Library library)
      {
         if (!await this.Connector.ConnectAsync()) { return false; }
         var profile = await this.Connector.GetProfileAsync();
         if (profile == null) { return false; }
         library.LibraryID = profile.id;
         library.Description = profile.Name;
         library.Available = true;
         return true;
      }

      public async Task<bool> RemoveLibrary(Library library)
      {
         return await this.Connector.DisconnectAsync();
      }

   }
}