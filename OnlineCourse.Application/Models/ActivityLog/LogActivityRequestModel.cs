using OnlineCourse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineCourse.Application.Models.Log
{
    public class LogActivityRequestModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public ActivityTargetType Target { get; set; }

        [Required]
        public string TargetId   { get; set; } = string.Empty;
    }
}
