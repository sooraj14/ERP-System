using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Features
    {
        [Key]
        public int fea_id {  get; set; }
        public string fea_name { get; set; }
        public string path { get; set; }
        public string plan_type { get; set; }
        public bool active { get; set; }
        public string role { get; set; }
       
    }
}
