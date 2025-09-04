using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;
using ToDoWebAPI.Data.Models;

#nullable disable

namespace ToDoWebAPI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var roleNames = typeof(RoleTypes)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => (string)f.GetValue(null))
                .ToArray();

            foreach (var roleName in roleNames)
            {
                migrationBuilder.InsertData(
                        table: "AspNetRoles",
                        columns: new[] { "Id", "Name", "NormalizedName" },
                        values: new object[]
                        {
                            Guid.NewGuid().ToString(),
                            roleName,
                            roleName.ToUpper(),
                        });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
