using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce_Shop_NDNB.Migrations
{
    /// <inheritdoc />
    public partial class couponcodetoordermodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "Orders");

        }
    }
}
