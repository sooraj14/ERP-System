using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class CollegeAdmin
    {
        [Key]
        public int admin_id {  get; set; }
        public int college_id { get; set; }
        public string college_name { get; set; }
        public string college_email { get; set; }
        public string college_pass { get; set; }
        public DateTime purchased { get; set; }

    }
}
