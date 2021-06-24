using System;
using System.Collections.Generic;


namespace PublicApiDTO.v1.v1.OrderModels
{
    public class FoodInOrder : FoodInOrderCreate
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
    }

    public class FoodInOrderCreate
    {
        public Guid FoodId { get; set; }
        public int Amount { get; set; }
    }

    public class FoodInOrderWithFood : FoodInOrder
    {
        public Food Food { get; set; } = null!;
    }
}