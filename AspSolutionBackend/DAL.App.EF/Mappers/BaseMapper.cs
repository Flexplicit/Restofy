using AutoMapper;
using Contracts.DAL.Base.Mappers;

namespace DAL.APP.EF.Mappers
{
    public class BaseMapper<TOutEntity, TInEntity> : IBaseMapper<TOutEntity, TInEntity> 
    {
        protected readonly IMapper Mapper;

        public BaseMapper(IMapper mapper)
        {
            Mapper = mapper;
        }
        
        public TOutEntity Map(TInEntity? inEntity)
        {
            return Mapper.Map<TOutEntity>(inEntity);
        }

        public TInEntity Map(TOutEntity? inEntity)
        {
            return Mapper.Map<TInEntity>(inEntity);
        }
    }
}