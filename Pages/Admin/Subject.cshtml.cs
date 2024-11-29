using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ERP_System.Pages.Admin
{
    public class SubjectModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Facultymappingsubject fms { get; set; }
        public List<Streams> stream { get; set; } = new List<Streams>();


        public List<CollegeAdmin> clgadmins { get; set; } = new List<CollegeAdmin>();

        [BindProperty]
        public AddSubject enetrsub { get; set; }

        [BindProperty]
        public AddBranch branch { get; set; }
        public List<Subject> subjectdetails { get; set; } = new List<Subject>();

        

        public List<Subject> sub { get; set; } = new List<Subject>();

        public List<Data.Entity.Faculty> facsubj { get; set; } = new List<Data.Entity.Faculty>();


        public SubjectModel(ApplicationDbContext context)
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
            var streamavailable = _context.streamdetails.Where(sd => sd.college_id == college_id).ToList();
            var facultyinfo = _context.facultydetails.Where(sd => sd.college_id == college_id).ToList();

            if (facultyinfo != null && facultyinfo.Any())
            {
                facsubj.AddRange(facultyinfo);
            }

            if (streamavailable != null && streamavailable.Any())
            {
                stream.AddRange(streamavailable);

            }


            var subdata = _context.subjects.Where(sd => sd.college_id == college_id).ToList();
            if (subdata != null && subdata.Any())
            {
                sub.AddRange(subdata);
            }


            if (admin != null)
            {
                clgadmins.Add(admin);

            }


            return Page();
        }
        

        public IActionResult OnPostSubjectData(int stream_id, List<string> subject_name)
        {
            int? college_id = HttpContext.Session.GetInt32("college_id");
            if (college_id == null)
            {
                return RedirectToPage("/Admin/AdminLogin");
            }
            foreach (var s in subject_name)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    var subjectdetails = new Subject
                    {
                        sem_id = enetrsub.sem_id,
                        stream_id = stream_id,
                        college_id = (int)college_id,
                        sub_name = s.Trim(),
                        is_active = true

                    };
                    _context.subjects.Add(subjectdetails);
                }
            }
            _context.SaveChanges();

            sub = _context.subjects.Where(sd => sd.college_id == college_id).ToList();

            return RedirectToPage();
        }
    }
}
