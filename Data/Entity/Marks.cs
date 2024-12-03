using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Marks
    {
        [Key]
        public int marks_id { get; set; }
        public int student_id { get; set; }
        public int college_id { get; set; }
        public int stream_id { get; set; }
        public int sem_id { get; set; }
        public int sub_id { get; set; }
        public int sem_marks {  get; set; }
        public int max_marks  { get; set; }
    }
}
