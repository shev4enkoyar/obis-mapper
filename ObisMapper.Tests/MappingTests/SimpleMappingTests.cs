using ObisMapper.Models;
using ObisMapper.Tests.Models;

namespace ObisMapper.Tests.MappingTests;

public sealed class SimpleMappingTests
{
    [Theory]
    [MemberData(nameof(GetCorrectTestDataForSimpleModel))]
    public void FillSimpleModel_ShouldUseTransmittedValues_WhenCorrectDataProvided(List<ObisDataItem> data,
        SimpleModel expectedModel)
    {
        // Given
        var model = new SimpleModel();
        var mapper = new LogicalNameMapper();

        // When
        foreach (var dataItem in data)
            mapper.FillObisModel(model, dataItem.LogicalName, dataItem.Value);

        // Then
        Assert.Equal(model, expectedModel);
    }

    [Theory]
    [MemberData(nameof(GetIncorrectTestDataForSimpleModel))]
    public void FillSimpleModel_ShouldUseDefaultValues_WhenInvalidDataProvided(List<ObisDataItem> data,
        SimpleModel expectedModel)
    {
        // Given
        var model = new SimpleModel();
        var mapper = new LogicalNameMapper();

        // When
        foreach (var dataItem in data)
            mapper.FillObisModel(model, dataItem.LogicalName, dataItem.Value);

        // Then
        Assert.Equal(model, expectedModel);
    }

    public static IEnumerable<object[]> GetCorrectTestDataForSimpleModel()
    {
        yield return
        [
            new List<ObisDataItem>
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
            new List<ObisDataItem>
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

    public static IEnumerable<object[]> GetIncorrectTestDataForSimpleModel()
    {
        yield return
        [
            new List<ObisDataItem>
            {
                new("1.1.1.1", Guid.Empty),
                new("1.1.1.2", Guid.Empty),
                new("1.1.2.1", Guid.Empty),
                new("1.1.2.2", Guid.Empty),
                new("1.1.3.1", Guid.Empty),
                new("1.1.3.2", Guid.Empty)
            },
            new SimpleModel
            {
                FirstNumericData = default,
                SecondNumericData = 10,
                FirstStringData = default!,
                SecondStringData = "error value",
                FirstDoubleData = default,
                SecondDoubleData = 3.14
            }
        ];
    }
}