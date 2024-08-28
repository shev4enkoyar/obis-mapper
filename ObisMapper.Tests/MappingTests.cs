using ObisMapper.Tests.Models;

namespace ObisMapper.Tests;

public class MappingTests
{
    [Theory]
    [MemberData(nameof(GetCorrectSimpleTestData))]
    public void FillSimpleModelCorrectly(List<ObisDataItem> data, SimpleModel expectedModel)
    {
        // Given
        var model = new SimpleModel();

        // When
        foreach (var dataItem in data)
            model.FillObisModel(dataItem.LogicalName, dataItem.Value);

        // Then
        Assert.Equal(model, expectedModel);
    }

    [Theory]
    [MemberData(nameof(GetCorrectComplexTestData))]
    public void FillComplexModelCorrectly(List<ObisDataItem> data, ComplexModel expectedModel)
    {
        // Given
        var model = new ComplexModel();

        // When
        foreach (var dataItem in data)
            model.FillObisModel(dataItem.LogicalName, dataItem.Value);

        // Then
        Assert.Equal(model, expectedModel);
    }

    public static IEnumerable<object[]> GetCorrectSimpleTestData()
    {
        yield return
        [
            new List<ObisDataItem>
            {
                new() { LogicalName = "1.1.1.1", Value = 1 },
                new() { LogicalName = "1.1.1.2", Value = 2 },
                new() { LogicalName = "1.1.2.1", Value = "Test1" },
                new() { LogicalName = "1.1.2.2", Value = "Test2" },
                new() { LogicalName = "1.1.3.1", Value = 1.23 },
                new() { LogicalName = "1.1.3.2", Value = 2.56 }
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
                new() { LogicalName = "1.1.1.1", Value = int.MaxValue },
                new() { LogicalName = "1.1.1.2", Value = int.MinValue },
                new() { LogicalName = "1.1.2.1", Value = "Some some some ?|}{$@#!^%(*)_" },
                new() { LogicalName = "1.1.2.2", Value = "some some ?|}{$@#!^%(*)_" },
                new() { LogicalName = "1.1.3.1", Value = double.MaxValue },
                new() { LogicalName = "1.1.3.2", Value = double.MinValue }
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

    public static IEnumerable<object[]> GetCorrectComplexTestData()
    {
        yield return
        [
            new List<ObisDataItem>
            {
                new() { LogicalName = "1.1.1.1", Value = 1 },
                new() { LogicalName = "2.1.1.1", Value = 2 },
                new() { LogicalName = "2.1.1.2", Value = "Test" }
            },
            new ComplexModel
            {
                SimpleData = 1,
                NestedData = new NestedModel
                {
                    NestedIntData = 2,
                    NestedStringData = "Test"
                }
            }
        ];
    }
}