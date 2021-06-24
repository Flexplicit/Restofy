using AutoMapper;
using DAL.APP.EF.Mappers;

namespace BLL.App.Mappers
{
    public class ContactMapper : BaseMapper<BLL.App.DTO.OrderModels.Contact, DAL.App.DTO.OrderModels.Contact>
    {
        public ContactMapper(IMapper mapper) : base(mapper)
        {
        }
    }
}