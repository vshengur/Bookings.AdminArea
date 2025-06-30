using Microsoft.EntityFrameworkCore.Migrations;

namespace Bookings.PostgresRepositories.Migrations;

public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "bookings",
            columns: table => new
            {
                id = table.Column<string>(type: "text", nullable: false),
                hotel_id = table.Column<string>(type: "text", nullable: false),
                room_id = table.Column<string>(type: "text", nullable: false),
                check_in_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                check_out_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                guest_name = table.Column<string>(type: "text", nullable: false),
                guest_email = table.Column<string>(type: "text", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                price = table.Column<double>(type: "double precision", nullable: false),
                status = table.Column<string>(type: "text", nullable: false),
                adults = table.Column<int>(type: "integer", nullable: false),
                kids = table.Column<int>(type: "integer", nullable: false),
                category = table.Column<string>(type: "text", nullable: false),
                state_id = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_bookings", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "booking_states",
            columns: table => new
            {
                id = table.Column<string>(type: "text", nullable: false),
                booking_id = table.Column<string>(type: "text", nullable: false),
                state = table.Column<int>(type: "integer", nullable: false),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_booking_states", x => x.id);
                table.ForeignKey(
                    name: "fk_booking_states_bookings_booking_id",
                    column: x => x.booking_id,
                    principalTable: "bookings",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_booking_states_booking_id",
            table: "booking_states",
            column: "booking_id");

        migrationBuilder.CreateIndex(
            name: "ix_bookings_hotel_id",
            table: "bookings",
            column: "hotel_id");

        migrationBuilder.CreateIndex(
            name: "ix_bookings_room_id",
            table: "bookings",
            column: "room_id");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "booking_states");

        migrationBuilder.DropTable(
            name: "bookings");
    }
} 