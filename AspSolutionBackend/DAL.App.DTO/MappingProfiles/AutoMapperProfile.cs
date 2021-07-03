using AutoMapper;

    
namespace DAL.App.DTO.MappingProfiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            
            
            
            CreateMap<DAL.App.DTO.Identity.AppRole, Domain.Identity.AppRole>().ReverseMap();
            CreateMap<DAL.App.DTO.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.Bill, Domain.OrderModels.Bill>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.Contact, Domain.OrderModels.Contact>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.Cost, Domain.OrderModels.Cost>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.Food, Domain.OrderModels.Food>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.Order, Domain.OrderModels.Order>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.Restaurant, Domain.OrderModels.Restaurant>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.Subscription, Domain.OrderModels.Subscription>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.BillLine, Domain.OrderModels.BillLine>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.ContactType, Domain.OrderModels.ContactType>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.CreditCard, Domain.OrderModels.CreditCard>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.FoodGroup, Domain.OrderModels.FoodGroup>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.PaymentType, Domain.OrderModels.PaymentType>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.RestaurantSubscription, Domain.OrderModels.RestaurantSubscription>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.CreditCardInfo, Domain.OrderModels.CreditCardInfo>().ReverseMap();
            CreateMap<DAL.App.DTO.OrderModels.FoodInOrder, Domain.OrderModels.FoodInOrder>().ReverseMap();
        }
    }
}