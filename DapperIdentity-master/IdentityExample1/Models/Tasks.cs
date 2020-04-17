using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample1.Models
{
    public class Tasks
    {
        private int id;
        private int userId;
        private string taskTitle;
        private string taskDescription;
        private DateTime dueDate;
        private int complete;

        public int Id { get => id; set => id = value; }
        public int UserId { get => userId; set => userId = value; }
        
        [DisplayName("Task Title: ")]
        [Required(ErrorMessage = "Title Error")]
        public string TaskTitle { get => taskTitle; set => taskTitle = value; }

        [DisplayName("Task Description: ")]
        [Required(ErrorMessage = "Description Error")]
        public string TaskDescription { get => taskDescription; set => taskDescription = value; }

        [DisplayName("Due Date: ")]
        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "1/1/1966", "12/31/9999")]
        [Required(ErrorMessage = "Please select a valid due date")]
        public DateTime DueDate { get => dueDate; set => dueDate = value; }

        [DisplayName("Have you completed this task?")]
        public int Complete { get => complete; set => complete = value; }
        
    }
}
