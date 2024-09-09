using ObisMapper.FluentMapper.Abstraction;

namespace ObisMapper.FluentMapper
{
    public class ObisMapper<TModel>
    {
        private MappingExpression<TModel>? _expression;

        public IMappingExpression<TModel> CreateMap()
        {
            _expression = new MappingExpression<TModel>();
            return _expression;
        }
    }
}