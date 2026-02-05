namespace Event_System.Dtos.AttendeeDto
{
    public class Update_Attendee
    {
        public string UserName { get; set; }

        public string Password { get; set; }
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }

        public long UserId { get; set; }

        public bool Gender { get; set; } = false;
        public long? Role { get; set; }







        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
