using AutoMapper;
using PublicApiDTO.v1.v1.OrderModels;

namespace PublicApiDTO.v1.v1.Mappers
{
    public class ContactTypeMapper : BaseApiMapper<ContactType, BLL.App.DTO.OrderModels.ContactType>
    {
        public ContactTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}