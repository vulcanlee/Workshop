using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LOBApp.Models
{
    
        public class CommUserGroupModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
