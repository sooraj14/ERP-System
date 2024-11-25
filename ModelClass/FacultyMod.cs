using System.ComponentModel.DataAnnotations;

namespace ERP_System.ModelClass
{
    public class FacultyMod
    {
        [Key] 
        public int fac_id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string fac_name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string fac_email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        public string fac_password { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string fac_phone { get; set; }

        public string fac_address { get; set; }

        [Required(ErrorMessage = "Qualification is required.")]
        public string qualification { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        public string experience { get; set; }

        [Required(ErrorMessage = "Stream ID is required.")]
        public int stream_id { get; set; }

        [Required(ErrorMessage = "College ID is required.")]
        public int college_id { get; set; }

        public bool is_active { get; set; } = true;
    }
}

