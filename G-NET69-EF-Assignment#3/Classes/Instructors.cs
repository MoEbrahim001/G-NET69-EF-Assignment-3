using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET69_EF_Assignment_3.Classes
{
    internal class Instructors
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string? ShortBio { get; set; }
        public string EmailAddress { get; set; }
        public DateOnly JoinedDate { get; set; }
        public ICollection<Course> Courses { get; set; } 


    }
}
