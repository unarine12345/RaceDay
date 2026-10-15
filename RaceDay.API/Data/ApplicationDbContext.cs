using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;

namespace RaceDay.API.Data;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    // Use RaceDayRoles for the assignment's custom Role entity. IdentityDbContext already has Roles.
    public DbSet<Role> RaceDayRoles => Set<Role>();
    public DbSet<Organiser> Organisers => Set<Organiser>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<EventEnrolment> EventEnrolments => Set<EventEnrolment>();
    public DbSet<Result> Results => Set<Result>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Role>().HasIndex(r => r.RoleName).IsUnique();
        builder.Entity<User>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Organiser>().HasOne(o => o.User).WithOne(u => u.Organiser).HasForeignKey<Organiser>(o => o.UserId);
        builder.Entity<Participant>().HasOne(p => p.User).WithOne(u => u.Participant).HasForeignKey<Participant>(p => p.UserId);
        builder.Entity<Event>().HasOne(e => e.Organiser).WithMany(o => o.Events).HasForeignKey(e => e.OrganiserId);
        builder.Entity<Category>().HasOne(c => c.Event).WithMany(e => e.Categories).HasForeignKey(c => c.EventId);
        builder.Entity<EventEnrolment>().HasIndex(e => new { e.ParticipantId, e.EventId }).IsUnique();
        builder.Entity<EventEnrolment>().HasOne(e => e.Participant).WithMany(p => p.EventEnrolments).HasForeignKey(e => e.ParticipantId);
        builder.Entity<EventEnrolment>().HasOne(e => e.Event).WithMany(e => e.EventEnrolments).HasForeignKey(e => e.EventId);
        builder.Entity<EventEnrolment>().HasOne(e => e.Category).WithMany(c => c.EventEnrolments).HasForeignKey(e => e.CategoryId);
        builder.Entity<Result>().HasIndex(r => r.EventEnrolmentId).IsUnique();
        builder.Entity<Result>().HasOne(r => r.EventEnrolment).WithOne(e => e.Result).HasForeignKey<Result>(r => r.EventEnrolmentId);
        builder.Entity<Event>().Property(e => e.Distance).HasPrecision(8, 2);
    }
}
