using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LOBApp.Models
{
    
        public class LeaveFormTypeModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
