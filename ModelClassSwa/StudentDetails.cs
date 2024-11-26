namespace ERP_System.ModelClassSwa
{
    public class StudentDetails
    {
        public int student_id { get; set; }
        public string student_name { get; set; }
        public string reg_num { get; set; }
        public string contact_no { get; set; }
        public string contact_email { get; set; }
        public int sem_id { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public int stream_id { get; set; }
        public int college_id { get; set; }
        public DateOnly joining_date { get; set; }
        public bool is_active { get; set; }
    }
}
