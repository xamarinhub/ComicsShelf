﻿using Xamarin.Forms;

namespace ComicsShelf
{
   public partial class App : Application
	{

      #region New
      public App()
      {
         InitializeComponent();
         var initialPage = new ContentPage();
         initialPage.Appearing +=
            async (object sender, System.EventArgs e) =>
            {
               await Helpers.ViewModels.NavVM.PushAsync<Startup.StartupVM>(true);
            };
         MainPage = new NavigationPage(initialPage);
      }
      #endregion

      #region Settings
      private static Helpers.Settings.Settings _Settings = null;
      internal static Helpers.Settings.Settings Settings
      {
         get
         {
            if (_Settings == null) { _Settings = new Helpers.Settings.Settings(); }
            return _Settings;
         }
      }
      #endregion

      #region Message
      private static Helpers.Controls.Messages _Message = null;
      public static Helpers.Controls.Messages Message
      {
         get
         {
            if (_Message == null) { _Message = new Helpers.Controls.Messages(); }
            return _Message;
         }
      }
      #endregion

      protected override void OnStart ()
		{ /* Handle when your app starts */ }

		protected override void OnSleep ()
		{ /* Handle when your app sleeps */ }

		protected override void OnResume ()
		{ /* Handle when your app resumes */ }

	}
}
