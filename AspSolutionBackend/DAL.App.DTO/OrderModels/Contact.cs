using System;
using System.ComponentModel.DataAnnotations;
using Contracts.Domain.Base;
using DAL.App.DTO.Identity;
using Domain.Base;


namespace DAL.App.DTO.OrderModels
{
    public sealed class Contact : DomainEntityId
    {
        public Guid ContactTypeId { get; set; }
        public Guid? RestaurantId { get; set; }

        [Required] [StringLength(100)] public string ContactValue { get; set; } = null!;


        public ContactType? ContactType { get; set; }
        public Restaurant? Restaurant { get; set; }


        public AppUser? AppUser { get; set; }
        public Guid? AppUserId { get; set; }
    }
}