﻿namespace ComicsShelf.Startup
{
   public class StartupData : Helpers.Observables.ObservableObject
   {

      #region Text
      string _Text;
      public string Text
      {
         get { return this._Text; }
         set { this.SetProperty(ref this._Text, value); }
      }
      #endregion

      #region Details
      string _Details;
      public string Details
      {
         get { return this._Details; }
         set { this.SetProperty(ref this._Details, value); }
      }
      #endregion

      #region Progress
      double _Progress = 0;
      public double Progress
      {
         get { return this._Progress; }
         set { this.SetProperty(ref this._Progress, value); }
      }
      #endregion


      #region RootFolder
      Folder.FolderData _RootFolder;
      public Folder.FolderData RootFolder
      {
         get { return this._RootFolder; }
         set { this.SetProperty(ref this._RootFolder, value); }
      }
      #endregion

   }
}