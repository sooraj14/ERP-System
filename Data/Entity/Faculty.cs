using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Faculty
    {
        [Key] 
        public int fac_id   { get; set; }
        public string fac_name { get; set; }
        public string fac_email { get; set; }
        public string fac_password { get; set; }
        public string fac_phone { get; set; }
        public string fac_address { get; set; }
        public string qualification { get; set; }
        public string experience { get; set; }
        public int stream_id { get; set; }
        public int college_id { get; set; }
        public bool is_active { get; set; }

    }
}
