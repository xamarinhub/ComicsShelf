﻿namespace ComicsShelf.Notify
{
   public class NotifyVM : Helpers.Observables.ObservableObject
   {
      private const string NotifyKey = "LibraryChangeNotify";

      #region New
      public NotifyVM() { }
      public NotifyVM(string subscribeKey)
      {
         Subscribe($"{NotifyKey}.{subscribeKey}", val =>
         {
            this.Text = val.Text;
            this.Details = val.Details;
            this.Progress = val.Progress;
            this.IsRunning = val.IsRunning;
         });
      }
      #endregion

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

      #region IsRunning
      bool _IsRunning = false;
      public bool IsRunning
      {
         get { return this._IsRunning; }
         set { this.SetProperty(ref this._IsRunning, value); }
      }
      #endregion

      #region Send

      private static string GetNotifyKey()
      { return $"{NotifyKey}.General"; }

      private static string[] GetNotifyKeys(Store.LibraryModel library)
      {
         return new string[] {
            $"{NotifyKey}.General",
            $"{NotifyKey}.{library.ID}"
         };
      }

      public void Send(Store.LibraryModel library, string text)
      {
         this.Text = text;
         this.Details = string.Empty;
         this.Progress = 0;
         this.IsRunning = true;
         foreach (var notifyKey in GetNotifyKeys(library)) { Send(notifyKey, this); }
      }

      public void Send(Store.LibraryModel library, string details, double progress)
      {
         this.Details = details;
         this.Progress = progress;
         this.IsRunning = true;
         foreach (var notifyKey in GetNotifyKeys(library)) { Send(notifyKey, this); }
      }

      public void Send(Store.LibraryModel library, bool isRunning)
      {
         this.IsRunning = isRunning;
         foreach (var notifyKey in GetNotifyKeys(library)) { Send(notifyKey, this); }
      }

      public static void Send(NotifyVM value)
      { Messaging.Send(GetNotifyKey(), value); }

      public static void Send(string key, NotifyVM value)
      { Messaging.Send(key, value); }

      private static void Subscribe(string key, System.Action<NotifyVM> callback)
      { Messaging.Subscribe<NotifyVM>(key, callback); }

      #endregion

   }
}
