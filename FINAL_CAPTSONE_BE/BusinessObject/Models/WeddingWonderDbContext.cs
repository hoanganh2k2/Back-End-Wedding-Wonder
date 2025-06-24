using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BusinessObject.Models;

public partial class WeddingWonderDbContext : DbContext
{
    public WeddingWonderDbContext()
    {
    }

    public WeddingWonderDbContext(DbContextOptions<WeddingWonderDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminLog> AdminLogs { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BookingServiceDetail> BookingServiceDetails { get; set; }

    public virtual DbSet<BookingTransactionHistory> BookingTransactionHistories { get; set; }

    public virtual DbSet<BusySchedule> BusySchedules { get; set; }

    public virtual DbSet<Catering> Caterings { get; set; }

    public virtual DbSet<ClothesService> ClothesServices { get; set; }

    public virtual DbSet<ComboBooking> ComboBookings { get; set; }

    public virtual DbSet<Contract> Contracts { get; set; }

    public virtual DbSet<ContractService> ContractServices { get; set; }

    public virtual DbSet<Contractbooking> Contractbookings { get; set; }

    public virtual DbSet<CustomerReview> CustomerReviews { get; set; }

    public virtual DbSet<EventConcept> EventConcepts { get; set; }

    public virtual DbSet<EventOrganizerService> EventOrganizerServices { get; set; }

    public virtual DbSet<EventPackage> EventPackages { get; set; }

    public virtual DbSet<FavoriteService> FavoriteServices { get; set; }

    public virtual DbSet<InforAdmin> InforAdmins { get; set; }

    public virtual DbSet<InforBooking> InforBookings { get; set; }

    public virtual DbSet<InvitationImage> InvitationImages { get; set; }

    public virtual DbSet<InvitationPackage> InvitationPackages { get; set; }

    public virtual DbSet<InvitationService> InvitationServices { get; set; }

    public virtual DbSet<MakeUpArtist> MakeUpArtists { get; set; }

    public virtual DbSet<MakeUpPackage> MakeUpPackages { get; set; }

    public virtual DbSet<MakeUpService> MakeUpServices { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Outfit> Outfits { get; set; }

    public virtual DbSet<OutfitImage> OutfitImages { get; set; }

    public virtual DbSet<OutfitOutfitType> OutfitOutfitTypes { get; set; }

    public virtual DbSet<OutfitType> OutfitTypes { get; set; }

    public virtual DbSet<PhotographPackage> PhotographPackages { get; set; }

    public virtual DbSet<PhotographService> PhotographServices { get; set; }

    public virtual DbSet<Photographer> Photographers { get; set; }

    public virtual DbSet<RequestImage> RequestImages { get; set; }

    public virtual DbSet<RequestUpgradeSupplier> RequestUpgradeSuppliers { get; set; }

    public virtual DbSet<RestaurantService> RestaurantServices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceImage> ServiceImages { get; set; }

    public virtual DbSet<ServiceTopic> ServiceTopics { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<SingleBooking> SingleBookings { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTopic> UserTopics { get; set; }

    public virtual DbSet<VoucherAdmin> VoucherAdmins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=WeddingWonderDB;uid=root;pwd=hoanganh1905", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.0.1-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AdminLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("admin_log");

            entity.HasIndex(e => e.AdminId, "Admin_Id");

            entity.Property(e => e.LogId).HasColumnName("Log_Id");
            entity.Property(e => e.ActionType)
                .HasMaxLength(50)
                .HasColumnName("Action_Type");
            entity.Property(e => e.AdminId).HasColumnName("Admin_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.DeviceType)
                .HasMaxLength(50)
                .HasColumnName("Device_Type");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .HasColumnName("Ip_Address");
            entity.Property(e => e.LogDetail)
                .HasColumnType("text")
                .HasColumnName("Log_Detail");

            entity.HasOne(d => d.Admin).WithMany(p => p.AdminLogs)
                .HasForeignKey(d => d.AdminId)
                .HasConstraintName("admin_log_ibfk_1");
        });

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("blog");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasMany(d => d.Tags).WithMany(p => p.Blogs)
                .UsingEntity<Dictionary<string, object>>(
                    "Blogtag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("blogtag_ibfk_2"),
                    l => l.HasOne<Blog>().WithMany()
                        .HasForeignKey("BlogId")
                        .HasConstraintName("blogtag_ibfk_1"),
                    j =>
                    {
                        j.HasKey("BlogId", "TagId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("blogtag");
                        j.HasIndex(new[] { "TagId" }, "TagId");
                    });
        });

        modelBuilder.Entity<BookingServiceDetail>(entity =>
        {
            entity.HasKey(e => e.DetailId).HasName("PRIMARY");

            entity.ToTable("booking_service_detail");

            entity.HasIndex(e => e.ArtistId, "Artist_Id");

            entity.HasIndex(e => e.BookingId, "Booking_Id");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.HasIndex(e => e.ServiceTypeId, "Service_Type_Id");

            entity.Property(e => e.DetailId).HasColumnName("Detail_Id");
            entity.Property(e => e.Appointment).HasColumnType("datetime");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_Id");
            entity.Property(e => e.BasicPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Basic_Price");
            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.NumberOfUses).HasColumnName("Number_Of_Uses");
            entity.Property(e => e.PackageId).HasColumnName("Package_Id");
            entity.Property(e => e.PreAppointment)
                .HasColumnType("datetime")
                .HasColumnName("Pre_Appointment");
            entity.Property(e => e.PrePackageId).HasColumnName("Pre_Package_Id");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.ServiceMode).HasColumnName("Service_Mode");
            entity.Property(e => e.ServiceTypeId).HasColumnName("Service_Type_Id");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Total_Price");

            entity.HasOne(d => d.Artist).WithMany(p => p.BookingServiceDetails)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("booking_service_detail_ibfk_4");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingServiceDetails)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_service_detail_ibfk_3");

            entity.HasOne(d => d.Service).WithMany(p => p.BookingServiceDetails)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_service_detail_ibfk_1");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.BookingServiceDetails)
                .HasForeignKey(d => d.ServiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_service_detail_ibfk_2");
        });

        modelBuilder.Entity<BookingTransactionHistory>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.ToTable("booking_transaction_history");

            entity.HasIndex(e => e.BookingId, "Booking_Id");

            entity.Property(e => e.TransactionId).HasColumnName("Transaction_Id");
            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.NumberPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Number_Price");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("Transaction_Date");
            entity.Property(e => e.TransactionType)
                .HasColumnType("text")
                .HasColumnName("Transaction_Type");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingTransactionHistories)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("booking_transaction_history_ibfk_1");
        });

        modelBuilder.Entity<BusySchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PRIMARY");

            entity.ToTable("busy_schedule");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.ScheduleId).HasColumnName("Schedule_Id");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");

            entity.HasOne(d => d.Service).WithMany(p => p.BusySchedules)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("busy_schedule_ibfk_1");
        });

        modelBuilder.Entity<Catering>(entity =>
        {
            entity.HasKey(e => e.CateringId).HasName("PRIMARY");

            entity.ToTable("catering");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.CateringId).HasColumnName("Catering_Id");
            entity.Property(e => e.CateringImage)
                .HasColumnType("text")
                .HasColumnName("Catering_Image");
            entity.Property(e => e.PackageContent)
                .HasColumnType("text")
                .HasColumnName("Package_Content");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.StyleName)
                .HasMaxLength(255)
                .HasColumnName("Style_Name");

            entity.HasOne(d => d.Service).WithMany(p => p.Caterings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("catering_ibfk_1");
        });

        modelBuilder.Entity<ClothesService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("clothes_service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithOne(p => p.ClothesService)
                .HasForeignKey<ClothesService>(d => d.ServiceId)
                .HasConstraintName("clothes_service_ibfk_1");
        });

        modelBuilder.Entity<ComboBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PRIMARY");

            entity.ToTable("combo_booking");

            entity.HasIndex(e => e.ContractId, "Contract_Id");

            entity.HasIndex(e => e.InforBrideId, "Infor_Bride_Id");

            entity.HasIndex(e => e.InforGroomId, "Infor_Groom_Id");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.HasIndex(e => e.VoucherId, "Voucher_Id");

            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.BasicPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Basic_Price");
            entity.Property(e => e.BookingStatus).HasColumnName("Booking_Status");
            entity.Property(e => e.Budget).HasPrecision(18, 2);
            entity.Property(e => e.ContractId).HasColumnName("Contract_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.DepositDate).HasColumnType("datetime");
            entity.Property(e => e.ExpectedWeddingDate)
                .HasColumnType("datetime")
                .HasColumnName("Expected_Wedding_Date");
            entity.Property(e => e.InforBrideId).HasColumnName("Infor_Bride_Id");
            entity.Property(e => e.InforGroomId).HasColumnName("Infor_Groom_Id");
            entity.Property(e => e.MinRequiredDeposit)
                .HasPrecision(18, 2)
                .HasColumnName("Min_Required_Deposit");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Total_Price");
            entity.Property(e => e.TypeCombo).HasColumnName("Type_Combo");
            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.VoucherId).HasColumnName("Voucher_Id");

            entity.HasOne(d => d.Contract).WithMany(p => p.ComboBookings)
                .HasForeignKey(d => d.ContractId)
                .HasConstraintName("combo_booking_ibfk_3");

            entity.HasOne(d => d.InforBride).WithMany(p => p.ComboBookingInforBrides)
                .HasForeignKey(d => d.InforBrideId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("combo_booking_ibfk_4");

            entity.HasOne(d => d.InforGroom).WithMany(p => p.ComboBookingInforGrooms)
                .HasForeignKey(d => d.InforGroomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("combo_booking_ibfk_5");

            entity.HasOne(d => d.User).WithMany(p => p.ComboBookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("combo_booking_ibfk_2");

            entity.HasOne(d => d.Voucher).WithMany(p => p.ComboBookings)
                .HasForeignKey(d => d.VoucherId)
                .HasConstraintName("combo_booking_ibfk_1");
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.ContractId).HasName("PRIMARY");

            entity.ToTable("contract");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.Property(e => e.ContractId).HasColumnName("Contract_Id");
            entity.Property(e => e.BackIdCardPath).HasMaxLength(255);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.FrontIdCardPath).HasMaxLength(255);
            entity.Property(e => e.IsConfirmed).HasDefaultValueSql("'0'");
            entity.Property(e => e.Otp).HasMaxLength(6);
            entity.Property(e => e.PdfFilePath).HasMaxLength(255);
            entity.Property(e => e.SignedDate).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Contracts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("contract_ibfk_1");
        });

        modelBuilder.Entity<ContractService>(entity =>
        {
            entity.HasKey(e => new { e.ContractId, e.ServiceId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("contract_service");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.ContractId).HasColumnName("Contract_Id");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.Content).HasColumnType("text");

            entity.HasOne(d => d.Contract).WithMany(p => p.ContractServices)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_service_ibfk_1");

            entity.HasOne(d => d.Service).WithMany(p => p.ContractServices)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("contract_service_ibfk_2");
        });

        modelBuilder.Entity<Contractbooking>(entity =>
        {
            entity.HasKey(e => e.ContractBookingId).HasName("PRIMARY");

            entity.ToTable("contractbooking");

            entity.HasIndex(e => e.BookingId, "Booking_Id");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.Property(e => e.ContractBookingId).HasColumnName("ContractBooking_Id");
            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount).HasPrecision(18, 2);
            entity.Property(e => e.IsSigned).HasDefaultValueSql("'0'");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.Signature).HasColumnType("text");
            entity.Property(e => e.Subtotal).HasPrecision(18, 2);
            entity.Property(e => e.Total).HasPrecision(18, 2);
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Booking).WithMany(p => p.Contractbookings)
                .HasForeignKey(d => d.BookingId)
                .HasConstraintName("contractbooking_ibfk_1");

            entity.HasOne(d => d.Service).WithMany(p => p.Contractbookings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("contractbooking_ibfk_3");

            entity.HasOne(d => d.User).WithMany(p => p.Contractbookings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("contractbooking_ibfk_2");
        });

        modelBuilder.Entity<CustomerReview>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.ToTable("customer_review");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.Property(e => e.ReviewId).HasColumnName("Review_Id");
            entity.Property(e => e.CanEdit).HasColumnName("Can_Edit");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Reply).HasColumnType("text");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.StarNumber).HasColumnName("Star_Number");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Service).WithMany(p => p.CustomerReviews)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customer_review_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerReviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("customer_review_ibfk_2");
        });

        modelBuilder.Entity<EventConcept>(entity =>
        {
            entity.HasKey(e => e.ConceptId).HasName("PRIMARY");

            entity.ToTable("event_concept");

            entity.HasIndex(e => e.PackageId, "Package_Id");

            entity.Property(e => e.ConceptId).HasColumnName("Concept_Id");
            entity.Property(e => e.ConceptName)
                .HasMaxLength(255)
                .HasColumnName("Concept_Name");
            entity.Property(e => e.PackageId).HasColumnName("Package_Id");

            entity.HasOne(d => d.Package).WithMany(p => p.EventConcepts)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("event_concept_ibfk_1");
        });

        modelBuilder.Entity<EventOrganizerService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("event_organizer_service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithOne(p => p.EventOrganizerService)
                .HasForeignKey<EventOrganizerService>(d => d.ServiceId)
                .HasConstraintName("event_organizer_service_ibfk_1");
        });

        modelBuilder.Entity<EventPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PRIMARY");

            entity.ToTable("event_package");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.PackageId).HasColumnName("Package_Id");
            entity.Property(e => e.EventType).HasColumnName("Event_Type");
            entity.Property(e => e.PackageContent)
                .HasColumnType("text")
                .HasColumnName("Package_Content");
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .HasColumnName("Package_Name");
            entity.Property(e => e.PackagePrice)
                .HasPrecision(18, 2)
                .HasColumnName("Package_Price");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithMany(p => p.EventPackages)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("event_package_ibfk_1");
        });

        modelBuilder.Entity<FavoriteService>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PRIMARY");

            entity.ToTable("favorite_services");

            entity.HasIndex(e => e.ServiceId, "idx_favorites_service");

            entity.HasIndex(e => e.UserId, "idx_favorites_user");

            entity.Property(e => e.FavoriteId).HasColumnName("Favorite_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Service).WithMany(p => p.FavoriteServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("favorite_services_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteServices)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("favorite_services_ibfk_1");
        });

        modelBuilder.Entity<InforAdmin>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("infor_admin");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("Phone_Number");
        });

        modelBuilder.Entity<InforBooking>(entity =>
        {
            entity.HasKey(e => e.InforId).HasName("PRIMARY");

            entity.ToTable("infor_booking");

            entity.Property(e => e.InforId).HasColumnName("Infor_Id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("datetime")
                .HasColumnName("Date_Of_Birth");
            entity.Property(e => e.District).HasMaxLength(255);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("Full_Name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.Ward).HasMaxLength(255);
        });

        modelBuilder.Entity<InvitationImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("invitation_image");

            entity.HasIndex(e => e.PackageId, "Package_Id");

            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.ImageText)
                .HasColumnType("text")
                .HasColumnName("Image_Text");
            entity.Property(e => e.PackageId).HasColumnName("Package_Id");

            entity.HasOne(d => d.Package).WithMany(p => p.InvitationImages)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("invitation_image_ibfk_1");
        });

        modelBuilder.Entity<InvitationPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PRIMARY");

            entity.ToTable("invitation_package");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.PackageId).HasColumnName("Package_Id");
            entity.Property(e => e.Component).HasMaxLength(255);
            entity.Property(e => e.Envelope).HasMaxLength(255);
            entity.Property(e => e.PackageDescribe)
                .HasColumnType("text")
                .HasColumnName("Package_Describe");
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .HasColumnName("Package_Name");
            entity.Property(e => e.PackagePrice)
                .HasPrecision(18, 2)
                .HasColumnName("Package_Price");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.Size).HasMaxLength(255);

            entity.HasOne(d => d.Service).WithMany(p => p.InvitationPackages)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("invitation_package_ibfk_1");
        });

        modelBuilder.Entity<InvitationService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("invitation_service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithOne(p => p.InvitationService)
                .HasForeignKey<InvitationService>(d => d.ServiceId)
                .HasConstraintName("invitation_service_ibfk_1");
        });

        modelBuilder.Entity<MakeUpArtist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("PRIMARY");

            entity.ToTable("make_up_artists");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.ArtistId).HasColumnName("Artist_Id");
            entity.Property(e => e.ArtistImage)
                .HasColumnType("text")
                .HasColumnName("Artist_Image");
            entity.Property(e => e.ArtistName)
                .HasMaxLength(255)
                .HasColumnName("Artist_Name");
            entity.Property(e => e.Awards).HasColumnType("text");
            entity.Property(e => e.Certifications).HasColumnType("text");
            entity.Property(e => e.ProfessionalFee)
                .HasPrecision(5, 2)
                .HasColumnName("Professional_Fee");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.Services).HasColumnType("text");

            entity.HasOne(d => d.Service).WithMany(p => p.MakeUpArtists)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("make_up_artists_ibfk_1");
        });

        modelBuilder.Entity<MakeUpPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PRIMARY");

            entity.ToTable("make_up_package");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.PackageId).HasColumnName("Package_Id");
            entity.Property(e => e.EventType).HasColumnName("Event_Type");
            entity.Property(e => e.PackageContent)
                .HasColumnType("text")
                .HasColumnName("Package_Content");
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .HasColumnName("Package_Name");
            entity.Property(e => e.PackagePrice)
                .HasPrecision(18, 2)
                .HasColumnName("Package_Price");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithMany(p => p.MakeUpPackages)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("make_up_package_ibfk_1");
        });

        modelBuilder.Entity<MakeUpService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("make_up_service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithOne(p => p.MakeUpService)
                .HasForeignKey<MakeUpService>(d => d.ServiceId)
                .HasConstraintName("make_up_service_ibfk_1");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PRIMARY");

            entity.ToTable("menu");

            entity.HasIndex(e => e.CateringId, "Catering_Id");

            entity.Property(e => e.MenuId).HasColumnName("Menu_Id");
            entity.Property(e => e.CateringId).HasColumnName("Catering_Id");
            entity.Property(e => e.MenuContent)
                .HasColumnType("text")
                .HasColumnName("Menu_Content");
            entity.Property(e => e.MenuName)
                .HasMaxLength(255)
                .HasColumnName("Menu_Name");
            entity.Property(e => e.MenuType).HasColumnName("Menu_Type");
            entity.Property(e => e.Price).HasPrecision(18, 2);

            entity.HasOne(d => d.Catering).WithMany(p => p.Menus)
                .HasForeignKey(d => d.CateringId)
                .HasConstraintName("menu_ibfk_1");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PRIMARY");

            entity.ToTable("message");

            entity.HasIndex(e => e.ReceiverId, "Receiver_Id");

            entity.HasIndex(e => e.SenderId, "Sender_Id");

            entity.Property(e => e.MessageId).HasColumnName("Message_Id");
            entity.Property(e => e.AttachmentUrl)
                .HasMaxLength(255)
                .HasColumnName("AttachmentURL");
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.ReceiverId).HasColumnName("Receiver_Id");
            entity.Property(e => e.SendDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Send_Date");
            entity.Property(e => e.SenderId).HasColumnName("Sender_Id");
            entity.Property(e => e.Type).HasMaxLength(10);

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("message_ibfk_2");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("message_ibfk_1");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notification");

            entity.HasIndex(e => e.IsRead, "idx_notification_isread");

            entity.HasIndex(e => e.ReceiverId, "idx_notification_receiver");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.IsRead)
                .HasDefaultValueSql("'0'")
                .HasColumnName("Is_Read");
            entity.Property(e => e.ReceiverId).HasColumnName("Receiver_Id");

            entity.HasOne(d => d.Receiver).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.ReceiverId)
                .HasConstraintName("notification_ibfk_1");
        });

        modelBuilder.Entity<Outfit>(entity =>
        {
            entity.HasKey(e => e.OutfitId).HasName("PRIMARY");

            entity.ToTable("outfit");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.OutfitId).HasColumnName("Outfit_Id");
            entity.Property(e => e.OutfitDescription)
                .HasColumnType("text")
                .HasColumnName("Outfit_Description");
            entity.Property(e => e.OutfitName)
                .HasMaxLength(255)
                .HasColumnName("Outfit_Name");
            entity.Property(e => e.OutfitPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Outfit_Price");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithMany(p => p.Outfits)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("outfit_ibfk_1");
        });

        modelBuilder.Entity<OutfitImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("outfit_image");

            entity.HasIndex(e => e.OutfitId, "Outfit_Id");

            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.ImageText)
                .HasColumnType("text")
                .HasColumnName("Image_Text");
            entity.Property(e => e.OutfitId).HasColumnName("Outfit_Id");

            entity.HasOne(d => d.Outfit).WithMany(p => p.OutfitImages)
                .HasForeignKey(d => d.OutfitId)
                .HasConstraintName("outfit_image_ibfk_1");
        });

        modelBuilder.Entity<OutfitOutfitType>(entity =>
        {
            entity.HasKey(e => new { e.OutfitId, e.OutfitTypeId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("outfit_outfit_type");

            entity.HasIndex(e => e.OutfitTypeId, "Outfit_Type_Id");

            entity.Property(e => e.OutfitId).HasColumnName("Outfit_Id");
            entity.Property(e => e.OutfitTypeId).HasColumnName("Outfit_Type_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");

            entity.HasOne(d => d.Outfit).WithMany(p => p.OutfitOutfitTypes)
                .HasForeignKey(d => d.OutfitId)
                .HasConstraintName("outfit_outfit_type_ibfk_1");

            entity.HasOne(d => d.OutfitType).WithMany(p => p.OutfitOutfitTypes)
                .HasForeignKey(d => d.OutfitTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("outfit_outfit_type_ibfk_2");
        });

        modelBuilder.Entity<OutfitType>(entity =>
        {
            entity.HasKey(e => e.OutfitTypeId).HasName("PRIMARY");

            entity.ToTable("outfit_type");

            entity.Property(e => e.OutfitTypeId).HasColumnName("Outfit_Type_Id");
            entity.Property(e => e.TypeName)
                .HasMaxLength(255)
                .HasColumnName("Type_Name");
        });

        modelBuilder.Entity<PhotographPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PRIMARY");

            entity.ToTable("photograph_package");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.PackageId).HasColumnName("Package_Id");
            entity.Property(e => e.EventType).HasColumnName("Event_Type");
            entity.Property(e => e.ImageSamples)
                .HasColumnType("text")
                .HasColumnName("Image_Samples");
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.PackageContent)
                .HasColumnType("text")
                .HasColumnName("Package_Content");
            entity.Property(e => e.PackageName)
                .HasMaxLength(255)
                .HasColumnName("Package_Name");
            entity.Property(e => e.PackagePrice)
                .HasPrecision(18, 2)
                .HasColumnName("Package_Price");
            entity.Property(e => e.PhotoType).HasColumnName("Photo_Type");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.WorkFlow)
                .HasColumnType("text")
                .HasColumnName("Work_Flow");

            entity.HasOne(d => d.Service).WithMany(p => p.PhotographPackages)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("photograph_package_ibfk_1");
        });

        modelBuilder.Entity<PhotographService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("photograph_service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithOne(p => p.PhotographService)
                .HasForeignKey<PhotographService>(d => d.ServiceId)
                .HasConstraintName("photograph_service_ibfk_1");
        });

        modelBuilder.Entity<Photographer>(entity =>
        {
            entity.HasKey(e => e.PhotographerId).HasName("PRIMARY");

            entity.ToTable("photographer");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.PhotographerId).HasColumnName("Photographer_Id");
            entity.Property(e => e.About).HasColumnType("text");
            entity.Property(e => e.Artwork1)
                .HasColumnType("text")
                .HasColumnName("Artwork_1");
            entity.Property(e => e.Artwork2)
                .HasColumnType("text")
                .HasColumnName("Artwork_2");
            entity.Property(e => e.PhotographerImage)
                .HasColumnType("text")
                .HasColumnName("Photographer_Image");
            entity.Property(e => e.PhotographerName)
                .HasMaxLength(255)
                .HasColumnName("Photographer_Name");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithMany(p => p.Photographers)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("photographer_ibfk_1");
        });

        modelBuilder.Entity<RequestImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("request_image");

            entity.HasIndex(e => e.RequestId, "Request_Id");

            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.ImageText)
                .HasColumnType("text")
                .HasColumnName("Image_Text");
            entity.Property(e => e.ImageType)
                .HasColumnType("enum('BusinessLicense','CCCDFront','CCCDBack','HoldingID')")
                .HasColumnName("Image_Type");
            entity.Property(e => e.RequestId).HasColumnName("Request_Id");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestImages)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("request_image_ibfk_1");
        });

        modelBuilder.Entity<RequestUpgradeSupplier>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PRIMARY");

            entity.ToTable("request_upgrade_supplier");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.Property(e => e.RequestId).HasColumnName("Request_Id");
            entity.Property(e => e.BusinessType)
                .HasMaxLength(255)
                .HasColumnName("Business_Type");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("Full_Name");
            entity.Property(e => e.IdNumber)
                .HasMaxLength(50)
                .HasColumnName("ID_Number");
            entity.Property(e => e.RejectReason)
                .HasColumnType("text")
                .HasColumnName("Reject_Reason");
            entity.Property(e => e.RequestContent)
                .HasColumnType("text")
                .HasColumnName("Request_Content");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Approved','Rejected')");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.RequestUpgradeSuppliers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("request_upgrade_supplier_ibfk_1");
        });

        modelBuilder.Entity<RestaurantService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("restaurant_service");

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithOne(p => p.RestaurantService)
                .HasForeignKey<RestaurantService>(d => d.ServiceId)
                .HasConstraintName("restaurant_service_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PRIMARY");

            entity.ToTable("service");

            entity.HasIndex(e => e.ServiceTypeId, "Service_Type_Id");

            entity.HasIndex(e => e.SupplierId, "Supplier_Id");

            entity.HasIndex(e => e.TopicId, "Topic_Id");

            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.District).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(255)
                .HasColumnName("Service_Name");
            entity.Property(e => e.ServiceTypeId).HasColumnName("Service_Type_Id");
            entity.Property(e => e.StarNumber)
                .HasPrecision(4, 1)
                .HasColumnName("Star_Number");
            entity.Property(e => e.SupplierId).HasColumnName("Supplier_Id");
            entity.Property(e => e.TopicId).HasColumnName("Topic_Id");
            entity.Property(e => e.VisitWebsiteLink)
                .HasColumnType("text")
                .HasColumnName("Visit_Website_Link");
            entity.Property(e => e.Ward).HasMaxLength(255);

            entity.HasOne(d => d.ServiceType).WithMany(p => p.Services)
                .HasForeignKey(d => d.ServiceTypeId)
                .HasConstraintName("service_ibfk_3");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Services)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("service_ibfk_2");

            entity.HasOne(d => d.Topic).WithMany(p => p.Services)
                .HasForeignKey(d => d.TopicId)
                .HasConstraintName("service_ibfk_1");
        });

        modelBuilder.Entity<ServiceImage>(entity =>
        {
            entity.HasKey(e => e.ImageId).HasName("PRIMARY");

            entity.ToTable("service_image");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.Property(e => e.ImageId).HasColumnName("Image_Id");
            entity.Property(e => e.ImageText)
                .HasColumnType("text")
                .HasColumnName("Image_Text");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceImages)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("service_image_ibfk_1");
        });

        modelBuilder.Entity<ServiceTopic>(entity =>
        {
            entity.HasKey(e => new { e.ServiceId, e.TopicId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("service_topics");

            entity.HasIndex(e => e.ServiceId, "idx_servicetopics_service");

            entity.HasIndex(e => e.TopicId, "idx_servicetopics_topic");

            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.TopicId).HasColumnName("Topic_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceTopics)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_topics_ibfk_1");

            entity.HasOne(d => d.Topic).WithMany(p => p.ServiceTopics)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_topics_ibfk_2");
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId).HasName("PRIMARY");

            entity.ToTable("service_type");

            entity.Property(e => e.ServiceTypeId).HasColumnName("Service_Type_Id");
            entity.Property(e => e.ServiceTypeName)
                .HasMaxLength(255)
                .HasColumnName("Service_Type_Name");
        });

        modelBuilder.Entity<SingleBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PRIMARY");

            entity.ToTable("single_booking");

            entity.HasIndex(e => e.ArtistId, "Artist_Id");

            entity.HasIndex(e => e.InforId, "Infor_Id");

            entity.HasIndex(e => e.ServiceId, "Service_Id");

            entity.HasIndex(e => e.ServiceTypeId, "Service_Type_Id");

            entity.HasIndex(e => e.UserId, "User_Id");

            entity.Property(e => e.BookingId).HasColumnName("Booking_Id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_Id");
            entity.Property(e => e.BasicPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Basic_Price");
            entity.Property(e => e.BookingStatus).HasColumnName("Booking_Status");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.DateOfUse)
                .HasColumnType("datetime")
                .HasColumnName("Date_Of_Use");
            entity.Property(e => e.InforId).HasColumnName("Infor_Id");
            entity.Property(e => e.Note).HasColumnType("text");
            entity.Property(e => e.NumberOfUses).HasColumnName("Number_Of_Uses");
            entity.Property(e => e.PackageId).HasColumnName("Package_Id");
            entity.Property(e => e.ServiceId).HasColumnName("Service_Id");
            entity.Property(e => e.ServiceMode).HasColumnName("Service_Mode");
            entity.Property(e => e.ServiceTypeId).HasColumnName("Service_Type_Id");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(18, 2)
                .HasColumnName("Total_Price");
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.Artist).WithMany(p => p.SingleBookings)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("single_booking_ibfk_5");

            entity.HasOne(d => d.Infor).WithMany(p => p.SingleBookings)
                .HasForeignKey(d => d.InforId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("single_booking_ibfk_3");

            entity.HasOne(d => d.Service).WithMany(p => p.SingleBookings)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("single_booking_ibfk_2");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.SingleBookings)
                .HasForeignKey(d => d.ServiceTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("single_booking_ibfk_4");

            entity.HasOne(d => d.User).WithMany(p => p.SingleBookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("single_booking_ibfk_1");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tag");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.KeyName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("token");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.KeyName).HasColumnName("Key_Name");
            entity.Property(e => e.Expiration).HasColumnType("datetime");
            entity.Property(e => e.KeyValue)
                .HasColumnType("text")
                .HasColumnName("Key_Value");

            entity.HasOne(d => d.User).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("token_ibfk_1");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.TopicId).HasName("PRIMARY");

            entity.ToTable("topics");

            entity.Property(e => e.TopicId).HasColumnName("Topic_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasColumnType("datetime")
                .HasColumnName("Updated_At");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.ToTable("transaction");

            entity.Property(e => e.BankName).HasMaxLength(255);
            entity.Property(e => e.CardHolderName).HasMaxLength(255);
            entity.Property(e => e.CardNumber).HasMaxLength(20);
            entity.Property(e => e.ProcessedDate).HasColumnType("datetime");
            entity.Property(e => e.Reason).HasColumnType("text");
            entity.Property(e => e.RequestDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TransactionType).HasColumnType("enum('Deposit','Withdraw')");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleId, "Role_Id");

            entity.HasIndex(e => e.IsVipSupplier, "idx_user_isvipsupplier");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BackCmnd)
                .HasColumnType("text")
                .HasColumnName("Back_CMND");
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.District).HasMaxLength(255);
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FrontCmnd)
                .HasColumnType("text")
                .HasColumnName("Front_CMND");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("Full_Name");
            entity.Property(e => e.IsActive).HasColumnName("Is_Active");
            entity.Property(e => e.IsEmailConfirm).HasColumnName("Is_Email_Confirm");
            entity.Property(e => e.IsOnline).HasColumnName("Is_Online");
            entity.Property(e => e.IsUpgradeConfirmed).HasDefaultValueSql("'0'");
            entity.Property(e => e.IsVipSupplier)
                .HasDefaultValueSql("'0'")
                .HasColumnName("Is_Vip_Supplier");
            entity.Property(e => e.LoginProvider)
                .HasMaxLength(50)
                .HasColumnName("Login_Provider");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("Phone_Number");
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.UpdateAt)
                .HasColumnType("datetime")
                .HasColumnName("Update_At");
            entity.Property(e => e.UserImage)
                .HasColumnType("text")
                .HasColumnName("User_Image");
            entity.Property(e => e.UserName).HasMaxLength(50);
            entity.Property(e => e.Ward).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("user_ibfk_1");
        });

        modelBuilder.Entity<UserTopic>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.TopicId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("user_topics");

            entity.HasIndex(e => e.TopicId, "idx_usertopics_topic");

            entity.HasIndex(e => e.UserId, "idx_usertopics_user");

            entity.Property(e => e.UserId).HasColumnName("User_Id");
            entity.Property(e => e.TopicId).HasColumnName("Topic_Id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("Created_At");

            entity.HasOne(d => d.Topic).WithMany(p => p.UserTopics)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_topics_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.UserTopics)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_topics_ibfk_1");
        });

        modelBuilder.Entity<VoucherAdmin>(entity =>
        {
            entity.HasKey(e => e.VoucherId).HasName("PRIMARY");

            entity.ToTable("voucher_admin");

            entity.Property(e => e.VoucherId).HasColumnName("Voucher_Id");
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("End_Date");
            entity.Property(e => e.Percent).HasPrecision(5, 2);
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("Start_Date");
            entity.Property(e => e.TypeOfCombo).HasColumnName("Type_Of_Combo");
            entity.Property(e => e.TypeOfDiscount).HasColumnName("Type_Of_Discount");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
