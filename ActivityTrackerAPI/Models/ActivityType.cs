﻿
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrackerAPI.Models
{
    public class ActivityType
    {
        [Key]
        public int ActivityTypeId { get; set; }

        [Required]
        public string Description { get; set; }

        ICollection<Activity> Activities { get; set; }
    }
}