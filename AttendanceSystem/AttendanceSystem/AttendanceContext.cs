using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceSystem
{
    public class AttendanceContext : DbContext
    {
        public DbSet<CourseUser> CourseUsers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseStudent> CourseStudents { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public string ConnectionString { get; }
        public AttendanceContext()
        {
            ConnectionString  = "Server=ROMAN_PC\\SQLEXPRESS;Database=Attendance;User Id=imroman;Password=123456;Trusted_Connection=True;TrustServerCertificate=True";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
