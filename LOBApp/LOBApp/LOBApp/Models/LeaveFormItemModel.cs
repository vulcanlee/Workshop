using LOBApp.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LOBApp.Models
{

    public class LeaveFormItemModel : INotifyPropertyChanged
    {
        public DateTime BeginDate { get; set; }
        public TimeSpan BeginTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalHours { get; set; }
        public string Description { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
