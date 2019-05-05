using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataAccesses.Entities
{
    public class InvoiceDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual Invoice Invoice { get; set; }
        public DateTime TDate { get; set; }
        public string Cnt { get; set; }
        public int Qty { get; set; }
        [Column(TypeName = "decimal(8,2)")]
        public Decimal Price { get; set; }
        [Column(TypeName = "decimal(12,2)")]
        public Decimal SubTotal { get; set; }
        public string PictureName { get; set; }
        public bool Flag { get; set; }
        public string Memo { get; set; }
    }
}
