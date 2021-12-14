using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocalDbLibrary.Classes
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string SupplierName { get; set; }
        public override string ToString() => ProductName;
    }
}
