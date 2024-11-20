using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class InternalExam
    {
        [Key]
        public int exm_id { get; set; }
        public int stream_id { get; set; }
        public int college_id { get; set; }
        public int sub_id { get; set; }
        public int sem_id { get; set; }
        public int student_id { get; set; }
        public string exm_type { get; set; }
        public int scored_marks { get; set; }
        public int max_mark { get; set; }
    }
}
