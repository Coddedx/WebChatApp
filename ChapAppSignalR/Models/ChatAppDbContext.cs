using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ChapAppSignalR.Models
{
    public class ChatAppDbContext : IdentityDbContext<AppUser>
    {
        public ChatAppDbContext()
        {
                
        }
        public ChatAppDbContext(DbContextOptions<ChatAppDbContext> options) : base(options)
        {
                
        }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //override onc yaz tab bas direk çıkıyo
        {
            optionsBuilder.UseSqlServer(
                "Server=LAPTOP-QT5SFJG4\\SQLEXPRESS; Database=ChatAppDbContext; Trusted_Connection=true; Encrypt=false;");
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Sender ilişkisini yapılandırma
            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)  // 'SentMessages' property'ini ApplicationUser sınıfında 
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Receiver ilişkisini yapılandırma
            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)  // 'ReceivedMessages' property'ini ApplicationUser sınıfında 
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
            
            
            //builder.Entity<Message>()
            //    .HasOne<AppUser>(a => a.Sender)
            //    .WithMany(d => d.Messages)
            //    .HasForeignKey(d => d.UserId);
        }
    }
}
