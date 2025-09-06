using OnlineCourse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourse.Application.Models.LogModels
{
    public class LogRequestModel
    {
        [Required]
        public Guid UserId { get; set; }
 

        [Required]
        public LogActionType Action { get; set; }

        [Required]
        public string IpAddress { get; set; } = string.Empty;
    }
}
