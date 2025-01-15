using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.DtoClasses
{
    public class ProductDto : BaseDto
    {
        public string ProductName { get; set; }
        public int UnitsInStock { get; set; }
        public string ImagePath { get; set; } //Ürünlerim resim yolunu tutacak olan property'dir
        public int? CategoryId { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
