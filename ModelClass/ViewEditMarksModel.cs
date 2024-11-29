using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace ERP_System.Models
{
    public class ViewMarksModel
    {
        [Required(ErrorMessage = "Please select a subject")]
        public int SelectedSubjectId { get; set; }

        [Required(ErrorMessage = "Please select an exam type")]
        public string ExamType { get; set; }

        public int? InternalNumber { get; set; }

        public List<Subject> AssignedSubjects { get; set; } = new();

        public List<StudentMarksDetail> StudentMarks { get; set; } = new();

        public bool ShowNoStudentsMessage { get; set; }

        public class StudentMarksDetail
        {
            public int student_id { get; set; }
            public string student_name { get; set; }
            public string reg_num { get; set; }
            public int MarksId { get; set; }

            [Range(0, 100, ErrorMessage = "Scored marks must be between 0 and 100")]
            public int ScoredMarks { get; set; }

            public int MaxMarks { get; set; }
            public double Percentage { get; set; }
        }
    }
}