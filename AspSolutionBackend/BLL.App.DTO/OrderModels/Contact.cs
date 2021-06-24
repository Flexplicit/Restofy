using System;
using System.ComponentModel.DataAnnotations;
using BLL.App.DTO.Identity;
using Contracts.Domain.Base;
using Domain.Base;
using BLL.App.DTO;


namespace BLL.App.DTO.OrderModels

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