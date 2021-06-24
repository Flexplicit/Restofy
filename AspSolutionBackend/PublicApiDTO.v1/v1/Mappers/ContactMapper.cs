using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class ContactMapper : BaseApiMapper<Contact, BLL.App.DTO.OrderModels.Contact>
    {
        public ContactMapper(IMapper mapper) : base(mapper)
        {
        }

        public static BLL.App.DTO.OrderModels.Contact MapContactPublicToBll(ContactCreate con)
        {
            return new()
            {
                ContactTypeId = con.ContactTypeId,
                ContactValue = con.ContactValue,
                RestaurantId = con.RestaurantId
            };
        }

        public static PublicApiDTO.v1.v1.OrderModels.ContactView MapBllToContactView(
            BLL.App.DTO.OrderModels.Contact con)
        {
            return new()
            {
                Id = con.Id,
                ContactTypeId = con.ContactTypeId,
                ContactValue = con.ContactValue,
                RestaurantId = con.RestaurantId,
                Type = con.ContactType!.TypeName.ToString()
            };
        }
    }
}