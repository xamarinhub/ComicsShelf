﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ComicsShelf.Helpers.Observables
{
   public class ObservableList<T> : ObservableCollection<T>
   {

      #region New
      public ObservableList() : base() { }
      public ObservableList(IEnumerable<T> collection) : base(collection) { }
      #endregion

      #region AddRange
      public void AddRange(IEnumerable<T> collection, NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
      {

         if (collection == null) throw new ArgumentNullException("collection");
         this.CheckReentrancy();

         if (notificationMode == NotifyCollectionChangedAction.Reset)
         {
            foreach (var i in collection) { Items.Add(i); }
            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            return;
         }

         int startIndex = Count;
         var changedItems = collection is List<T> ? (List<T>)collection : new List<T>(collection);
         foreach (var i in changedItems)
         {
            Items.Add(i);
         }

         this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
         this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
         this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
      }
      #endregion

      #region RemoveRange
      public void RemoveRange(IEnumerable<T> collection)
      {
         if (collection == null) throw new ArgumentNullException("collection");

         foreach (var i in collection)
            Items.Remove(i);
         this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
      }
      #endregion

      #region ReplaceRange

      public void Replace(T item)
      {
         this.ReplaceRange(new T[] { item });
      }

      public void ReplaceRange(IEnumerable<T> collection)
      {
         if (collection == null) throw new ArgumentNullException("collection");

         Items.Clear();
         this.AddRange(collection, NotifyCollectionChangedAction.Reset);
      }

      #endregion

   }
}