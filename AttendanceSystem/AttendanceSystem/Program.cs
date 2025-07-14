
using AttendanceSystem;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;

using (var context = new AttendanceContext())
{
    while (true)
    {

        Console.Write("Username: ");
        string username = Console.ReadLine();
        
        
        Console.Write("Password: ");
        string password = Console.ReadLine();
        var user = context.CourseUsers.FirstOrDefault(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            Console.WriteLine("User Not Found");
            continue;

        }
        else
        {
            switch (user.Role.ToLower())
            {
                case "admin":
                    AdminMenu(context);
                    break;
                case "student":
                    StudentMenu(context, user);
                    break;
                case "teacher":
                    TeacherMenu(context, user);
                    break;
            }

        }
    }
}

void AdminMenu (AttendanceContext context)
{
    while (true)
    {
        Console.WriteLine("// Welcome To Admin Menu //");
        Console.WriteLine("1. Create Teacher");
        Console.WriteLine("2. Create Student");
        Console.WriteLine("3. Create Course");
        Console.WriteLine("4. Assign Teacher to Course");
        Console.WriteLine("5. Assign Student to Course");
        Console.WriteLine("6. Set Class Schedule");
        Console.WriteLine("0. Logout");
        int input = int.Parse(Console.ReadLine());
        switch (input)
        {
            case 1:
                CreateUser(context, "Teacher");
                break;
            case 2:
                CreateUser(context, "Student");
                break;
            case 3:
                CreateCourse(context);
                break;
            case 4:
                AssignTeacher(context);
                break;
            case 5:
                AssignStudent(context);
                break;
            case 6:
                SetClassSchedule(context);
                break;
            case 0:
                return;
            default:
                Console.WriteLine("invalid input");
                break;
        }
    }
}
void CreateUser(AttendanceContext context,string Role)
{
    Console.Write("Enter Name:");
    string name = Console.ReadLine();
    Console.Write("Enter UserName:");
    string username = Console.ReadLine();
    bool exists = context.CourseUsers.Any(p => p.Username == username);
    if(exists)
    {
        Console.WriteLine("Username Already Taken! Try Another Name!");
        return;
    }
    else
    {
        Console.Write("Enter Password:");
        string password = Console.ReadLine();
        var user = new CourseUser
        {
            Name = name,
            Username = username,
            Password = password,
            Role = Role,
        };
        context.CourseUsers.Add(user);
        context.SaveChanges();
        Console.WriteLine("Account Created SuccessFully");
    }
    

}
void CreateCourse (AttendanceContext context)
{
    Console.Write("Enter Course Name:");
    string name = Console.ReadLine();
    Console.Write("Enter Course Fees: ");
    decimal fees = decimal.Parse(Console.ReadLine());
    var course = new Course
    {
        CourseName = name,
        Fees = fees
    };
    context.Courses.Add(course);
    context.SaveChanges();
    Console.WriteLine("Course Created SuccessFully");
}
void AssignTeacher (AttendanceContext context)
{
    Console.Write("Enter Course ID: ");
    int courseId = int.Parse(Console.ReadLine());

    var teachers = context.CourseUsers.Where(t => t.Role == "Teacher").ToList();
    foreach(var t in teachers)
    {
        Console.WriteLine($"{t.Id}: {t.Name}");
    }
    Console.Write("Enter Teacher ID to assign: ");
    int teacherId = int.Parse(Console.ReadLine());
   
    var course = context.Courses.Find(courseId);
    course.AssignedTeacherId = teacherId;
    context.SaveChanges();
    Console.WriteLine("Teacher Assign Successfull");
}

void AssignStudent (AttendanceContext context)
{
    Console.Write("Enter Course Id:");
    int courseId = int.Parse(Console.ReadLine());
    var students = context.CourseUsers.Where(s => s.Role == "Student").ToList();
    foreach(var s in students)
    {
        Console.WriteLine($"{s.Id}: {s.Name}");
    }
    Console.Write("Student Id To Assign:");
    int studentId = int.Parse(Console.ReadLine());
    var enroll = new CourseStudent
    {
        StudentId = studentId,
        CourseId = courseId,
    };
    context.CourseStudents.Add(enroll);
    context.SaveChanges();
    Console.WriteLine("Student Assign SuccessFull");
    }

void SetClassSchedule(AttendanceContext context)
{
    Console.Write("Enter Course Id");
    int couseId = int.Parse(Console.ReadLine());
    Console.Write("Enter Day (Start from Monday");
    var day = Enum.Parse<DayOfWeek>(Console.ReadLine(), true);
    Console.Write("Start Time( 19:00): ");
    var start = TimeSpan.Parse(Console.ReadLine());
    Console.Write("End Time (21:00): ");
    var end = TimeSpan.Parse(Console.ReadLine());
    Console.Write("Total Classes: ");
    int total = int.Parse(Console.ReadLine());
    var schedule = new ClassSchedule
    {
        CourseId = couseId,
        Day = day,
        StartTime = start,
        EndTime = end,
        TotalClasses = total,
    };
    context.ClassSchedules.Add(schedule);
    context.SaveChanges();
    Console.WriteLine("Schedule Set SuccessFully");
}

void TeacherMenu (AttendanceContext context, CourseUser teacher)
{
    while (true)
    {
        Console.WriteLine("// Welcome To Teacher Menu //");
        Console.WriteLine("1.View Attendance Report ");
        Console.WriteLine("2. Logout");
        int intput = int.Parse(Console.ReadLine());
        if (intput == 1)
        {
            var course = context.Courses.Where(c => c.AssignedTeacherId == teacher.Id).ToList();
            if (course.Count == 0)
            {
                Console.WriteLine("There are no course for you ");
                continue;

            }
            else
            {
                foreach (var c in course)
                {
                    Console.WriteLine($"{c.Id}: {c.CourseName}");
                }
                Console.Write("Enter Course Id: ");
                int courseid = int.Parse(Console.ReadLine());
                var today = DateTime.Today;
                var attendance = context.Attendances.Include(a => a.Student).Where(a => a.CourseId == courseid).ToList();
                var attendances = attendance.Where(a => a.ClassDate.DayOfWeek == DateTime.Now.DayOfWeek).ToList();
                var attend = attendances.Where(a => a.ClassDate.Date == today).ToList();
                int total = attend.Count();
                Console.WriteLine($"Total attendances: {total}");
                foreach (var a in attend)
                {
                    Console.WriteLine($"{a.Student.Name}");
                }

            }
        }
        else
        {
            break;
        }
    }
}

void StudentMenu(AttendanceContext context, CourseUser student)
{
    while (true) {
        Console.WriteLine("// Student Menu //");
        Console.WriteLine("1.Give Attendance");
        Console.WriteLine("2.Logout");
        int input = int.Parse(Console.ReadLine());
        if (input == 1)
        {
            var courses = context.CourseStudents.Where(cs => cs.StudentId == student.Id).Select(cs => cs.Course).ToList();
            if (courses.Count == 0)
            {
                Console.WriteLine(" You have No course ");
            }
            else
            {
                foreach (var c in courses)
                {
                    Console.WriteLine($"{c.Id} {c.CourseName}");
                }
                Console.Write("Enter Course Id: ");
                int courseid = int.Parse(Console.ReadLine());
                var Today = DateTime.Now;
                var now = Today.TimeOfDay;
                var schedule = context.ClassSchedules.FirstOrDefault(s => s.CourseId == courseid && s.Day == Today.DayOfWeek);
            
                if (schedule == null)
                {
                    Console.WriteLine("There Are No Class Today");
                }
                else if (now < schedule.EndTime || now < schedule.StartTime)
                {
                    Console.WriteLine("You can only give attendance during class time.");
                    return;
                }
                else
                {
                    var alreadyMarked = context.Attendances.Any(a => a.StudentId == student.Id && a.CourseId == courseid && a.ClassDate.Date == Today.Date);

                    if (alreadyMarked)
                    {
                        Console.WriteLine("You have already given attendance today.");
                    }
                    else
                    {
                        var attendance = new Attendance
                        {
                            StudentId = student.Id,
                            CourseId = courseid,
                            ClassDate = Today,
                            IsPresent = true,
                        };
                        context.Attendances.Add(attendance);
                        context.SaveChanges();
                        Console.WriteLine("Attendance Save SuccessFully ");
                    }

                }
                
            }
        }
        else
        {
            return;
        }
    }
}