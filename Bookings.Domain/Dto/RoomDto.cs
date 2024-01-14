namespace Bookings.Domain.Dto;

public record RoomDto
{
    public string Name { get; set; } = null!;

    public int MaxPersons { get; set; }

    public int AdditionalFreeKids { get; set; }
}