using ObisMapper.Models;
using ObisMapper.Tests.Models;

namespace ObisMapper.Tests.MappingTests;

public class FluentNewMappingTest
{
    [Fact]
    public void TestNewMapping()
    {
        var mapper = new FluentMapper.ObisMapper();
        mapper.CreateMap<Person>()
            .ForMember(x => x.Name, new LogicalNameGroup(new LogicalName("1.1.1.1")), configuration =>
            {
                configuration.AddConverter(o => (string)o);
            })
            .ForMember(x => x.Address, configure =>
            {
                configure.ForMember(x => x.City, new LogicalNameGroup(new LogicalName("2.1.1.1")), configuration =>
                {
                    configuration.AddConverter(o => (string)o);
                });
            });
        
        var dataItems = new List<ObisDataItem>
        {
            new("1.1.1.1", "John Doe"),
            new("2.1.1.1", "New York"),
            new("3.1.1.1", "5th Avenue"),
            new("3.1.1.2", 101)
        };

        // var mapper = new Mapper(configuration, tag);
        // var resultModel = new Person();
        //
        // foreach (var item in dataItems)
        // {
        //     mapper.Map(item, resultModel);
        // }
    }
}