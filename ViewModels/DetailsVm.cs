using RockyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RockyProject.ViewModels
{
    public class DetailsVm
    {
        public DetailsVm()
        {
            Product = new Product();
        }
        public Product Product { get; set; }
        public bool ExistInCart { get; set; }
    }
}
