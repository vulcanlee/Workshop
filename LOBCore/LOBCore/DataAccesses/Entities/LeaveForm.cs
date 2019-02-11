using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataAccesses.Entities
{
    public class LeaveForm
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public virtual LobUser User { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalHours { get; set; }
        public virtual LeaveFormType LeaveFormType { get; set; }
        public string Description { get; set; }
    }
}
