﻿// <auto-generated />
using System;
using Kyoo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kyoo.Models.DatabaseMigrations
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2");

            modelBuilder.Entity("Kyoo.Models.Account", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT");

                    b.Property<string>("OTAC")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("OTACExpires")
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Kyoo.Models.Collection", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImgPrimary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Overview")
                        .HasColumnType("TEXT");

                    b.Property<string>("Poster")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Kyoo.Models.CollectionLink", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CollectionID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ShowID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CollectionID");

                    b.HasIndex("ShowID");

                    b.ToTable("CollectionLinks");
                });

            modelBuilder.Entity("Kyoo.Models.Episode", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("AbsoluteNumber")
                        .HasColumnType("INTEGER");

                    b.Property<long>("EpisodeNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalIDs")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgPrimary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Overview")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("Runtime")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("SeasonID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ShowID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("SeasonID");

                    b.HasIndex("ShowID");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("Kyoo.Models.Genre", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Kyoo.Models.GenreLink", b =>
                {
                    b.Property<long>("ShowID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("GenreID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ShowID", "GenreID");

                    b.HasIndex("GenreID");

                    b.ToTable("GenreLinks");
                });

            modelBuilder.Entity("Kyoo.Models.Library", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Paths")
                        .HasColumnType("TEXT");

                    b.Property<string>("Providers")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Libraries");
                });

            modelBuilder.Entity("Kyoo.Models.LibraryLink", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CollectionID")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LibraryID")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ShowID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CollectionID");

                    b.HasIndex("LibraryID");

                    b.HasIndex("ShowID");

                    b.ToTable("LibraryLinks");
                });

            modelBuilder.Entity("Kyoo.Models.People", b =>
                {
                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExternalIDs")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgPrimary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Slug");

                    b.ToTable("Peoples");
                });

            modelBuilder.Entity("Kyoo.Models.PeopleLink", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PeopleID")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<long>("ShowID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("PeopleID");

                    b.HasIndex("ShowID");

                    b.ToTable("PeopleLinks");
                });

            modelBuilder.Entity("Kyoo.Models.Season", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalIDs")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgPrimary")
                        .HasColumnType("TEXT");

                    b.Property<string>("Overview")
                        .HasColumnType("TEXT");

                    b.Property<long>("SeasonNumber")
                        .HasColumnType("INTEGER");

                    b.Property<long>("ShowID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<long?>("Year")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("ShowID");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("Kyoo.Models.Show", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Aliases")
                        .HasColumnType("TEXT");

                    b.Property<long?>("EndYear")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ExternalIDs")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgBackdrop")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgLogo")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgPrimary")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImgThumb")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsMovie")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Overview")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.Property<long?>("StartYear")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("StudioID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<string>("TrailerUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("StudioID");

                    b.ToTable("Shows");
                });

            modelBuilder.Entity("Kyoo.Models.Studio", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Studios");
                });

            modelBuilder.Entity("Kyoo.Models.Track", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Codec")
                        .HasColumnType("TEXT");

                    b.Property<long>("EpisodeID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsExternal")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsForced")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Language")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("EpisodeID");

                    b.ToTable("Tracks");
                });

            modelBuilder.Entity("Kyoo.Models.CollectionLink", b =>
                {
                    b.HasOne("Kyoo.Models.Collection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionID");

                    b.HasOne("Kyoo.Models.Show", "Show")
                        .WithMany()
                        .HasForeignKey("ShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kyoo.Models.Episode", b =>
                {
                    b.HasOne("Kyoo.Models.Season", "Season")
                        .WithMany("Episodes")
                        .HasForeignKey("SeasonID");

                    b.HasOne("Kyoo.Models.Show", "Show")
                        .WithMany("Episodes")
                        .HasForeignKey("ShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kyoo.Models.GenreLink", b =>
                {
                    b.HasOne("Kyoo.Models.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kyoo.Models.Show", "Show")
                        .WithMany("GenreLinks")
                        .HasForeignKey("ShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kyoo.Models.LibraryLink", b =>
                {
                    b.HasOne("Kyoo.Models.Collection", "Collection")
                        .WithMany()
                        .HasForeignKey("CollectionID");

                    b.HasOne("Kyoo.Models.Library", "Library")
                        .WithMany()
                        .HasForeignKey("LibraryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kyoo.Models.Show", "Show")
                        .WithMany()
                        .HasForeignKey("ShowID");
                });

            modelBuilder.Entity("Kyoo.Models.PeopleLink", b =>
                {
                    b.HasOne("Kyoo.Models.People", "People")
                        .WithMany("Roles")
                        .HasForeignKey("PeopleID");

                    b.HasOne("Kyoo.Models.Show", "Show")
                        .WithMany("People")
                        .HasForeignKey("ShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kyoo.Models.Season", b =>
                {
                    b.HasOne("Kyoo.Models.Show", "Show")
                        .WithMany("Seasons")
                        .HasForeignKey("ShowID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kyoo.Models.Show", b =>
                {
                    b.HasOne("Kyoo.Models.Studio", "Studio")
                        .WithMany()
                        .HasForeignKey("StudioID");
                });

            modelBuilder.Entity("Kyoo.Models.Track", b =>
                {
                    b.HasOne("Kyoo.Models.Episode", "Episode")
                        .WithMany("Tracks")
                        .HasForeignKey("EpisodeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
