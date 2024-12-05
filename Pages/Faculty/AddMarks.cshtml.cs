using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.Models;
using System.Linq;

namespace ERP_System.Pages.Faculty
{
    public class AddMarksModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddMarksModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public FacultyMarksModel FacultyMarks { get; set; } = new();
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
        public IActionResult OnPostLoadStudents()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/loginfaculty");
            }
            LoadAssignedSubjects(facultyId.Value);

            if (FacultyMarks.SelectedSubjectId > 0 && !string.IsNullOrEmpty(FacultyMarks.ExamType))
            {
                var subject = _context.subjects.FirstOrDefault(s => s.sub_id == FacultyMarks.SelectedSubjectId && s.is_active);
                if (subject != null)
                {
                    FacultyMarks.LoadedStudents = _context.studentdetails.Where(s => s.stream_id == subject.stream_id && s.sem_id == subject.sem_id && s.is_active)
                        .OrderBy(s => s.student_name).ToList();

                    FacultyMarks.ShowNoStudentsMessage = !FacultyMarks.LoadedStudents.Any();
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please select a subject and exam type.";
            }

            return Page();
        }
        public IActionResult OnPostAddMarks()
        {
            var facultyId = HttpContext.Session.GetInt32("FacultyID");
            if (facultyId == null)
            {
                return RedirectToPage("/Faculty/loginfaculty");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            var subject = _context.subjects.FirstOrDefault(s => s.sub_id == FacultyMarks.SelectedSubjectId);
            if (subject == null)
            {
                TempData["ErrorMessage"] = "Selected subject not found.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }
            if (FacultyMarks.ExamType == "Internal" && FacultyMarks.InternalNumber.HasValue)
            {
                var existingMarks = _context.internals.Any(ie => ie.sub_id == FacultyMarks.SelectedSubjectId &&
                    ie.exm_type == FacultyMarks.ExamType + " " + FacultyMarks.InternalNumber);

                if (existingMarks)
                {
                    TempData["ErrorMessage"] = "Marks for this subject and exam type already exist.";
                    LoadAssignedSubjects(facultyId.Value);
                    return Page();
                }

                var internalExamRecords = FacultyMarks.StudentMarks.Select(mark => new InternalExam
                {
                    student_id = mark.StudentId,
                    sub_id = FacultyMarks.SelectedSubjectId,
                    exm_type = FacultyMarks.ExamType + " " + FacultyMarks.InternalNumber,
                    scored_marks = mark.ScoredMarks,
                    max_mark = FacultyMarks.MaxMarks,
                    sem_id = subject.sem_id,
                    stream_id = subject.stream_id,
                    college_id = subject.college_id
                }).ToList();

                _context.internals.AddRange(internalExamRecords);
            }
            else if (FacultyMarks.ExamType == "Final")
            {
                var finalExamRecords = FacultyMarks.StudentMarks.Select(mark => new Marks
                {
                    student_id = mark.StudentId,
                    sub_id = FacultyMarks.SelectedSubjectId,
                    sem_marks = mark.ScoredMarks,
                    max_marks = FacultyMarks.MaxMarks,
                    sem_id = subject.sem_id,
                    stream_id = subject.stream_id,
                    college_id = subject.college_id
                }).ToList();

                _context.marks.AddRange(finalExamRecords);
            }
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Marks successfully submitted.";
            return Page();
        }
        private void LoadAssignedSubjects(int facultyId)
        {
            FacultyMarks.AssignedSubjects = _context.facultysubjects
                .Where(fs => fs.fac_id == facultyId)
                .Join(
                    _context.subjects,
                    fs => fs.sub_id,
                    s => s.sub_id,
                    (fs, s) => s
                )
                .Where(s => s.is_active)
                .ToList();
        }
    }
}
