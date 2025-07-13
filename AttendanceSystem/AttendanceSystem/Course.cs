using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public decimal Fees { get; set; }

        public int? AssignedTeacherId { get; set; }
        public CourseUser AssignedTeacher { get; set; }

        public List<CourseStudent> EnrolledStudents { get; set; }
        public List<ClassSchedule> Schedules { get; set; }
    }
}
