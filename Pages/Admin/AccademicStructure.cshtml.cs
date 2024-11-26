using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Admin
{
    public class AccademicStructureModel : PageModel
    {
        private readonly ApplicationDbContext _context;


        public List<Streams> stream { get; set; } = new List<Streams>();
        public List<CollegeAdmin> clgadmins { get; set; } = new List<CollegeAdmin>();

        [BindProperty]
        public AddSubject enetrsub { get; set; }
        [BindProperty]
        public AddBranch branch { get; set; }
        public List<Subject> subjectdetails { get; set; } = new List<Subject>();

        public List<Subject> sub { get; set; } = new List<Subject>();
        public AccademicStructureModel(ApplicationDbContext context)
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


        public IActionResult OnPostSendBranchDetails(List<string> stream_name)
        {
            int? college_id = HttpContext.Session.GetInt32("college_id");
            if (college_id == null)
            {
                return RedirectToPage("/Admin/AdminLogin");
            }
            foreach (var name in stream_name)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    var storebranch = new Streams
                    {
                        college_id = (int)college_id,
                        stream_name = name.Trim(),
                        is_active = true
                    };
                    _context.streamdetails.Add(storebranch);
                }
            }
            _context.SaveChanges();
            stream = _context.streamdetails
       .Where(sd => sd.college_id == college_id) 
       .ToList();
            return Page();
        }

        public IActionResult OnPostDeleteBranch(int stream_id)
        {

            if (stream_id != null)
            {
             var deletestream =   _context.streamdetails.Find(stream_id);
                _context.streamdetails.Remove(deletestream);
                _context.SaveChanges();
                return RedirectToPage();
            }
            
            return Page();
        }
        public IActionResult OnPostSubjectData(int stream_id)
        {
            int? college_id = HttpContext.Session.GetInt32("college_id");
            if(college_id== null)
            {
                return RedirectToPage("/Admin/AdminLogin");
            }
          
            var subjectdetails = new Subject
            {
                sem_id = enetrsub.sem_id,
                stream_id =  stream_id,
               college_id= (int)college_id,
                sub_name= enetrsub.sub_name,
               is_active = true

            };
            _context.subjects.Add(subjectdetails);
            _context.SaveChanges();

            sub =  _context.subjects.Where(sd => sd.college_id == college_id).ToList();
         
            return Page();
        }
    }

    
}
