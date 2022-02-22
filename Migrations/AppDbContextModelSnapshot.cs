﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NotifyServer.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NotifyServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NotifyFolderNotifyUser", b =>
                {
                    b.Property<Guid>("FoldersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParticipantsId")
                        .HasColumnType("uuid");

                    b.HasKey("FoldersId", "ParticipantsId");

                    b.HasIndex("ParticipantsId");

                    b.ToTable("NotifyFolderNotifyUser");
                });

            modelBuilder.Entity("NotifyNotificationNotifyUser", b =>
                {
                    b.Property<Guid>("NotificationsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParticipantsId")
                        .HasColumnType("uuid");

                    b.HasKey("NotificationsId", "ParticipantsId");

                    b.HasIndex("ParticipantsId");

                    b.ToTable("NotifyNotificationNotifyUser");
                });

            modelBuilder.Entity("NotifyServer.Models.NotifyFolder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("NotifyServer.Models.NotifyNotification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("NotifyFolderId")
                        .HasColumnType("uuid");

                    b.Property<int>("RepeatMode")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UniqueClaim")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("NotifyFolderId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("NotifyServer.Models.NotifyUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Color")
                        .HasColumnType("bigint");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ForgeinUid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NotifyUserNotifyUser", b =>
                {
                    b.Property<Guid>("SubscribersId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SubscriptionsId")
                        .HasColumnType("uuid");

                    b.HasKey("SubscribersId", "SubscriptionsId");

                    b.HasIndex("SubscriptionsId");

                    b.ToTable("NotifyUserNotifyUser");
                });

            modelBuilder.Entity("NotifyFolderNotifyUser", b =>
                {
                    b.HasOne("NotifyServer.Models.NotifyFolder", null)
                        .WithMany()
                        .HasForeignKey("FoldersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotifyServer.Models.NotifyUser", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotifyNotificationNotifyUser", b =>
                {
                    b.HasOne("NotifyServer.Models.NotifyNotification", null)
                        .WithMany()
                        .HasForeignKey("NotificationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotifyServer.Models.NotifyUser", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotifyServer.Models.NotifyFolder", b =>
                {
                    b.HasOne("NotifyServer.Models.NotifyUser", "Creator")
                        .WithMany("FolderWhereCreator")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("NotifyServer.Models.NotifyNotification", b =>
                {
                    b.HasOne("NotifyServer.Models.NotifyUser", "Creator")
                        .WithMany("NotificationsWhereCreator")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotifyServer.Models.NotifyFolder", null)
                        .WithMany("NotificationsList")
                        .HasForeignKey("NotifyFolderId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("NotifyUserNotifyUser", b =>
                {
                    b.HasOne("NotifyServer.Models.NotifyUser", null)
                        .WithMany()
                        .HasForeignKey("SubscribersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NotifyServer.Models.NotifyUser", null)
                        .WithMany()
                        .HasForeignKey("SubscriptionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NotifyServer.Models.NotifyFolder", b =>
                {
                    b.Navigation("NotificationsList");
                });

            modelBuilder.Entity("NotifyServer.Models.NotifyUser", b =>
                {
                    b.Navigation("FolderWhereCreator");

                    b.Navigation("NotificationsWhereCreator");
                });
#pragma warning restore 612, 618
        }
    }
}
