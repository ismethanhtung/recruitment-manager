using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.Data.Model
{
    public class ImportDataModel
    {
        public class ImportUserModel
        {
            public List<CreateUserModel> ListUser { get; set; }
            public List<CreateCandidateModel> ListCan { get; set; }
            public List<CreateInterviewerModel> ListInter { get; set; }
            public List<CreateRecruiterModel> listRec { get; set; }
        }

        public class ImportJobPostModel
        {
            public string RecEmail { get; set; }
            public CreateJobModel JobData { get; set; }
        }

        public class ImportEventPostModel
        {
            public string RecEmail { get; set; }
            public CreateEventModel EventData { get; set; }
        }
    }
}
