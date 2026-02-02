namespace Event_System.Dtos.EventParticipantsDtos
{
    public class ShowAllParticipantsDto
    {
        public long Id { get; set; }
        public long OrganizerId { get; set; }
        public long LocationId { get; set; }
        public long AttendeeId { get; set; }
        public long EventId { get; set; }
        public string AttendeeName { get; set; }
        public string Email { get; set; }

        public string EventName { get; set; }

        public string Location { get; set; }

        public DateTime HeldAt { get; set; }
        public string OrganizerName { get; set; }
    }
}
