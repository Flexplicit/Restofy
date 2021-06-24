using AutoMapper;

namespace BLL.App.Mappers
{
    public class ContactTypeMapper: BaseMapper<BLL.App.DTO.OrderModels.ContactType, DAL.App.DTO.OrderModels.ContactType>
    {
        public ContactTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}