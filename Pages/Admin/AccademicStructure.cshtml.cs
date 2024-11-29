using ERP_System.Data.context;
using ERP_System.Data.Entity;
using ERP_System.ModelClassSwa;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ERP_System.Pages.Admin
{
    public class AccademicStructureModel : PageModel
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

        public async Task<IActionResult>  OnGetBranchwithsubjectAsync(int stream_id, int sem_id)
        {
            int? college_id = HttpContext.Session.GetInt32("college_id");
            var teachers = _context.subjects.Where(s => s.college_id == college_id && s.stream_id == stream_id && s.sem_id == sem_id).Select(s => new {s.sub_id, s.sub_name }).ToList();
            Console.WriteLine(teachers.Count());
            return new JsonResult(teachers);
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
            return RedirectToPage();
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
        public IActionResult OnPostSubjectData(int stream_id, List<string> subject_name)
        {
            int? college_id = HttpContext.Session.GetInt32("college_id");
            if(college_id== null)
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

            sub =  _context.subjects.Where(sd => sd.college_id == college_id).ToList();
         
            return RedirectToPage();
        }

        //public IActionResult OnPostFacultyMapSubject()
        //{
       
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }
            


        //    int? college_id = HttpContext.Session.GetInt32("college_id");
        //    if (college_id == null)
        //    {
        //        return RedirectToPage("/Admin/AdminLogin");
        //    }
        //    var facultymapsubj = new FacultySubject
        //    {
        //        fac_id = fms.fac_id,
        //        sub_id = fms.sub_id,
        //        college_id = (int)college_id,
        //        stream_id = fms.stream_id,
        //        sem_id = fms.sem_id,

        //    };

        //    _context.facultysubjects.Add(facultymapsubj);
        //   _context.SaveChanges();
        //    Console.WriteLine("data saved");
        //    //var SuccessMessage = "Faculty successfully mapped to subject.";
        //    return RedirectToPage();
        //}
    }

    
}
