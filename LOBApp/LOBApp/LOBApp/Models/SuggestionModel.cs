using LOBApp.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LOBApp.Models
{
    public class SuggestionModel : INotifyPropertyChanged
    {
        public string Subject { get; set; }
        public string Message { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
