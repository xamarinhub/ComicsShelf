﻿using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComicsShelf.Helpers.FolderDialog
{
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class FolderDialogPage : ContentPage
   {

      public FolderDialogPage()
      {
         InitializeComponent();
      }

      protected override bool OnBackButtonPressed()
      {
         return true; // PREVENT THE BACK BUTON
      }

   }
}