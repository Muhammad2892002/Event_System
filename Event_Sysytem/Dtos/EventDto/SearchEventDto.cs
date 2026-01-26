using Event_System.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Dtos.EventDto
{
    public class SearchEventDto
    {
      

        public string? Title { get; set; }
        public DateTime? EventDate { get; set; }
        public long? OrganizerId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public long? LocationId { get; set; }
    }
}
