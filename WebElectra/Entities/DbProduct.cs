using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebElectra.Entities
{
    [Table("tblProducts")]
    public class DbProduct
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(maximumLength:500)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime DateCreate { get; set; }
    }
}
