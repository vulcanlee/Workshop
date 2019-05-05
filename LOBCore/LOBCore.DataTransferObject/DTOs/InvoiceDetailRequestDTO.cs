using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataTransferObject.DTOs
{
    public class InvoiceDetailRequestDTO
    {
        public int Id { get; set; }
        public InvoiceDTO Invoice { get; set; }
        public DateTime TDate { get; set; }
        public string Cnt { get; set; }
        public int Qty { get; set; }
        public Decimal Price { get; set; }
        public Decimal SubTotal { get; set; }
        public string PictureName { get; set; }
        public bool Flag { get; set; }
        public string Memo { get; set; }
    }
}
