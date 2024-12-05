using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP_System.Data.Entity;

namespace ERP_System.Models
{
    public class ViewMarksModel
    {
        [Required(ErrorMessage = "Please select a subject.")]
            public int SelectedSubjectId { get; set; }
            public List<Subject> AssignedSubjects { get; set; } = new();

            [Required(ErrorMessage = "Please select an exam type.")]
            public string ExamType { get; set; }

            public int? InternalNumber { get; set; }

            public List<MarksEntry> LoadedMarks { get; set; } = new();
            public bool ShowNoMarksMessage { get; set; }

            public class MarksEntry
            {
                public int StudentId { get; set; }
                public string RegistrationNo { get; set; }
                public string StudentName { get; set; }

                [Range(0, 100, ErrorMessage = "Scored marks must be between 0 and 100.")]
                public int ScoredMarks { get; set; }
                public int MaxMarks { get; set; }
            }
        }
    }

