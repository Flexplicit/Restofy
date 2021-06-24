using AutoMapper;
using PublicDTO = PublicApiDTO.v1.v1;
using BllAppDTO = BLL.App.DTO;


namespace PublicApiDTO.v1.v1.Mappers.MappingProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<PublicDTO.OrderModels.Restaurant, BllAppDTO.OrderModels.Restaurant>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.Food, BllAppDTO.OrderModels.Food>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.FoodGroup, BllAppDTO.OrderModels.FoodGroup>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.PaymentType, BllAppDTO.OrderModels.PaymentType>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.ContactType, BllAppDTO.OrderModels.ContactType>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.Contact, BllAppDTO.OrderModels.Contact>().ReverseMap();
            
            CreateMap<PublicDTO.OrderModels.Bill, BllAppDTO.OrderModels.Bill>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.BillLine, BllAppDTO.OrderModels.BillLine>().ReverseMap();
            
            CreateMap<PublicDTO.OrderModels.CreditCard, BllAppDTO.OrderModels.CreditCard>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.FoodInOrder, BllAppDTO.OrderModels.FoodInOrder>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.Order, BllAppDTO.OrderModels.Order>().ReverseMap();
            
            CreateMap<PublicDTO.OrderModels.Subscription, BllAppDTO.OrderModels.Subscription>().ReverseMap();
            CreateMap<PublicDTO.OrderModels.RestaurantSubscription, BllAppDTO.OrderModels.RestaurantSubscription>().ReverseMap();
        }
    }
}