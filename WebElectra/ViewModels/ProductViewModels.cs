using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebElectra.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
    public class ProductAddVM
    {
        [Required(ErrorMessage = "Поле не може бути пустим")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не може бути пустим")]
        public decimal Price { get; set; }
    }
    public class ProductDeleteVM
    {
        public int Id { get; set; }
    }
}
