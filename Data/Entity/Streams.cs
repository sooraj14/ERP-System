using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Streams
    {
        [Key]
        public int stream_id {  get; set; }
        public string college_id { get; set; }
        public string stream_name { get; set; }
        public bool is_active {  get; set; }

    }

}
