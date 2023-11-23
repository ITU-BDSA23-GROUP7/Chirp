﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Chirp.EFCoreDatabase.Migrations
{
    [DbContext(typeof(ChirpDBContext))]
    [Migration("20231121133031_addFollowingFollowers")]
    partial class addFollowingFollowers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.14");

            modelBuilder.Entity("Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("AuthorAuthor", b =>
                {
                    b.Property<int>("FollowersAuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FollowingAuthorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("FollowersAuthorId", "FollowingAuthorId");

                    b.HasIndex("FollowingAuthorId");

                    b.ToTable("AuthorAuthor");
                });

            modelBuilder.Entity("Cheep", b =>
                {
                    b.Property<int>("CheepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("CheepId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Cheeps");
                });

            modelBuilder.Entity("AuthorAuthor", b =>
                {
                    b.HasOne("Author", null)
                        .WithMany()
                        .HasForeignKey("FollowersAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Author", null)
                        .WithMany()
                        .HasForeignKey("FollowingAuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Cheep", b =>
                {
                    b.HasOne("Author", "Author")
                        .WithMany("Cheeps")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Author", b =>
                {
                    b.Navigation("Cheeps");
                });
#pragma warning restore 612, 618
        }
    }
}
