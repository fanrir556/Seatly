using System;
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

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

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

    public virtual DbSet<RestaurantOffer> RestaurantOffers { get; set; }

    public virtual DbSet<RestaurantTable> RestaurantTables { get; set; }

    public virtual DbSet<RestaurantTime> RestaurantTimes { get; set; }

    public virtual DbSet<WaitlistInfo> WaitlistInfos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\ProjectModels;Initial Catalog=Seatly;Integrated Security=true;TrustServerCertificate=true;");

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
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetUserRoles).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);
        });

        modelBuilder.Entity<BookingOrder>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.ActivityBarcode).HasMaxLength(6);
            entity.Property(e => e.ActivityId).HasColumnName("ActivityID");
            entity.Property(e => e.ActivityName).HasMaxLength(100);
            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .ValueGeneratedOnAdd()
                .HasColumnName("OrderID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<CollectionItem>(entity =>
        {
            entity.HasKey(e => e.SerialId).HasName("PK__Collecti__5E5B3EC4313A5301");

            entity.Property(e => e.SerialId)
                .ValueGeneratedNever()
                .HasColumnName("SerialID");
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAAA9C948B7");

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
            entity.HasKey(e => e.Id).HasName("PK__DailyChe__3214EC2784CC8202");

            entity.ToTable("DailyCheckIn");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.FlowId).HasName("PK__Friends__1184B35C4067DEEB");

            entity.Property(e => e.FlowId)
                .ValueGeneratedNever()
                .HasColumnName("FlowID");
            entity.Property(e => e.FriendUserName).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        modelBuilder.Entity<GamePoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GamePoin__3214EC2720D32651");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
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
            entity.HasNoKey();

            entity.Property(e => e.ActivityId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ActivityID");
            entity.Property(e => e.ActivityMethod).HasMaxLength(50);
            entity.Property(e => e.ActivityName).HasMaxLength(100);
            entity.Property(e => e.DescriptionN).HasMaxLength(1000);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.HashTag).HasMaxLength(255);
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");
            entity.Property(e => e.RecurringTime).HasMaxLength(50);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.HasKey(e => e.OrganizerId).HasName("PK__Restaura__87454CB580C1ADB7");

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
            entity.HasKey(e => e.ProductId).HasName("PK__PointSto__B40CC6EDFFC74A27");

            entity.ToTable("PointStore");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ProductDescription).HasMaxLength(500);
            entity.Property(e => e.ProductImage).HasMaxLength(200);
            entity.Property(e => e.ProductName).HasMaxLength(100);
        });

        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PointTra__3214EC278CB88AFA");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF85C161E2D2E");

            entity.Property(e => e.RatingId)
                .ValueGeneratedNever()
                .HasColumnName("RatingID");
            entity.Property(e => e.CommentTime).HasColumnType("datetime");
            entity.Property(e => e.MemberAccount).HasMaxLength(50);
            entity.Property(e => e.RestaurantAccount).HasMaxLength(50);
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Reply__C25E4629A63C3880");

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

        modelBuilder.Entity<RestaurantOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC27E9595DBE");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Photo).HasMaxLength(200);
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
        });

        modelBuilder.Entity<RestaurantTable>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__Restaura__7D5F018ED68A1E9D");

            entity.Property(e => e.TableId)
                .ValueGeneratedNever()
                .HasColumnName("TableID");
            entity.Property(e => e.PartitionName).HasMaxLength(50);
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
            entity.Property(e => e.Status).HasMaxLength(10);
            entity.Property(e => e.TableName).HasMaxLength(50);
        });

        modelBuilder.Entity<RestaurantTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC27813E92DC");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
        });

        modelBuilder.Entity<WaitlistInfo>(entity =>
        {
            entity.HasKey(e => e.WaitlistId).HasName("PK__Waitlist__FE78FE802312BE23");

            entity.ToTable("WaitlistInfo");

            entity.Property(e => e.WaitlistId)
                .ValueGeneratedNever()
                .HasColumnName("WaitlistID");
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
