using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class Cost : CostEdit
    {
         public decimal CostWithoutVat { get; set; }

    }

    public class CostEdit : CostCreate
    {
        public Guid Id { get; set; }
    }

    public class CostCreate
    {
        public decimal CostWithVat { get; set; }
        public decimal Vat { get; set; } = 0.2m;
    }
}