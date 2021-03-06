﻿using ComicsShelf.ComicFiles;
using ComicsShelf.Helpers.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComicsShelf.Splash
{
   public class SplashVM : BaseVM
   {

      readonly Store.ILibraryStore Store;
      readonly Store.LibraryModel Library;
      public List<ComicFileVM> FolderFiles { get; private set; }
      public SplashVM(ComicFileVM file)
      {
         this.CurrentFile = file;
         this.Store = DependencyService.Get<ComicsShelf.Store.ILibraryStore>();
         this.Library = this.Store.GetLibrary(file.ComicFile.LibraryKey);

         this.FolderFiles = this.Store.LibraryFiles[ComicsShelf.Store.enLibraryFilesGroup.Libraries]
            .Where(x =>
               x.ComicFile.LibraryKey == file.ComicFile.LibraryKey &&
               x.ComicFile.FolderPath == file.ComicFile.FolderPath &&
               x.ComicFile.Available == true)
            .OrderByDescending(x => x.ComicFile.FilePath)
            .ToList();

         this.ItemSelectedCommand = new Command(async (item) => await this.ItemSelected(item));
         this.ItemOpenCommand = new Command(async () => await this.ItemOpen());
         this.ClearCacheCommand = new Command(async () => await this.ClearCache());
      }

      public void OnAppearing()
      {
         try
         {
            this.OnPropertyChanged("CurrentFileRating");
            this.OnPropertyChanged("CurrentFileReaded");
            this.OnPropertyChanged("IsAllReaded");
         }
         catch { }
      }

      ComicFileVM _CurrentFile;
      public ComicFileVM CurrentFile
      {
         get { return this._CurrentFile; }
         set
         {
            this.SetProperty(ref this._CurrentFile, value);
            this.OnPropertyChanged("CurrentFileRating");
            this.OnPropertyChanged("CurrentFileReaded");
         }
      }

      public short CurrentFileRating
      {
         get { return this.CurrentFile.Rating; }
         set
         {
            Helpers.AppCenter.TrackEvent("SplashPage.OnPropertyChanged", $"Rating:{value}");
            this.CurrentFile.Rating = value;
            this.OnPropertyChanged("CurrentFileRating");
            this.UpdateLibrary();
         }
      }

      public bool CurrentFileReaded
      {
         get { return this.CurrentFile.Readed; }
         set
         {
            Helpers.AppCenter.TrackEvent("SplashPage.OnPropertyChanged", $"Readed:{value}");
            this.CurrentFile.Readed = value;
            this.OnPropertyChanged("CurrentFileReaded");
            this.UpdateLibrary();
         }
      }

      public bool IsAllReaded
      {
         get { return (this.FolderFiles.Count(x => !x.Readed) == 0); }
         set
         {
            Helpers.AppCenter.TrackEvent("SplashPage.OnPropertyChanged", $"IsAllReaded:{value}");
            this.FolderFiles.ForEach(x => x.Readed = value);
            this.OnPropertyChanged("IsAllReaded");
            this.UpdateLibrary();
         }
      }

      private async void UpdateLibrary()
      { await Services.LibraryService.UpdateLibrary(this.CurrentFile.ComicFile.LibraryKey); }

      ComicPageSize _PageSize;
      public ComicPageSize PageSize
      {
         get { return this._PageSize; }
         set { this.SetProperty(ref this._PageSize, value); }
      }

      public Command ItemSelectedCommand { get; set; }
      private async Task ItemSelected(object item)
      {
         try
         {
            this.CurrentFile = item as ComicFileVM;
         }
         catch (Exception ex) { await App.ShowMessage(ex); }
      }

      public Command ClearCacheCommand { get; set; }
      private async Task ClearCache()
      {
         try
         {
            if (!await App.ConfirmMessage(R.Strings.SPLASH_FILE_CLEAR_COMIC_CACHE_MESSAGE)) { return; }

            if (System.IO.Directory.Exists(this.CurrentFile.CachePath))
            { System.IO.Directory.Delete(this.CurrentFile.CachePath, true); }
            if (System.IO.File.Exists($"{this.CurrentFile.CachePath}.zip"))
            { System.IO.File.Delete($"{this.CurrentFile.CachePath}.zip"); }

            this.CurrentFile.CachePath = string.Empty;
         }
         catch (Exception ex) { await App.ShowMessage(ex); }
      }

      public Command ItemOpenCommand { get; set; }
      private async Task ItemOpen()
      {
         try
         {
            this.IsBusy = true;
            if (this.CurrentFile == null || this.CurrentFile.ComicFile == null) { throw new NullReferenceException("The CurrentFile variable is not defined"); }
            Messaging.Send("OnComicFileOpening", this.CurrentFile);
            var startTime = DateTime.Now;

            var wasCached = System.IO.Directory.Exists(this.CurrentFile.ComicFile.CachePath);
            var engine = Engines.Engine.Get(this.Library.Type);
            var pages = await engine.ExtractPages(this.Library, this.CurrentFile.ComicFile);

            if (System.IO.Directory.Exists(this.CurrentFile.ComicFile.CachePath))
            {
               if (pages != null && pages.Count != 0)
               {
                  this.CurrentFile.CachePath = this.CurrentFile.ComicFile.CachePath;
                  this.CurrentFile.Pages = new ObservableList<ComicPageVM>(pages.ToList());
                  this.CurrentFile.Pages.Add(new ComicPageVM { Text = "END" });
                  this.CurrentFile.ComicFile.TotalPages = (short)(this.CurrentFile.Pages.Count - 2); /* cover and the end one */

                  var vm = new Reading.ReadingVM(this.CurrentFile);
                  var page = new Reading.ReadingPage { BindingContext = vm };
                  await App.Navigation().PushAsync(page);
               }
            }

            var endTime = DateTime.Now;
            var trackProps = new Dictionary<string, string> {
               { "LibraryType", this.Library.Type.ToString() },
               { "ElapsedSeconds", ((int)(endTime-startTime).TotalSeconds).ToString() },
               { "PagesCount", pages.Count.ToString() },
               { "WasCached", wasCached.ToString() }
            };
            Helpers.AppCenter.TrackEvent($"SplashPage.OnItemOpen", trackProps);

         }
         catch (Exception ex) { await App.ShowMessage(ex); }
         finally
         {
            Messaging.Send("OnComicFileOpened", this.CurrentFile);
            this.IsBusy = false;
         }
      }

   }
}
