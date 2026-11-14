using G_NET69_EF_Assignment_3.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET69_EF_Assignment_3.Classes
{
    internal class Enrollment
    {
        public DateOnly EnrollmentDate { get; set; }
        public decimal Percentage { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal? FinalGrade { get; set; }
        public int StudentId { get; set; }
        public Students Student { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
