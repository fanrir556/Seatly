﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Seatly1.Models;

public partial class SeatlyContext : DbContext
{
    public SeatlyContext()
    {
    }

    public SeatlyContext(DbContextOptions<SeatlyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<BookingOrder> BookingOrders { get; set; }

    public virtual DbSet<CollectionItem> CollectionItems { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DailyCheckIn> DailyCheckIns { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<GamePoint> GamePoints { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<NotificationRecord> NotificationRecords { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<PointStore> PointStores { get; set; }

    public virtual DbSet<PointTransaction> PointTransactions { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=Seatly;TrustServerCertificate=True;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.MemberNickname).HasMaxLength(10);
            entity.Property(e => e.MemberRealName).HasMaxLength(20);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.Sex).HasMaxLength(1);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<BookingOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__BookingO__C3905BAF4F8536F1");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ActivityBarcode).HasMaxLength(6);
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ActivityName).HasMaxLength(100);
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<CollectionItem>(entity =>
        {
            entity.HasKey(e => e.SerialId).HasName("PK__Collecti__5E5B3EC446FF6BE9");

            entity.Property(e => e.SerialId).HasColumnName("SerialID");
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAABE080226");

            entity.Property(e => e.CommentId)
                .ValueGeneratedNever()
                .HasColumnName("CommentID");
            entity.Property(e => e.MemberAccount).HasMaxLength(50);
            entity.Property(e => e.ReContent)
                .HasMaxLength(1000)
                .HasColumnName("reContent");
            entity.Property(e => e.RestaurantAccount).HasMaxLength(50);
        });

        modelBuilder.Entity<DailyCheckIn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DailyChe__3214EC27AFB6E39E");

            entity.ToTable("DailyCheckIn");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId)
                .HasMaxLength(450)
                .HasColumnName("MemberID");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.FlowId);

            entity.Property(e => e.FlowId).HasColumnName("FlowID");
            entity.Property(e => e.FriendUserId)
                .HasMaxLength(50)
                .HasColumnName("FriendUserID");
            entity.Property(e => e.FriendUserName).HasMaxLength(50);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<GamePoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GamePoin__3214EC27C4ACC4AB");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId)
                .HasMaxLength(450)
                .HasColumnName("MemberID");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.MemberId).HasName("PK__Members__0CF04B38AC717DEA");

            entity.Property(e => e.MemberId)
                .ValueGeneratedNever()
                .HasColumnName("MemberID");
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.MemberAccount).HasMaxLength(20);
            entity.Property(e => e.MemberName).HasMaxLength(30);
            entity.Property(e => e.MemberNickname).HasMaxLength(10);
            entity.Property(e => e.MemberPassword).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(16);
            entity.Property(e => e.Sex).HasMaxLength(1);
        });

        modelBuilder.Entity<NotificationRecord>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Notifica__45F4A7F13E52F071");

            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ActivityMethod).HasMaxLength(5);
            entity.Property(e => e.ActivityName).HasMaxLength(100);
            entity.Property(e => e.DescriptionN).HasMaxLength(1000);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.HashTag1).HasMaxLength(255);
            entity.Property(e => e.HashTag2).HasMaxLength(255);
            entity.Property(e => e.HashTag3).HasMaxLength(255);
            entity.Property(e => e.HashTag4).HasMaxLength(255);
            entity.Property(e => e.HashTag5).HasMaxLength(255);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");
            entity.Property(e => e.RecurringTime).HasMaxLength(50);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Hashtag).HasMaxLength(100);
            entity.Property(e => e.LoginPassword).HasMaxLength(50);
            entity.Property(e => e.Menu).HasMaxLength(100);
            entity.Property(e => e.OrganizerAccount).HasMaxLength(50);
            entity.Property(e => e.OrganizerCategory).HasMaxLength(50);
            entity.Property(e => e.OrganizerName).HasMaxLength(100);
            entity.Property(e => e.OrganizerPhoto).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.ReservationUrl)
                .HasMaxLength(200)
                .HasColumnName("ReservationURL");
        });

        modelBuilder.Entity<PointStore>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__PointSto__B40CC6ED9AE755B5");

            entity.ToTable("PointStore");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ProductDescription).HasMaxLength(500);
            entity.Property(e => e.ProductImage).HasMaxLength(200);
            entity.Property(e => e.ProductName).HasMaxLength(100);
        });

        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PointTra__3214EC272AA20793");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId)
                .HasMaxLength(450)
                .HasColumnName("MemberID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF85C69CC4A10");

            entity.Property(e => e.RatingId)
                .ValueGeneratedNever()
                .HasColumnName("RatingID");
            entity.Property(e => e.CommentTime).HasColumnType("datetime");
            entity.Property(e => e.MemberAccount).HasMaxLength(50);
            entity.Property(e => e.RestaurantAccount).HasMaxLength(50);
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Reply__C25E46295613AB40");

            entity.ToTable("Reply");

            entity.Property(e => e.ReplyId)
                .ValueGeneratedNever()
                .HasColumnName("ReplyID");
            entity.Property(e => e.ReContent)
                .HasMaxLength(50)
                .HasColumnName("reContent");
            entity.Property(e => e.ReplyAccount).HasMaxLength(50);
            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
