using ERP_System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace ERP_System.ModelClass
{
    public class AttendanceModel
    {
        public List<Subject> AssignedSubjects { get; set; } = new();
        public List<StudentAttendanceInfo> Students { get; set; } = new();
        public bool ShowNoStudentsMessage { get; set; }

        [Required(ErrorMessage = "Please select a subject")]
        public int SelectedSubjectId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateOnly AttendanceDate { get; set; }

        [Required(ErrorMessage = "Time is required")]
        public TimeOnly AttendanceTime { get; set; }

        public List<int> PresentStudentIds { get; set; } = new();
        public List<TimeOnly> ExistingSessionTimes { get; set; } = new();
    }

    public class StudentAttendanceInfo : Studentinfo
    {
        public int TotalClasses { get; set; }
        public int AttendedClasses { get; set; }
        public double AttendancePercentage { get; set; }
    }
}
