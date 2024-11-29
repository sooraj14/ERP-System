using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace ERP_System.Pages.Admin
{
    
    public class AdminDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;



       /* public List<Studentinfo> studentdetails { get; set; } = new List<Studentinfo>();*/

        [BindProperty]
        public StudentDetails stu { get; set; }

        public List<Studentinfo> stuinfo { get; set; } = new List<Studentinfo>();

        public List<Data.Entity.Faculty> faculties { get; set; } = new List<Data.Entity.Faculty>();
        public List<CollegeAdmin> clgadmins { get; set; } = new List<CollegeAdmin>();
        [BindProperty]

        public List<Streams> streamofclgs { get; set; } = new List<Streams>();
        [BindProperty]
        public TeacherDetails td { get; set; }
        public AdminDashboardModel(ApplicationDbContext context)
        {
            _context = context;
                
        }
        public IActionResult OnGet()
        {

            int? college_id = HttpContext.Session.GetInt32("college_id");
            if (college_id == null)
            {
                return RedirectToPage("/Admin/AdminLogin");
            }
            var admin = _context.collegeadmins.FirstOrDefault(ca => ca.college_id == college_id);
            var streamavailable = _context.streamdetails.Where(sd=>sd.college_id == college_id).ToList();
            var studentinfo = _context.studentdetails.Where(si => si.college_id == college_id).ToList();
            var teacherinfo= _context.facultydetails.Where(fa=> fa.college_id==college_id).ToList();
            if (streamavailable != null) 
            {
                streamofclgs.AddRange(streamavailable); 
            }
            if(studentinfo != null && studentinfo.Any())
            {
                stuinfo.AddRange(studentinfo);
            }
            if(teacherinfo != null)
            {
                faculties.AddRange(teacherinfo);
            }
            if (admin != null)
            {
                clgadmins.Add(admin);
            }

            return Page();

        }

        public IActionResult OnPostStudentDetails(int stream_id)
        {
            int? college_ids = HttpContext.Session.GetInt32("college_id");
            var streamavailables = _context.streamdetails.Find(college_ids);

            var studentregister = new Studentinfo
            {
                student_name = stu.student_name,
                reg_num = stu.reg_num,
                contact_no = stu.contact_no,
                contact_email = stu.contact_email,
                sem_id = stu.sem_id,
                password = stu.password,
                address = stu.address,
                stream_id = stream_id,
                college_id = (int)college_ids,
                is_active = true

            }; _context.studentdetails.Add(studentregister);
            _context.SaveChanges();

          /*  stuinfo = _context.studentdetails
     .Where(sd => sd.college_id == college_ids && sd.stream_id==stream_id)
     .ToList();*/
            
            return RedirectToPage();



        }

        public IActionResult OnPostTeacherDetails(int stream_id)
        {
            int? college_ids = HttpContext.Session.GetInt32("college_id");
            if (college_ids != null) {
                var teacherdetails = new Data.Entity.Faculty
                {
                    fac_name = td.fac_name,
                    fac_email = td.fac_email,
                    fac_password = td.fac_password,
                    fac_phone = td.fac_phone,
                    fac_address = td.fac_address,
                    qualification = td.qualification,
                    experience = td.experience,
                    stream_id = stream_id,
                    college_id = (int)college_ids,
                    is_active = true
                };
             _context.facultydetails.Add(teacherdetails);
                _context.SaveChanges();
                return RedirectToPage();
            };
           

            return Page();

        }
    }
}
