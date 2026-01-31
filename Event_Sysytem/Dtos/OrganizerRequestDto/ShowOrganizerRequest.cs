using Event_System.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Dtos.OrganizerRequest
{
    public class ShowOrganizerRequest
    {
        public long Id { get; set; }
      
        public long UserId { get; set; }
        public string Name { get; set; }
    

        public int Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public string StatusNamer { get; set; }

        public DateTime RequestedAt { get; set; }










    }
}
