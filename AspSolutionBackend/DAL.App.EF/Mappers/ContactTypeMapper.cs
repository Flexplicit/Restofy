using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class ContactTypeMapper: BaseMapper<DAL.App.DTO.OrderModels.ContactType, Domain.OrderModels.ContactType>
    {
        public ContactTypeMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}