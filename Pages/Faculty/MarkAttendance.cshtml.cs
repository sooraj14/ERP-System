
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ERP_System.Data.Entity;
using ERP_System.Data.context;
using ERP_System.ModelClass;
using System.Linq;

namespace ERP_System.Pages.Faculty
{
    public class MarkAttendanceModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MarkAttendanceModel(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public AttendanceModel AttendanceData { get; set; } = new();

        public IActionResult OnGet()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/loginfaculty");
            }
            
            LoadAssignedSubjects(facultyId.Value);
            return Page();
        }

        public IActionResult OnPostLoadStudents(AttendanceModel attendanceData)
        {
            AttendanceData = attendanceData;

            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/loginfaculty");
            }

            LoadAssignedSubjects(facultyId.Value);

            if (attendanceData.SelectedSubjectId > 0)
            {
                var subject = _context.subjects
                    .FirstOrDefault(s => s.sub_id == attendanceData.SelectedSubjectId && s.fac_id == facultyId);

                if (subject != null)
                {
                    var students = _context.studentdetails
                        .Where(s => s.stream_id == subject.stream_id
                               && s.sem_id == subject.sem_id
                               && s.is_active)
                        .OrderBy(s => s.student_name)
                        .ToList();

                    AttendanceData.Students = CalculateStudentAttendance(students, attendanceData.SelectedSubjectId);
                    AttendanceData.ShowNoStudentsMessage = !AttendanceData.Students.Any();
                }
            }

            return Page();
        }

        public IActionResult OnPostMarkAttendance(AttendanceModel attendanceData)
        {
            AttendanceData = attendanceData;

            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/loginfaculty");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            var subject = _context.subjects
                .FirstOrDefault(s => s.sub_id == attendanceData.SelectedSubjectId);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Selected subject not found.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            var existingAttendance = _context.attences
                .Any(a => a.sub_id == attendanceData.SelectedSubjectId
                       && a.date == attendanceData.AttendanceDate);

            if (existingAttendance)
            {
                TempData["ErrorMessage"] = "Attendance for this subject and date already exists.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            var eligibleStudents = _context.studentdetails
                .Where(s => s.stream_id == subject.stream_id
                       && s.sem_id == subject.sem_id
                       && s.is_active)
                .Select(s => s.student_id)
                .ToList();

            var attendanceRecords = eligibleStudents.Select(studentId => new Attendence
            {
                student_id = studentId,
                college_id = subject.college_id,
                stream_id = subject.stream_id,
                sub_id = attendanceData.SelectedSubjectId,
                date = attendanceData.AttendanceDate,
                time = attendanceData.AttendanceTime,
                Active = attendanceData.PresentStudentIds.Contains(studentId)
            }).ToList();

            _context.attences.AddRange(attendanceRecords);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Attendance marked successfully!";
            return RedirectToPage("/Faculty/FacultyDashboard");
        }

        private void LoadAssignedSubjects(int facultyId)
        {
            AttendanceData.AssignedSubjects = _context.facultysubjects
                .Where(fs => fs.fac_id == facultyId)
                .Join(
                    _context.subjects,
                    fs => fs.sub_id,
                    s => s.sub_id,
                    (fs, s) => s
                )
                .Where(s => s.is_active)
                .OrderBy(s => s.sub_name)
                .ToList();
        }

        private List<StudentAttendanceInfo> CalculateStudentAttendance(List<Studentinfo> students, int subjectId)
        {
            var studentAttendanceInfoList = new List<StudentAttendanceInfo>();

            foreach (var student in students)
            {
                var totalClasses = _context.attences
                    .Count(a => a.sub_id == subjectId
                             && a.student_id == student.student_id);

                var attendedClasses = _context.attences
                    .Count(a => a.sub_id == subjectId
                             && a.student_id == student.student_id
                             && a.Active);

                double attendancePercentage = totalClasses > 0
                    ? (double)attendedClasses / totalClasses * 100
                    : 0;

                studentAttendanceInfoList.Add(new StudentAttendanceInfo
                {
                    student_id = student.student_id,
                    student_name = student.student_name,
                    reg_num = student.reg_num,
                    sem_id = student.sem_id,
                    TotalClasses = totalClasses,
                    AttendedClasses = attendedClasses,
                    AttendancePercentage = Math.Round(attendancePercentage, 2)
                });
            }

            return studentAttendanceInfoList;
        }
    }
}
