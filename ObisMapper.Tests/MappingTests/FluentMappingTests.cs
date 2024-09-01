using ObisMapper.Abstractions;
using ObisMapper.Fluent;
using ObisMapper.Models;
using ObisMapper.Tests.Models;

namespace ObisMapper.Tests.MappingTests;

public class FluentMappingTests
{
    [Theory]
    [MemberData(nameof(GetCorrectTestDataForSimpleModel))]
    public async Task FillFluentSimpleModel_ShouldUseTransmittedValues_WhenCorrectDataProvided(List<ObisDataModel> data,
        SimpleModel expectedModel)
    {
        // Given
        var model = new SimpleModel();
        var mapper = new ObisMapper();
        var configuration = new SimpleModelConfiguration();

        // When
        foreach (var dataItem in data)
            await mapper.PartialMapAsync(model, dataItem, configuration);

        // Then
        Assert.Equal(model, expectedModel);
    }

    public static IEnumerable<object[]> GetCorrectTestDataForSimpleModel()
    {
        yield return
        [
            new List<ObisDataModel>
            {
                new("1.1.1.1", 1),
                new("1.1.1.2", 2),
                new("1.1.2.1", "Test1"),
                new("1.1.2.2", "Test2"),
                new("1.1.3.1", 1.23),
                new("1.1.3.2", 2.56)
            },
            new SimpleModel
            {
                FirstNumericData = 1,
                SecondNumericData = 2,
                FirstStringData = "Test1",
                SecondStringData = "Test2",
                FirstDoubleData = 1.23,
                SecondDoubleData = 2.56
            }
        ];
        yield return
        [
            new List<ObisDataModel>
            {
                new("1.1.1.1", int.MaxValue),
                new("1.1.1.2", int.MinValue),
                new("1.1.2.1", "Some some some ?|}{$@#!^%(*)_"),
                new("1.1.2.2", "some some ?|}{$@#!^%(*)_"),
                new("1.1.3.1", double.MaxValue),
                new("1.1.3.2", double.MinValue)
            },
            new SimpleModel
            {
                FirstNumericData = int.MaxValue,
                SecondNumericData = int.MinValue,
                FirstStringData = "Some some some ?|}{$@#!^%(*)_",
                SecondStringData = "some some ?|}{$@#!^%(*)_",
                FirstDoubleData = double.MaxValue,
                SecondDoubleData = double.MinValue
            }
        ];
    }

    private class SimpleModelConfiguration : ModelConfiguration<SimpleModel>
    {
        public SimpleModelConfiguration()
        {
            RuleFor(x => x.FirstNumericData)
                .AddLogicalName(new LogicalNameModel("1.1.1.1"))
                .AddValidatorAsync((x, token) => { return Task.FromResult(true); })
                .AddValidator(x => x is < 5 or int.MaxValue)
                .AddDefaultValue(10);

            RuleFor(x => x.SecondNumericData)
                .AddLogicalName(new LogicalNameModel("1.1.1.2"));

            RuleFor(x => x.FirstStringData)
                .AddLogicalName(new LogicalNameModel("1.1.2.1"));

            RuleFor(x => x.SecondStringData)
                .AddLogicalName(new LogicalNameModel("1.1.2.2"));

            RuleFor(x => x.FirstDoubleData)
                .AddLogicalName(new LogicalNameModel("1.1.3.1"));

            RuleFor(x => x.SecondDoubleData)
                .AddLogicalName(new LogicalNameModel("1.1.3.2"));
        }
    }
}