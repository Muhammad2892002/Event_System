using Event_System.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Dtos.EventRequestDto
{
    public class EventRequestDto
    {
        public long Id { get; set; }

       
        public long EventId { get; set; }

       

       
        public long UserId { get; set; }

       


        public int Status { get; set; } // e.g., "Pending", "Approved", "Rejected"


        public DateTime RequestedAt { get; set; }
    }
}
