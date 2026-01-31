using Event_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Event_System.DbContexts
{
    public class EventContext :DbContext
    {
        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Event>().HasOne(ev => ev.LocationNavigation)
                .WithMany()
                .HasForeignKey(ev => ev.Location)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Attendee>().HasOne(ev => ev.RoleNavigation)
                .WithMany()
                .HasForeignKey(ev => ev.Role)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Lookup>().HasData(
                new Lookup { Id = 1, MajorCode = 0, MinorCode = 0, Name = "Role" },
                 new Lookup { Id = 2, MajorCode = 0, MinorCode = 1, Name = "Admin" },
                  new Lookup { Id = 3, MajorCode = 0, MinorCode = 2, Name = "Organizer" },
                   new Lookup { Id = 4, MajorCode = 0, MinorCode = 3, Name = "Attendee" },


                    new Lookup { Id = 5, MajorCode = 1, MinorCode = 1, Name = "Location" },
                     new Lookup { Id = 6, MajorCode = 1, MinorCode = 2, Name = "Amman-Abdali Mall" },
                      new Lookup { Id = 7, MajorCode = 1, MinorCode = 3, Name = "Amman-The Boulevard, Abdali" },
                        new Lookup { Id = 8, MajorCode = 1, MinorCode = 4, Name = "Grand Hyatt Amman" },
                          new Lookup { Id = 9, MajorCode = 1, MinorCode = 5, Name = "Le Royal Hotel Amman" }




                );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 5, UserName = "admin", HashedPassword = "$2a$11$sZCZCInSvcGZh3AT8ZylyeU4aonLYzQHos3czzJOoLs0./KdnlS8y", IsAdmin = true }
                );
            modelBuilder.Entity<Attendee>().HasIndex(u => u.UserId).IsUnique();

            modelBuilder.Entity<EventParticipant>().HasIndex(ep => new { ep.UserId, ep.EventId }).IsUnique();


            modelBuilder.Entity<EventParticipant>()
    .HasOne(ep => ep.UserNavigation)
    .WithMany()
    .HasForeignKey(ep => ep.UserId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.EventNavigation)
                .WithMany()
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade); // safe




            modelBuilder.Entity<EventRequest>()
     .HasOne(er => er.UserNavigation)
     .WithMany()
     .HasForeignKey(er => er.UserId)
     .OnDelete(DeleteBehavior.Restrict); // <--- THIS FIXES THE MULTIPLE CASCADE PATHS

            modelBuilder.Entity<EventRequest>()
                .HasOne(er => er.EventNavigation)
                .WithMany()
                .HasForeignKey(er => er.EventId)
                .OnDelete(DeleteBehavior.Cascade);



        }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<EventParticipant> EventParticipants { get; set; }

        public DbSet<EventRequest> EventRequests { get; set; }

        public DbSet<Lookup> Lookups { get; set; }

        public DbSet<OrganizerRequest> OrganizerRequests { get; set; }

        public DbSet<User> Users { get; set; }








    }
}
