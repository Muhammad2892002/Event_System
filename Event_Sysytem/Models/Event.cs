using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Models
{
    public class Event
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
        [ForeignKey("LocationNavigation")]
        public long Location { get; set; }

        public Lookup LocationNavigation { get; set; }

        
        public DateTime EventDate { get; set; }

        public long MaxParticipants { get; set; }
        [ForeignKey("Organizer")]
        public long OrganizerId { get; set; }

        public Attendee Organizer { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
