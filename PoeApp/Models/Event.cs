using PoeApp.Models;

namespace PoeApp.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime EventDate { get; set; }
        public string Description { get; set; }
        public int VenueID { get; set; }
        public Venue? Venue { get; set; }
        public int? EventTypeID { get; set; }
        public EventType? EventType { get; set; }
    }
}