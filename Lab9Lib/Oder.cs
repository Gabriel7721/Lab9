using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab9Lib
{
    [Table("tbOder")]
    public class Oder
    {
        public int Quantity { get; set; }
        public DateTime OderDate { get; set; }
        public Product? Product { get; set; }
    }
}
