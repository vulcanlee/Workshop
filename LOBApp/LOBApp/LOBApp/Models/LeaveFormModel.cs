using LOBApp.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LOBApp.Models
{

    public class LeaveFormModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public UserDTO user { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TotalHours { get; set; }
        public LeaveFormTypeModel leaveFormType { get; set; }
        public string Description { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public LeaveFormModel Clone()
        {
            LeaveFormModel leaveFormModel = (LeaveFormModel)this.MemberwiseClone();
            leaveFormModel.user = new UserDTO() { Id = this.user.Id };
            leaveFormModel.leaveFormType = new LeaveFormTypeModel() { Id = this.leaveFormType.Id, Name = this.leaveFormType.Name };
            return leaveFormModel;
        }
    }
}
