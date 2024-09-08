using ObisMapper.FluentMapper.Abstraction;

namespace ObisMapper.FluentMapper
{
    public class ObisMapper
    {
        public IMappingExpression<TModel> CreateMap<TModel>()
        {
            return new MappingExpression<TModel>();
        }
    }
}