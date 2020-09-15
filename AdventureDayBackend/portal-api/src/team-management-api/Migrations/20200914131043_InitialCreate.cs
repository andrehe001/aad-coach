using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace team_management_api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RunnerProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    RunnerStatus = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    CurrentPhase = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    PhaseConfigurations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunnerProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    GameEngineUri = table.Column<string>(nullable: true),
                    SubscriptionId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    TeamPassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    TeamId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeamLogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamId = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    ResponeTimeMs = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLogEntries", x => new { x.TeamId, x.Id });
                    table.ForeignKey(
                        name: "FK_TeamLogEntries_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamScores",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    Wins = table.Column<int>(nullable: false),
                    Loses = table.Column<int>(nullable: false),
                    Errors = table.Column<int>(nullable: false),
                    Income = table.Column<int>(nullable: false),
                    Costs = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamScores", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_TeamScores_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "RunnerProperties",
                columns: new[] { "Id", "CurrentPhase", "Name", "PhaseConfigurations", "RunnerStatus" },
                values: new object[] { 1, "Phase1_Deployment", "default", @"{
  ""Phase1_Deployment"": {
    ""RequestExecutorLatencyMillis"": 1000,
    ""PlayerTypes"": [
      0
    ]
  },
  ""Phase2_Change"": {
    ""RequestExecutorLatencyMillis"": 1000,
    ""PlayerTypes"": [
      0
    ]
  },
  ""Phase3_Monitoring"": {
    ""RequestExecutorLatencyMillis"": 1000,
    ""PlayerTypes"": [
      0
    ]
  },
  ""Phase4_Scale"": {
    ""RequestExecutorLatencyMillis"": 100,
    ""PlayerTypes"": [
      0
    ]
  },
  ""Phase5_Security"": {
    ""RequestExecutorLatencyMillis"": 500,
    ""PlayerTypes"": [
      0
    ]
  },
  ""Phase6_Intelligence"": {
    ""RequestExecutorLatencyMillis"": 500,
    ""PlayerTypes"": [
      1
    ]
  }
}", "Stopped" });

            migrationBuilder.CreateIndex(
                name: "IX_Members_TeamId",
                table: "Members",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamLogEntries_Timestamp",
                table: "TeamLogEntries",
                column: "Timestamp");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "RunnerProperties");

            migrationBuilder.DropTable(
                name: "TeamLogEntries");

            migrationBuilder.DropTable(
                name: "TeamScores");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
