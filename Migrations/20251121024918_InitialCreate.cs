using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameLevels",
                columns: table => new
                {
                    GameLevelId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLevels", x => x.GameLevelId);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    regionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.regionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    roleId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.roleId);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    OptionA = table.Column<string>(type: "TEXT", nullable: true),
                    OptionB = table.Column<string>(type: "TEXT", nullable: true),
                    OptionC = table.Column<string>(type: "TEXT", nullable: true),
                    OptionD = table.Column<string>(type: "TEXT", nullable: true),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    GameLevelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_Questions_GameLevels_GameLevelId",
                        column: x => x.GameLevelId,
                        principalTable: "GameLevels",
                        principalColumn: "GameLevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    linkAvatar = table.Column<string>(type: "TEXT", nullable: true),
                    otp = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    regionId = table.Column<int>(type: "INTEGER", nullable: false),
                    roleId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameLevelId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Users_GameLevels_GameLevelId",
                        column: x => x.GameLevelId,
                        principalTable: "GameLevels",
                        principalColumn: "GameLevelId");
                    table.ForeignKey(
                        name: "FK_Users_Regions_regionId",
                        column: x => x.regionId,
                        principalTable: "Regions",
                        principalColumn: "regionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Roles_roleId",
                        column: x => x.roleId,
                        principalTable: "Roles",
                        principalColumn: "roleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LevelResults",
                columns: table => new
                {
                    LevelResultId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameLevelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Score = table.Column<int>(type: "INTEGER", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelResults", x => x.LevelResultId);
                    table.ForeignKey(
                        name: "FK_LevelResults_GameLevels_GameLevelId",
                        column: x => x.GameLevelId,
                        principalTable: "GameLevels",
                        principalColumn: "GameLevelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LevelResults_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GameLevels",
                columns: new[] { "GameLevelId", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Mức độ dễ", "Level 1" },
                    { 2, "Mức độ trung bình", "Level 2" },
                    { 3, "Mức độ khó", "Level 3" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "regionId", "Name" },
                values: new object[,]
                {
                    { 1, "Region1" },
                    { 2, "Region2" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "roleId", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "QuestionId", "Answer", "Content", "GameLevelId", "OptionA", "OptionB", "OptionC", "OptionD" },
                values: new object[,]
                {
                    { 1, "A", "Question 1 of Level 1?", 1, "A", "B", "C", "D" },
                    { 2, "B", "Question 2 of Level 1?", 1, "A", "B", "C", "D" },
                    { 3, "C", "Question 1 of Level 2?", 2, "A", "B", "C", "D" },
                    { 4, "D", "Question 2 of Level 2?", 2, "A", "B", "C", "D" },
                    { 5, "A", "Question 1 of Level 3?", 3, "A", "B", "C", "D" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "Email", "GameLevelId", "IsDeleted", "Password", "linkAvatar", "otp", "regionId", "roleId", "username" },
                values: new object[,]
                {
                    { 1, "admin@example.com", null, false, "", "avatar1.png", "123456", 1, 1, "user1" },
                    { 2, "user@example.com", null, false, "", "avatar2.png", "654321", 2, 2, "user2" }
                });

            migrationBuilder.InsertData(
                table: "LevelResults",
                columns: new[] { "LevelResultId", "CompletionDate", "GameLevelId", "Score", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 80, 1 },
                    { 2, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 60, 1 },
                    { 3, new DateTime(2025, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 90, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelResults_GameLevelId",
                table: "LevelResults",
                column: "GameLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_LevelResults_UserId",
                table: "LevelResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_GameLevelId",
                table: "Questions",
                column: "GameLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GameLevelId",
                table: "Users",
                column: "GameLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_regionId",
                table: "Users",
                column: "regionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_roleId",
                table: "Users",
                column: "roleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelResults");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "GameLevels");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
