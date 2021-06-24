using AutoMapper;
using Contracts.BLL.Base.Mappers;


namespace PublicApiDTO.v1.v1.Mappers
{
    // OUT  = ApiDTO, in = BllDTO
    public class BaseApiMapper<TOutEntity, TIntEntity> : IBaseMapper<TOutEntity, TIntEntity>
    {
        protected IMapper Mapper;

        public BaseApiMapper(IMapper mapper)
        {
            Mapper = mapper;
        }

        public TOutEntity? Map(TIntEntity? inEntity)
        {
            return Mapper.Map<TOutEntity>(inEntity);
        }

        public TIntEntity? Map(TOutEntity? inEntity)
        {
            return Mapper.Map<TIntEntity>(inEntity);
        }
    }
}