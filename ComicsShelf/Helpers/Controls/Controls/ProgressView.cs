﻿using Xamarin.Forms;

namespace ComicsShelf.Helpers.Controls
{
   public class ProgressView : StackLayout
   {

      public ProgressView()
      {
         this.VerticalOptions = LayoutOptions.FillAndExpand;
         this.Padding = new Thickness(10);

         this.TextLabel = new Label
         {
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center, 
            FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label))
         };
         this.Children.Add(this.TextLabel);

         this.ProgressBar = new ProgressBar
         { HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 5 };
         this.Children.Add(this.ProgressBar);

         this.DetailsLabel = new Label
         {
            HorizontalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label))
         };
         this.Children.Add(this.DetailsLabel);

      }

      #region Text
      Label TextLabel { get; set; }
      public static readonly BindableProperty TextProperty =
         BindableProperty.Create("Text", typeof(string), typeof(CoverFrameView), string.Empty,
         propertyChanged: OnTextChanged, defaultBindingMode: BindingMode.TwoWay);
      public string Text
      {
         get { return (string)GetValue(TextProperty); }
         set { SetValue(TextProperty, value); }
      }
      private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
      { (bindable as ProgressView).TextLabel.Text = (string)newValue; }
      #endregion

      #region Progress
      ProgressBar ProgressBar { get; set; }
      public static readonly BindableProperty ProgressProperty =
         BindableProperty.Create("Progress", typeof(double), typeof(CoverFrameView), (double)0,
         propertyChanged: OnProgressChanged, defaultBindingMode: BindingMode.TwoWay);
      public double Progress
      {
         get { return (double)GetValue(ProgressProperty); }
         set { SetValue(ProgressProperty, value); }
      }
      private static void OnProgressChanged(BindableObject bindable, object oldValue, object newValue)
      { (bindable as ProgressView).ProgressBar.Progress = (double)newValue; }
      #endregion

      #region Details
      Label DetailsLabel { get; set; }
      public static readonly BindableProperty DetailsProperty =
         BindableProperty.Create("Details", typeof(string), typeof(CoverFrameView), string.Empty,
         propertyChanged: OnDetailsChanged, defaultBindingMode: BindingMode.TwoWay);
      public string Details
      {
         get { return (string)GetValue(DetailsProperty); }
         set { SetValue(DetailsProperty, value); }
      }
      private static void OnDetailsChanged(BindableObject bindable, object oldValue, object newValue)
      { (bindable as ProgressView).DetailsLabel.Text = (string)newValue; }
      #endregion

   }
}