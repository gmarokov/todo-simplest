using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace todo_app.Models
{
    public class ToDo
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsDone { get; set; }

        public string UserId { get; set; }
        
        public User User { get;set; }
    }
}