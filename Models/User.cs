using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace todo_app.Models
{
    public class User : IdentityUser
    {
        public HashSet<ToDo> ToDos { get; set; }
    }
}