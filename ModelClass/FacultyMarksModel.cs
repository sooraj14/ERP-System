using ERP_System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ERP_System.Models
{
    public class FacultyMarksModel
    {
        [Required(ErrorMessage = "Please select a subject.")]
        public int SelectedSubjectId { get; set; }

        public List<Subject> AssignedSubjects { get; set; } = new();

        [Required(ErrorMessage = "Please select an exam type.")]
        public string ExamType { get; set; }

        public int? InternalNumber { get; set; }

        [Range(1, 100, ErrorMessage = "Max marks must be between 1 and 100.")]
        public int MaxMarks { get; set; }

        public List<Studentinfo> LoadedStudents { get; set; } = new();

        public List<StudentMarksEntry> StudentMarks { get; set; } = new();

        public bool ShowNoStudentsMessage { get; set; }

        public class StudentMarksEntry
        {
            public int StudentId { get; set; }

            [Range(0, 100, ErrorMessage = "Scored marks must be between 0 and 100.")]
            public int ScoredMarks { get; set; }
        }
    }
}
