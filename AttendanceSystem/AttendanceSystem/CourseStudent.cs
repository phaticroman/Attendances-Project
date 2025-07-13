using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class CourseStudent
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int StudentId { get; set; }
        public CourseUser Student { get; set; }
    }
}

