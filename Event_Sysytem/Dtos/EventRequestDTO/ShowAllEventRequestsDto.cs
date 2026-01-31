using Event_System.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Dtos.EventRequestDTO
{
    public class ShowAllEventRequestsDto
    {
        public long Id { get; set; }

      
        public long EventId { get; set; }

        public string EventName { get; set; }

     

       
        public long UserId { get; set; }

        public string Name { get; set; }

        public string email { get; set; }

     


        public int Status { get; set; } // e.g., "Pending", "Approved", "Rejected"


        public string StatusName { get; set; }


        public DateTime RequestedAt { get; set; }
    }
}
