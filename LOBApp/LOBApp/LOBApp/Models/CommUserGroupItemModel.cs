using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LOBApp.Models
{
    public class CommUserGroupItemModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
