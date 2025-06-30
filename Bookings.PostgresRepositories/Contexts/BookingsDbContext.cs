using Bookings.Domain;
using Bookings.PostgresRepositories.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bookings.PostgresRepositories.Contexts;

public class BookingsDbContext : DbContext
{
    private readonly ILogger<BookingsDbContext> _logger;
    private readonly PostgresDatabaseSettings _settings;

    public BookingsDbContext(
        DbContextOptions<BookingsDbContext> options,
        ILogger<BookingsDbContext> logger,
        IOptions<PostgresDatabaseSettings> settings) : base(options)
    {
        _logger = logger;
        _settings = settings.Value;
    }

    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingStateEntity> BookingStates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_settings.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация для Booking
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.ToTable("bookings");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.HotelId).HasColumnName("hotel_id").IsRequired();
            entity.Property(e => e.RoomId).HasColumnName("room_id").IsRequired();
            entity.Property(e => e.GuestName).HasColumnName("guest_name").IsRequired();
            entity.Property(e => e.GuestEmail).HasColumnName("guest_email").IsRequired();
            entity.Property(e => e.CheckInDate).HasColumnName("check_in_date").IsRequired();
            entity.Property(e => e.CheckOutDate).HasColumnName("check_out_date").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();
            entity.Property(e => e.Price).HasColumnName("price").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").IsRequired();
            entity.Property(e => e.Adults).HasColumnName("adults").IsRequired();
            entity.Property(e => e.Kids).HasColumnName("kids").IsRequired();
            entity.Property(e => e.Category).HasColumnName("category").IsRequired();
            entity.Property(e => e.StateId).HasColumnName("state_id").IsRequired();
        });

        // Конфигурация для BookingState
        modelBuilder.Entity<BookingStateEntity>(entity =>
        {
            entity.ToTable("booking_states");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").IsRequired();
            entity.Property(e => e.BookingId).HasColumnName("booking_id").IsRequired();
            entity.Property(e => e.Status).HasColumnName("state").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").IsRequired();
            
            // Связь с Booking
            entity.HasOne<Booking>()
                .WithMany()
                .HasForeignKey(e => e.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
} 