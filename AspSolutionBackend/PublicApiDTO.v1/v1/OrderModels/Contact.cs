using System;
using System.ComponentModel.DataAnnotations;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class Contact : ContactCreate
    {
        public Guid Id { get; set; }
    }

    public class ContactCreate
    {
        public Guid ContactTypeId { get; set; }
        public Guid? RestaurantId { get; set; }
        [Required] [StringLength(100)] public string ContactValue { get; set; } = null!;
    }

    public class ContactView : Contact
    {
        public string Type { get; set; } = null!;
    }
}