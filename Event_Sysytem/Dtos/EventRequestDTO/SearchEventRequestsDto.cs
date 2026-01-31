namespace Event_System.Dtos.EventRequestDTO
{
    public class SearchEventRequestsDto
    {
        public long ? EventId { get; set; }
        public long? UserId { get; set; }
        public int ? Status { get; set; } // e.g., "Pending", "Approved", "Rejected"
        public DateTime ? RequestedAt { get; set; }
    }
}
