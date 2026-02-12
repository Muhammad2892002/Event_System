using Event_System.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Dtos.EventDto
{
    public class CreateEventDto
    {
        public long Id { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }
      
        public long Location { get; set; }

      


        public DateTime EventDate { get; set; }

        public long MaxParticipants { get; set; }
      
        public long? OrganizerId { get; set; }

        
        public DateTime CreatedAt { get; set; }



    }
}
