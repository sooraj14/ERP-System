using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ERP_System.ModelClassSwa
{
    public class Facultymappingsubject
    {
        public int fac_id { get; set; }
        public int sub_id { get; set; }
        //public int college_id { get; set; }
        public int stream_id { get; set; }
        public int sem_id { get; set; }

    }
}
