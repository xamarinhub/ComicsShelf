﻿using SQLite;

namespace ComicsShelf.Helpers.Database
{

   [Table("Libraries")]
   public class Library
   {

      [PrimaryKey, AutoIncrement]
      public int LibraryID { get; set; }

      public string LibraryText { get; set; }

      [Indexed]
      public string LibraryPath { get; set; }

      [Ignore]
      public ComicsShelf.Library.LibraryTypeEnum Type
      {
         get { return (ComicsShelf.Library.LibraryTypeEnum)this.TypeInner; }
         set { this.TypeInner = (short)value; }
      }
      public short TypeInner { get; set; }

      public bool Available { get; set; }
      public int FileCount { get; set; }

   }
}