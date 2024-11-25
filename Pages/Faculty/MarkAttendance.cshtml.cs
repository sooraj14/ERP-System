using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ERP_System.Data.Entity;
using ERP_System.Data.context;
using System.ComponentModel.DataAnnotations;

namespace ERP_System.Pages.Faculty
{
    public class MarkAttendanceModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MarkAttendanceModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Subject> AssignedSubjects { get; set; } = new();
        public List<Studentinfo> Students { get; set; } = new();
        public bool ShowNoStudentsMessage { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please select a subject")]
        public int SelectedSubjectId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Date is required")]
        public DateOnly AttendanceDate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Time is required")]
        public TimeOnly AttendanceTime { get; set; }

        [BindProperty]
        public List<int> PresentStudentIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/FacultyLogin");
            }

            await LoadAssignedSubjects(facultyId.Value);
            return Page();
        }

        public async Task<IActionResult> OnPostLoadStudentsAsync()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/FacultyLogin");
            }

            await LoadAssignedSubjects(facultyId.Value);

            if (SelectedSubjectId > 0)
            {
                var subject = await _context.subjects
                    .FirstOrDefaultAsync(s => s.sub_id == SelectedSubjectId && s.fac_id == facultyId);

                if (subject != null)
                {
                    Students = await _context.studentdetails
                        .Where(s => s.stream_id == subject.stream_id
                               && s.sem_id == subject.sem_id
                               && s.is_active)
                        .OrderBy(s => s.student_name)
                        .ToListAsync();

                    ShowNoStudentsMessage = !Students.Any();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostMarkAttendanceAsync()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/FacultyLogin");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please fill in all required fields.";
                await LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            try
            {
                var subject = await _context.subjects
                    .FirstOrDefaultAsync(s => s.sub_id == SelectedSubjectId);

                if (subject == null)
                {
                    TempData["ErrorMessage"] = "Selected subject not found.";
                    await LoadAssignedSubjects(facultyId.Value);
                    return Page();
                }

                var existingAttendance = await _context.attences
                    .AnyAsync(a => a.sub_id == SelectedSubjectId
                             && a.date == AttendanceDate);

                if (existingAttendance)
                {
                    TempData["ErrorMessage"] = "Attendance for this subject and date already exists.";
                    await LoadAssignedSubjects(facultyId.Value);
                    return Page();
                }

                var eligibleStudents = await _context.studentdetails
                    .Where(s => s.stream_id == subject.stream_id
                           && s.sem_id == subject.sem_id
                           && s.is_active)
                    .Select(s => s.student_id)
                    .ToListAsync();

                var attendanceRecords = eligibleStudents.Select(studentId => new Attendence
                {
                    student_id = studentId,
                    college_id = subject.college_id,
                    stream_id = subject.stream_id,
                    sub_id = SelectedSubjectId,
                    date = AttendanceDate,
                    time = AttendanceTime,
/*                    is_present = PresentStudentIds.Contains(studentId)
*/                }).ToList();

                await _context.attences.AddRangeAsync(attendanceRecords);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Attendance marked successfully!";
                return RedirectToPage("/Faculty/FacultyDashboard");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while marking attendance. Please try again.";
                await LoadAssignedSubjects(facultyId.Value);
                return Page();
            }
        }

        private async Task LoadAssignedSubjects(int facultyId)
        {
            AssignedSubjects = await _context.facultysubjects
                .Where(fs => fs.fac_id == facultyId)
                .Join(
                    _context.subjects,
                    fs => fs.sub_id,
                    s => s.sub_id,
                    (fs, s) => s
                )
                .Where(s => s.is_active)
                .OrderBy(s => s.sub_name)
                .ToListAsync();
        }
    }
}