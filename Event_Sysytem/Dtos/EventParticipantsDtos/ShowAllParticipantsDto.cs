namespace Event_System.Dtos.EventParticipantsDtos
{
    public class ShowAllParticipantsDto
    {
        public string AttendeeName { get; set; }
        public string Email { get; set; }

        public string EventName { get; set; }

        public string Location { get; set; }

        public DateTime HeldAt { get; set; }
        public string OrganizerName { get; set; }
    }
}
