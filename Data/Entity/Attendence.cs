using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Attendence
    {
        [Key]
        public int atten_id {  get; set; }
        public int student_id { get; set; }
        public int college_id { get; set; }
        public int stream_id { get; set; }
        public int sub_id { get; set; }
        public DateOnly date {  get; set; }
        public TimeOnly time { get; set; }
    }
}
