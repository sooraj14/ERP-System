using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Semester
    {
        [Key]
        public int semester_id { get; set; }
        public int college_id { get; set; }
        public int stream_id { get; set; }
        public int sem_id { get; set; }
        public bool is_active { get; set; }


    }
}
