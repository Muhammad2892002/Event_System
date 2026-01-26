using System.ComponentModel.DataAnnotations.Schema;

namespace Event_System.Models
{
    public class Attendee
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

         public string LastName { get; set; }
        public string Email { get; set; }
        [ForeignKey("UserNavigation")]
        public long UserId { get; set; }
        public User UserNavigation { get; set; }
        public bool Gender { get; set; }=false;




        [ForeignKey("RoleNavigation")]
        public  long Role { get; set; }   
        public Lookup RoleNavigation { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }





    }
}
