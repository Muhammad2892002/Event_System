using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Security;

namespace Event_System.Models
{
    public class EventParticipant
    {
        public long Id { get; set; }
        [ForeignKey("EventNavigation")]
        public long EventId { get; set; }
        public Event EventNavigation { get; set; }
        [ForeignKey("UserNavigation")]
        public long UserId { get; set; }
        public Attendee UserNavigation { get; set; }
                            
       
    }
}
