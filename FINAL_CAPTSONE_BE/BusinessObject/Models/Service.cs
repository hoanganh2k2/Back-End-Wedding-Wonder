using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public int SupplierId { get; set; }

    public int ServiceTypeId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? Description { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? Address { get; set; }

    public decimal? StarNumber { get; set; }

    public string? VisitWebsiteLink { get; set; }

    public int? IsActive { get; set; }

    public int? TopicId { get; set; }

    public virtual ICollection<BookingServiceDetail> BookingServiceDetails { get; } = new List<BookingServiceDetail>();

    public virtual ICollection<BusySchedule> BusySchedules { get; } = new List<BusySchedule>();

    public virtual ClothesService? ClothesService { get; set; }

    public virtual ICollection<ContractService> ContractServices { get; } = new List<ContractService>();

    public virtual ICollection<Contractbooking> Contractbookings { get; } = new List<Contractbooking>();

    public virtual ICollection<CustomerReview> CustomerReviews { get; } = new List<CustomerReview>();

    public virtual EventOrganizerService? EventOrganizerService { get; set; }

    public virtual ICollection<FavoriteService> FavoriteServices { get; } = new List<FavoriteService>();

    public virtual InvitationService? InvitationService { get; set; }

    public virtual MakeUpService? MakeUpService { get; set; }

    public virtual PhotographService? PhotographService { get; set; }

    public virtual RestaurantService? RestaurantService { get; set; }

    public virtual ICollection<ServiceImage> ServiceImages { get; } = new List<ServiceImage>();

    public virtual ICollection<ServiceTopic> ServiceTopics { get; } = new List<ServiceTopic>();

    public virtual ServiceType ServiceType { get; set; } = null!;

    public virtual ICollection<SingleBooking> SingleBookings { get; } = new List<SingleBooking>();

    public virtual User Supplier { get; set; } = null!;

    public virtual Topic? Topic { get; set; }
}
