using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ERP_System.Data.context;
using ERP_System.Models;
using ERP_System.Data.Entity;
using System.Linq;

namespace ERP_System.Pages.Faculty
{
    public class ViewOrEditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ViewOrEditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ViewMarksModel ViewMarks { get; set; } = new();

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

            public IActionResult OnPostLoadMarks()
            {
                var facultyId = HttpContext.Session.GetInt32("FacultyID");
                if (facultyId == null)
                {
                    return RedirectToPage("/Faculty/loginfaculty");
                }

                LoadAssignedSubjects(facultyId.Value);

                if (ViewMarks.SelectedSubjectId > 0 && !string.IsNullOrEmpty(ViewMarks.ExamType))
                {
                    // Load marks based on exam type
                    if (ViewMarks.ExamType == "Internal")
                    {
                        LoadInternalMarks();
                    }
                    else if (ViewMarks.ExamType == "Final")
                    {
                        LoadFinalMarks();
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please select a subject and exam type.";
                }

                return Page();
            }

            public IActionResult OnPostUpdateMarks()
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

                try
                {
                    if (ViewMarks.ExamType == "Internal")
                    {
                        UpdateInternalMarks();
                    }
                    else if (ViewMarks.ExamType == "Final")
                    {
                        UpdateFinalMarks();
                    }

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Marks successfully updated.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                }

                return Page();
            }

            private void LoadInternalMarks()
            {
                ViewMarks.LoadedMarks = _context.internals
                    .Where(i => i.sub_id == ViewMarks.SelectedSubjectId &&
                                i.exm_type == $"{ViewMarks.ExamType} {ViewMarks.InternalNumber}")
                    .Join(_context.studentdetails,
                        exam => exam.student_id,
                        student => student.student_id,
                        (exam, student) => new ViewMarksModel.MarksEntry
                        {
                            StudentId = student.student_id,
                            RegistrationNo = student.reg_num,
                            StudentName = student.student_name,
                            ScoredMarks = exam.scored_marks,
                            MaxMarks = exam.max_mark
                        })
                    .ToList();

                ViewMarks.ShowNoMarksMessage = !ViewMarks.LoadedMarks.Any();
            }

            private void LoadFinalMarks()
            {
                ViewMarks.LoadedMarks = _context.marks
                    .Where(m => m.sub_id == ViewMarks.SelectedSubjectId)
                    .Join(_context.studentdetails,
                        exam => exam.student_id,
                        student => student.student_id,
                        (exam, student) => new ViewMarksModel.MarksEntry
                        {
                            StudentId = student.student_id,
                            RegistrationNo = student.reg_num,
                            StudentName = student.student_name,
                            ScoredMarks = exam.sem_marks,
                            MaxMarks = exam.max_marks
                        })
                    .ToList();

                ViewMarks.ShowNoMarksMessage = !ViewMarks.LoadedMarks.Any();
            }

            private void UpdateInternalMarks()
            {
                var examType = $"{ViewMarks.ExamType} {ViewMarks.InternalNumber}";
                foreach (var entry in ViewMarks.LoadedMarks)
                {
                    var existingMarks = _context.internals
                        .FirstOrDefault(i => i.student_id == entry.StudentId &&
                                             i.sub_id == ViewMarks.SelectedSubjectId &&
                                             i.exm_type == examType);

                    if (existingMarks != null)
                    {
                        existingMarks.scored_marks = entry.ScoredMarks;
                    }
                }
            }

            private void UpdateFinalMarks()
            {
                foreach (var entry in ViewMarks.LoadedMarks)
                {
                    var existingMarks = _context.marks
                        .FirstOrDefault(m => m.student_id == entry.StudentId &&
                                             m.sub_id == ViewMarks.SelectedSubjectId);

                    if (existingMarks != null)
                    {
                        existingMarks.sem_marks = entry.ScoredMarks;
                    }
                }
            }

            private void LoadAssignedSubjects(int facultyId)
            {
                ViewMarks.AssignedSubjects = _context.facultysubjects
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
