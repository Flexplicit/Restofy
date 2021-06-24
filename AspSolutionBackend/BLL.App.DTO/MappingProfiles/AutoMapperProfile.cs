using AutoMapper;

namespace BLL.App.DTO.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BLL.App.DTO.Identity.AppRole, DAL.App.DTO.Identity.AppRole>().ReverseMap();
            CreateMap<BLL.App.DTO.Identity.AppUser, DAL.App.DTO.Identity.AppUser>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.Bill, DAL.App.DTO.OrderModels.Bill>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.Contact, DAL.App.DTO.OrderModels.Contact>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.Cost, DAL.App.DTO.OrderModels.Cost>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.Food, DAL.App.DTO.OrderModels.Food>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.Order, DAL.App.DTO.OrderModels.Order>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.Restaurant, DAL.App.DTO.OrderModels.Restaurant>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.Subscription, DAL.App.DTO.OrderModels.Subscription>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.BillLine, DAL.App.DTO.OrderModels.BillLine>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.ContactType, DAL.App.DTO.OrderModels.ContactType>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.CreditCard, DAL.App.DTO.OrderModels.CreditCard>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.FoodGroup, DAL.App.DTO.OrderModels.FoodGroup>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.PaymentType, DAL.App.DTO.OrderModels.PaymentType>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.RestaurantSubscription, DAL.App.DTO.OrderModels.RestaurantSubscription>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.CreditCardInfo, DAL.App.DTO.OrderModels.CreditCardInfo>().ReverseMap();
            CreateMap<BLL.App.DTO.OrderModels.FoodInOrder, DAL.App.DTO.OrderModels.FoodInOrder>().ReverseMap();
        }
    }
}