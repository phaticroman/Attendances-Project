using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class Attendance
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public CourseUser Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DateTime ClassDate { get; set; }
        public bool IsPresent { get; set; }
    }
}
