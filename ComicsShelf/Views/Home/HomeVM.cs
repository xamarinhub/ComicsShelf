﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComicsShelf.Views.Home
{
   public class HomeVM : Folder.FolderVM<HomeData>
   {

      #region New
      public HomeVM() : this(new HomeData { Text = R.Strings.AppTitle }) { }
      public HomeVM(HomeData args) : base(args)
      {
         this.Title = args.Text;
#if DEBUG
         try
         {
            var appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Title += $" - v{appVersion.Major}.{appVersion.Minor}.{appVersion.Revision}";
         }
         catch { }
#endif
         this.ViewType = typeof(HomeView);

         this.Data = args;
         this.OpenLibraryCommand = new Command(async (item) => await this.OpenLibrary(item));

         this.NotifyData = new Engine.BaseData();
         Helpers.Controls.Messaging.Subscribe<Engine.BaseData>(Helpers.Controls.Messaging.Keys.SearchEngine, (data) => {
            this.NotifyData.Text = data.Text;
            this.NotifyData.Details = data.Details;
            this.NotifyData.Progress = data.Progress;
            this.NotifyData.IsRunning = data.IsRunning;
         });
      }
      #endregion

      #region NotifyData
      Engine.BaseData _NotifyData;
      public Engine.BaseData NotifyData
      {
         get { return this._NotifyData; }
         set { this.SetProperty(ref this._NotifyData, value); }
      }
      #endregion

      #region OpenLibrary
      public Command OpenLibraryCommand { get; set; }
      private async Task OpenLibrary(object item)
      {
         try
         { await Helpers.NavVM.PushAsync<Views.Library.LibraryVM>(false); }
         catch (Exception ex) { await App.ShowMessage(ex); }
      }
      #endregion

   }
}