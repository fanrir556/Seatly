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

    public virtual DbSet<BookingOrder> BookingOrders { get; set; }

    public virtual DbSet<CollectionItem> CollectionItems { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<DailyCheckIn> DailyCheckIns { get; set; }

    public virtual DbSet<GamePoint> GamePoints { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<NotificationRecord> NotificationRecords { get; set; }

    public virtual DbSet<PointStore> PointStores { get; set; }

    public virtual DbSet<PointTransaction> PointTransactions { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<RestaurantOffer> RestaurantOffers { get; set; }

    public virtual DbSet<RestaurantTable> RestaurantTables { get; set; }

    public virtual DbSet<RestaurantTime> RestaurantTimes { get; set; }

    public virtual DbSet<WaitlistInfo> WaitlistInfos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Data Source=T770;Initial Catalog=Seatly;TrustServerCertificate=True;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookingOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__BookingO__C3905BAF39920861");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("OrderID");
            entity.Property(e => e.ContactInfo).HasMaxLength(100);
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.WaitingName).HasMaxLength(100);
        });

        modelBuilder.Entity<CollectionItem>(entity =>
        {
            entity.HasKey(e => e.SerialId).HasName("PK__Collecti__5E5B3EC498D4885F");

            entity.Property(e => e.SerialId)
                .ValueGeneratedNever()
                .HasColumnName("SerialID");
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAA3F6B824D");

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
            entity.HasKey(e => e.Id).HasName("PK__DailyChe__3214EC27AE741CBC");

            entity.ToTable("DailyCheckIn");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
        });

        modelBuilder.Entity<GamePoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GamePoin__3214EC27AFEAD1E6");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
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
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E323860F8AF");

            entity.Property(e => e.NotificationId)
                .ValueGeneratedNever()
                .HasColumnName("NotificationID");
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.MessageContent).HasMaxLength(500);
            entity.Property(e => e.NotificationContent).HasMaxLength(500);
            entity.Property(e => e.NotificationStatus).HasMaxLength(50);
            entity.Property(e => e.NotificationTimestamp).HasColumnType("datetime");
            entity.Property(e => e.NotificationType).HasMaxLength(50);
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.Order).WithMany(p => p.NotificationRecords)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__Notificat__Order__4E88ABD4");
        });

        modelBuilder.Entity<PointStore>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__PointSto__B40CC6ED990765CC");

            entity.ToTable("PointStore");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.ProductDescription).HasMaxLength(500);
            entity.Property(e => e.ProductImage).HasMaxLength(200);
            entity.Property(e => e.ProductName).HasMaxLength(100);
        });

        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PointTra__3214EC27180FCD49");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MemberId).HasColumnName("MemberID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.RatingId).HasName("PK__Ratings__FCCDF85CD16A3093");

            entity.Property(e => e.RatingId)
                .ValueGeneratedNever()
                .HasColumnName("RatingID");
            entity.Property(e => e.CommentTime).HasColumnType("datetime");
            entity.Property(e => e.MemberAccount).HasMaxLength(50);
            entity.Property(e => e.RestaurantAccount).HasMaxLength(50);
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Reply__C25E46293EA50A7C");

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

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.HasKey(e => e.RestaurantId).HasName("PK__Restaura__87454CB580C1ADB7");

            entity.Property(e => e.RestaurantId)
                .ValueGeneratedNever()
                .HasColumnName("RestaurantID");
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.DepartmentStoreName).HasMaxLength(100);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Hashtag).HasMaxLength(100);
            entity.Property(e => e.LoginPassword).HasMaxLength(50);
            entity.Property(e => e.MenuPhoto).HasMaxLength(100);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.ReservationUrl)
                .HasMaxLength(200)
                .HasColumnName("ReservationURL");
            entity.Property(e => e.RestaurantAccount).HasMaxLength(50);
            entity.Property(e => e.RestaurantCategory).HasMaxLength(50);
            entity.Property(e => e.RestaurantInfo).HasMaxLength(100);
            entity.Property(e => e.RestaurantName).HasMaxLength(100);
            entity.Property(e => e.RestaurantPhoto).HasMaxLength(100);
        });

        modelBuilder.Entity<RestaurantOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC27D749CD0D");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Photo).HasMaxLength(200);
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
        });

        modelBuilder.Entity<RestaurantTable>(entity =>
        {
            entity.HasKey(e => e.TableId).HasName("PK__Restaura__7D5F018E7CF660C3");

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
            entity.HasKey(e => e.Id).HasName("PK__Restaura__3214EC27528C8260");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.RestaurantId).HasColumnName("RestaurantID");
        });

        modelBuilder.Entity<WaitlistInfo>(entity =>
        {
            entity.HasKey(e => e.WaitlistId).HasName("PK__Waitlist__FE78FE8022C11AF5");

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
