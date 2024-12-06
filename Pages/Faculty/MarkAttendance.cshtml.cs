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

        [BindProperty]
        public AttendanceModel AttendanceData { get; set; } = new();

        public IActionResult OnGet()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/loginfaculty");
            }

            LoadAssignedSubjects(facultyId.Value);
            AttendanceData.AttendanceDate = DateOnly.FromDateTime(DateTime.Now);
            AttendanceData.AttendanceTime = TimeOnly.FromDateTime(DateTime.Now);
            return Page();
        }

        public IActionResult OnPostLoadStudents()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/loginfaculty");
            }

            LoadAssignedSubjects(facultyId.Value);

            if (AttendanceData.SelectedSubjectId > 0)
            {
                var facultySubject = _context.facultysubjects
                    .FirstOrDefault(fs => fs.sub_id == AttendanceData.SelectedSubjectId && fs.fac_id == facultyId);

                if (facultySubject != null)
                {
                    AttendanceData.ExistingSessionTimes = _context.attences
                        .Where(a => a.sub_id == AttendanceData.SelectedSubjectId
                                    && a.date == AttendanceData.AttendanceDate)
                        .Select(a => a.time)
                        .Distinct()
                        .OrderBy(t => t)
                        .ToList();

                    var students = _context.studentdetails
                        .Where(s => s.stream_id == facultySubject.stream_id
                               && s.sem_id == facultySubject.sem_id
                               && s.is_active)
                        .OrderBy(s => s.student_name)
                        .ToList();

                    AttendanceData.Students = CalculateStudentAttendance(students, AttendanceData.SelectedSubjectId);
                    AttendanceData.ShowNoStudentsMessage = !AttendanceData.Students.Any();
                }
            }

            return Page();
        }

        public IActionResult OnPostMarkAttendance()
        {
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

            var facultySubject = _context.facultysubjects
                .FirstOrDefault(fs => fs.sub_id == AttendanceData.SelectedSubjectId && fs.fac_id == facultyId);

            if (facultySubject == null)
            {
                TempData["ErrorMessage"] = "Selected subject not found.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            // Check if 2 sessions already exist for today
            var existingSessions = _context.attences
                .Where(a => a.sub_id == AttendanceData.SelectedSubjectId && a.date == AttendanceData.AttendanceDate)
                .Select(a => a.time)
                .Distinct()
                .Count();

            if (existingSessions >= 2)
            {
                TempData["ErrorMessage"] = "You can only mark a maximum of 2 attendance sessions per day.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            var eligibleStudents = _context.studentdetails
                .Where(s => s.stream_id == facultySubject.stream_id
                       && s.sem_id == facultySubject.sem_id
                       && s.is_active)
                .Select(s => s.student_id)
                .ToList();

            var attendanceRecords = eligibleStudents.Select(studentId => new Attendence
            {
                student_id = studentId,
                college_id = facultySubject.college_id,
                stream_id = facultySubject.stream_id,
                sub_id = AttendanceData.SelectedSubjectId,
                sem_id = facultySubject.sem_id,
                date = AttendanceData.AttendanceDate,
                time = AttendanceData.AttendanceTime,
                Active = AttendanceData.PresentStudentIds.Contains(studentId)
            }).ToList();

            _context.attences.AddRange(attendanceRecords);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Attendance marked successfully!";
            return RedirectToPage("/Faculty/MarkAttendance");
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
            return students.Select(student =>
            {
                var totalClasses = _context.attences
                    .Count(a => a.sub_id == subjectId && a.student_id == student.student_id);

                var attendedClasses = _context.attences
                    .Count(a => a.sub_id == subjectId && a.student_id == student.student_id && a.Active);

                double attendancePercentage = totalClasses > 0
                    ? (double)attendedClasses / totalClasses * 100
                    : 0;

                return new StudentAttendanceInfo
                {
                    student_id = student.student_id,
                    student_name = student.student_name,
                    reg_num = student.reg_num,
                    sem_id = student.sem_id,
                    TotalClasses = totalClasses,
                    AttendedClasses = attendedClasses,
                    AttendancePercentage = Math.Round(attendancePercentage, 2)
                };
            }).ToList();
        }
    }
}
