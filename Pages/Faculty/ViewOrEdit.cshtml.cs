using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ERP_System.Data.context;
using ERP_System.Models;
using System.Linq;
using ERP_System.Data.Entity;

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
                var subject = _context.subjects.FirstOrDefault(s =>
                    s.sub_id == ViewMarks.SelectedSubjectId && s.is_active);

                if (subject != null)
                {
                    if (ViewMarks.ExamType == "Internal")
                    {
                        if (!ViewMarks.InternalNumber.HasValue)
                        {
                            TempData["ErrorMessage"] = "Please select an internal assessment number.";
                            return Page();
                        }

                        ViewMarks.StudentMarks = _context.internals
                            .Where(i => i.sub_id == ViewMarks.SelectedSubjectId &&
                                   i.exm_type == $"Internal {ViewMarks.InternalNumber}")
                            .Join(_context.studentdetails,
                                  internalExam => internalExam.student_id,
                                  student => student.student_id,
                                  (internalExam, student) => new ViewMarksModel.StudentMarksDetail
                                  {
                                      student_id = student.student_id,
                                      student_name = student.student_name,
                                      reg_num = student.reg_num,
                                      ScoredMarks = internalExam.scored_marks,
                                      MaxMarks = internalExam.max_mark,
                                      Percentage = Math.Round((double)internalExam.scored_marks / internalExam.max_mark * 100, 2)
                                  })
                            .OrderBy(s => s.student_name)
                            .ToList();
                    }
                    else if (ViewMarks.ExamType == "Final")
                    {
                        ViewMarks.StudentMarks = _context.marks
                            .Where(m => m.sub_id == ViewMarks.SelectedSubjectId)
                            .Join(_context.studentdetails,
                                  marks => marks.marks_id,
                                  student => student.student_id,
                                  (marks, student) => new ViewMarksModel.StudentMarksDetail
                                  {
                                      student_id = student.student_id,
                                      student_name = student.student_name,
                                      reg_num = student.reg_num,
                                      MarksId = marks.marks_id,
                                      ScoredMarks = marks.sem_marks,
                                      MaxMarks = marks.max_marks,
                                      Percentage = Math.Round((double)marks.sem_marks / marks.max_marks * 100, 2)
                                  })
                            .OrderBy(s => s.student_name)
                            .ToList();
                    }

                    ViewMarks.ShowNoStudentsMessage = !ViewMarks.StudentMarks.Any();
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

            ModelState.Clear();

            var subject = _context.subjects.FirstOrDefault(s =>
                s.sub_id == ViewMarks.SelectedSubjectId && s.is_active);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Selected subject not found.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            bool hasValidationErrors = false;
            foreach (var mark in ViewMarks.StudentMarks)
            {
                if (mark.ScoredMarks < 0 || mark.ScoredMarks > mark.MaxMarks)
                {
                    hasValidationErrors = true;
                    break;
                }
            }

            if (hasValidationErrors)
            {
                TempData["ErrorMessage"] = "Invalid marks entry. Please ensure marks are within the valid range.";
                LoadAssignedSubjects(facultyId.Value);
                return Page();
            }

            if (ViewMarks.ExamType == "Internal")
            {
                foreach (var mark in ViewMarks.StudentMarks)
                {
                    var existingMark = _context.internals.FirstOrDefault(i =>
                        i.student_id == mark.student_id &&
                        i.sub_id == ViewMarks.SelectedSubjectId &&
                        i.exm_type == $"Internal {ViewMarks.InternalNumber}");

                    if (existingMark != null)
                    {
                        existingMark.scored_marks = mark.ScoredMarks;
                    }
                }
            }
            else if (ViewMarks.ExamType == "Final")
            {
                foreach (var mark in ViewMarks.StudentMarks)
                {
                    var existingMark = _context.marks.FirstOrDefault(m =>
                        m.sub_id == ViewMarks.SelectedSubjectId &&
                        m.stream_id == subject.stream_id &&
                        m.sem_id == subject.sem_id &&
                        m.marks_id == mark.student_id);

                    if (existingMark != null)
                    {
                        existingMark.sem_marks = mark.ScoredMarks;
                    }
                    else
                    {
                        var newMark = new Marks
                        {
                            sub_id = ViewMarks.SelectedSubjectId,
                            stream_id = subject.stream_id,
                            sem_id = subject.sem_id,
                            college_id = subject.college_id,
                            sem_marks = mark.ScoredMarks,
                            max_marks = mark.MaxMarks
                        };
                        _context.marks.Add(newMark);
                    }
                }
            }

            _context.SaveChanges();
            TempData["SuccessMessage"] = "Marks updated successfully.";
            return RedirectToPage();
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

