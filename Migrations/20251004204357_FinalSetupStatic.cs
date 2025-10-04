using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PARCIAL_programaci_n.Migrations
{
    /// <inheritdoc />
    public partial class FinalSetupStatic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b94c32b5-3135-4603-b0e6-e910229381c1",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "95df7a0c-64b9-4aa9-b88c-2b454cad3c4f", "jazarelly_zapata@usmp.pe", "JAZARELLY_ZAPATA@USMP.PE", "JAZARELLY_ZAPATA@USMP.PE", "AQAAAAIAAYagAAAAEDdYjas9Kz7TmhiZTgrjhoJcb/tnw8Ph3COFynlaeApwqOLfj00zhaRvI+jzcUclGw==", "c7aa65af-20cf-4a55-b031-7800c10f650c", "jazarelly_zapata@usmp.pe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b94c32b5-3135-4603-b0e6-e910229381c1",
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "94796d73-f9bd-4d10-84f6-a4955b8b58fd", "coordinador@portal.uni", "COORDINADOR@PORTAL.UNI", "COORDINADOR@PORTAL.UNI", "AQAAAAIAAYagAAAAEP6XzeE8ueQs91l8Tt4YulN8CUOp/Zb19lmoU6kJd2Po1UtgmVIF6yPvtySybtg3fw==", "7dbac993-55d6-4b0e-9062-aab2b4da7945", "coordinador@portal.uni" });
        }
    }
}
