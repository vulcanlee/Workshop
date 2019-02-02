using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.Models
{
    public class NotificationToken
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Token { get; set; }
        public virtual LobUser User { get; set; }
        public OSType OSType { get; set; }
        public DateTime RegistrationTime { get; set; }
    }
}
