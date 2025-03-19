using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsternShip.Data.Model
{
    public class MeetingModel
    {
        public string? MeetingId { get; set; }
        public string? Title { get; set; }
        public string? StartUrl { get; set; }
        public string? JoinUrl { get; set; }
        public int? Duration { get; set; }
        public DateTime? Time { get; set; }
    }
    public class MeetingBaseModel
    {
        public string? MeetingId { get; set; }
        public string? Title { get; set; }
        public string? JoinUrl { get; set; }
        public int? Duration { get; set; }
        public DateTime? Time { get; set; }
    }
    public class CreateMeetingModel
    {
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }

    }
    public class MeetingListModel
    {
        //public string? NextPageToken { get; set; }
        public int? TotalCount { get; set; }
        public ICollection<MeetingBaseModel>? Meetings { get; set; }
    }
}
