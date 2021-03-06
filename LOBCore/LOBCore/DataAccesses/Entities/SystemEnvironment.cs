﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LOBCore.DataAccesses.Entities
{
    public class SystemEnvironment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string AppName { get; set; }
        public string AndroidVersion { get; set; }
        public string AndroidUrl { get; set; }
        public string iOSVersion { get; set; }
        public string iOSUrl { get; set; }

    }
}
