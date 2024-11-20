using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Subject
    {
        [Key]
        public int sub_id { get; set; }
        public int sem_id { get; set; }
        public int stream_id { get; set; }
        public int fac_id { get; set; }
        public int college_id { get; set; }
        public string sub_name { get; set; }
        public bool is_active { get; set; }
    }
}
