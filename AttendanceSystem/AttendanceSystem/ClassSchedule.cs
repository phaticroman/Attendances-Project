using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class ClassSchedule
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }

        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int TotalClasses { get; set; }
    }
}
