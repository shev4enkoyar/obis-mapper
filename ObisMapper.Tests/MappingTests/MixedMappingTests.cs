using ObisMapper.Abstractions;
using ObisMapper.Abstractions.Mappings;
using ObisMapper.Models;

namespace ObisMapper.Tests.MappingTests;

public sealed class MixedMappingTests
{
    [Fact]
    public void AbstractMappingTest()
    {
        var testModelConverter = new TestModelConverter();
    }
}

internal class TestModel : IObisModel
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<string> NoHomos { get; set; } = [];
}

internal class TestModelConverter : AbstractObisModelConverter<TestModel>
{
    public TestModelConverter()
    {
        var noHomosLogicalNames = new LogicalNameModelGroup([
            new LogicalNameModel("1.1.1.3"),
            new LogicalNameModel("1.1.1.4", "ws")
        ]);

        RuleFor(x => x.Id)
            .AddLogicalName(new LogicalNameModel("1.1.1.1"))
            .AddDefaultValue(13)
            .AddConverter(value => (int)value);

        RuleFor(x => x.Name)
            .AddLogicalName(new LogicalNameModel("1.1.1.2"))
            .AddDefaultValue("mew")
            .AddConverter(value => (string)value)
            .AddValidator(value => !string.IsNullOrWhiteSpace(value));

        RuleFor(x => x.NoHomos)
            .AddLogicalName(noHomosLogicalNames)
            .AddDefaultValue([]);
    }
}