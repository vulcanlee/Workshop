using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataAccesses.Entities
{
    public class Invoice
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public virtual LobUser User { get; set; }
        public DateTime Date { get; set; }
        public string Memo { get; set; }
        public virtual ICollection<InvoiceDetail> Details { get; set; }
    }
}
