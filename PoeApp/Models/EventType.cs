namespace PoeApp.Models
{
    public class EventType
    {
        public int EventTypeID { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public override string ToString() => Name;
    }
}
