using System;
using System.ComponentModel.DataAnnotations;
using Domain.OrderModels.DbEnums;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class ContactType
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; } = null!;
    }
}