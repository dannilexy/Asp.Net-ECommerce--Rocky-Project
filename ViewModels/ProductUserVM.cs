using RockyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockyProject.ViewModels
{
    public class ProductUserVM
    {
        public ProductUserVM()
        {
            ProductList = new List<Product>();
        }

        public ApplicationUser ApplicationUser { get; set; }
        public IList<Product> ProductList { get; set; }
    }
}
