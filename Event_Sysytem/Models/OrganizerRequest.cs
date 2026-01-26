using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Models
{
    public class OrganizerRequest
    {
        public long Id { get; set; }
        [ForeignKey("UserNavigation")]
        public long UserId { get; set; }
         public Attendee UserNavigation { get; set; }

        public int Status { get; set; } // e.g., "Pending", "Approved", "Rejected"

        public DateTime RequestedAt { get; set; }
    }
}
