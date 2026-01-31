namespace Event_System.Dtos.AttendeeDto
{
    public class SearchAttendee
    {

        public string? AteendeeFirstName { get; set; }

        public string? AteendeeLastName { get; set; }

        public Enums.Roles? Role { get; set; }

        public bool? Gender { get; set; }

      

        public bool? IsActivated { get; set; }
    }
}
