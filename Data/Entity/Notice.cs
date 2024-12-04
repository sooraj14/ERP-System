using System.ComponentModel.DataAnnotations;

namespace ERP_System.Data.Entity
{
    public class Notice
    {
        [Key]
        public int notice_id { get; set; }
        public string notice_Data { get; set; }
         public int college_id { get; set; }
        public int stream_id      { get; set; }

    }
}
