namespace Contracts.DAL.Base.Mappers
{
    public interface IBaseMapper<TOutEntity, TInEntity>
    {
        TOutEntity? Map(TInEntity? inEntity);
        TInEntity? Map(TOutEntity? inEntity);
    }
}