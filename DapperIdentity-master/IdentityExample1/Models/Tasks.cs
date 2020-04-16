using System;
using System.Collections.Generic;
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

        [Required(ErrorMessage = "Title Error")]
        public string TaskTitle { get => taskTitle; set => taskTitle = value; }

        [Required(ErrorMessage = "Description Error")]
        public string TaskDescription { get => taskDescription; set => taskDescription = value; }

        [Required(ErrorMessage = "Date Error")]
        public DateTime DueDate { get => dueDate; set => dueDate = value; }

        public int Complete { get => complete; set => complete = value; }
        
    }
}
