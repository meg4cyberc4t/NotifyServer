using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyServer.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Firstname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Lastname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Color = table.Column<int>(type: "integer", nullable: false),
                    ForgeinUid = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifyUserNotifyUser",
                columns: table => new
                {
                    SubscribersId = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifyUserNotifyUser", x => new { x.SubscribersId, x.SubscriptionsId });
                    table.ForeignKey(
                        name: "FK_NotifyUserNotifyUser_Users_SubscribersId",
                        column: x => x.SubscribersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifyUserNotifyUser_Users_SubscriptionsId",
                        column: x => x.SubscriptionsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    RepeatMode = table.Column<int>(type: "integer", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    UniqueClaim = table.Column<int>(type: "integer", nullable: false),
                    NotifyFolderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Folders_NotifyFolderId",
                        column: x => x.NotifyFolderId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifyFolderNotifyUser",
                columns: table => new
                {
                    FoldersId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifyFolderNotifyUser", x => new { x.FoldersId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_NotifyFolderNotifyUser_Folders_FoldersId",
                        column: x => x.FoldersId,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifyFolderNotifyUser_Users_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotifyNotificationNotifyUser",
                columns: table => new
                {
                    NotificationsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifyNotificationNotifyUser", x => new { x.NotificationsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_NotifyNotificationNotifyUser_Notifications_NotificationsId",
                        column: x => x.NotificationsId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotifyNotificationNotifyUser_Users_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Folders_CreatorId",
                table: "Folders",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatorId",
                table: "Notifications",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotifyFolderId",
                table: "Notifications",
                column: "NotifyFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyFolderNotifyUser_ParticipantsId",
                table: "NotifyFolderNotifyUser",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyNotificationNotifyUser_ParticipantsId",
                table: "NotifyNotificationNotifyUser",
                column: "ParticipantsId");

            migrationBuilder.CreateIndex(
                name: "IX_NotifyUserNotifyUser_SubscriptionsId",
                table: "NotifyUserNotifyUser",
                column: "SubscriptionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotifyFolderNotifyUser");

            migrationBuilder.DropTable(
                name: "NotifyNotificationNotifyUser");

            migrationBuilder.DropTable(
                name: "NotifyUserNotifyUser");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
