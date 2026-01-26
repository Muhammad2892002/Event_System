namespace Event_System.Dtos.EventDto
{
    public class ShowAllEvents
    {
      

        public string Title { get; set; }
        public string Description { get; set; }

        public long Location { get; set; }

        public string LocationName { get; set; }




        public DateTime EventDate { get; set; }

        public long MaxParticipants { get; set; }

        public long OrganizerId { get; set; }

        public string OrganizerName { get; set; }


        public DateTime CreatedAt { get; set; }
    }
}
