using AutoMapper;

namespace DAL.APP.EF.Mappers
{
    public class ContactMapper: BaseMapper<DAL.App.DTO.OrderModels.Contact, Domain.OrderModels.Contact>
    {
        public ContactMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}