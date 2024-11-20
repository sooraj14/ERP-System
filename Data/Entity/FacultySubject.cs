using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class FacultySubject
    {
        [Key]
        public int fac_sub_id {  get; set; }
        public int fac_id { get; set; }
        public int sub_id { get; set; }
        public int college_id { get; set; }
        public int stream_id { get; set; }
        public  int sem_id { get; set; }
    }
}
