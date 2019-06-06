using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFCoreDemo.Data.Model
{
    public partial class Test1
    {
        public int Id { get; set; }
        public int? Val1 { get; set; }
        public string Name3 { get; set; }
        public DateTime? Name4 { get; set; }
        public decimal? Name5 { get; set; }
        public DateTime? RowVersion { get; set; }
    }
}
