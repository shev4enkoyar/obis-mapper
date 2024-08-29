using ObisMapper.Tests.Models;

namespace ObisMapper.Tests;

public class MappingTests
{
    [Theory]
    [MemberData(nameof(GetCorrectTestDataForSimpleModel))]
    public void Fill_SimpleModel_Correctly(List<ObisDataItem> data, SimpleModel expectedModel)
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
    [MemberData(nameof(GetCorrectTestDataForModelWithNestedModel))]
    public void Fill_NestedModel_Correctly(List<ObisDataItem> data, ModelWithNestedModel expectedModelWithNestedModel)
    {
        // Given
        var model = new ModelWithNestedModel();

        // When
        foreach (var dataItem in data)
            model.FillObisModel(dataItem.LogicalName, dataItem.Value);

        // Then
        Assert.Equal(model, expectedModelWithNestedModel);
    }

    [Theory]
    [MemberData(nameof(GetCorrectTestDataForCustomMapping))]
    public void Fill_ModelWithCustomMapping_Correctly(List<ObisDataItem> data, ComplexModel expectedModel)
    {
        // Given
        var model = new ComplexModel();
        var customPropertyMapping = new CustomPropertyMapping<ComplexModel>()
            .CreateMapping(x => x.EnumeratedValue, (propertyData, newValue) =>
            {
                propertyData.Add((int)newValue);
                return propertyData;
            });

        // When
        foreach (var dataItem in data)
            model.FillObisModel(dataItem.LogicalName, dataItem.Value, customMapping: customPropertyMapping);

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

    public static IEnumerable<object[]> GetCorrectTestDataForModelWithNestedModel()
    {
        yield return
        [
            new List<ObisDataItem>
            {
                new("1.1.1.1", 1),
                new("2.1.1.1", 2),
                new("2.1.1.2", "Test")
            },
            new ModelWithNestedModel
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

    public static IEnumerable<object[]> GetCorrectTestDataForCustomMapping()
    {
        yield return
        [
            new List<ObisDataItem>
            {
                new("1.1.1.1", 1),
                new("1.1.1.2", 2),
                new("1.1.1.2", 3),
                new("1.1.1.2", 4),
                new("1.1.1.2", 5),
                new("1.1.1.2", 6)
            },
            new ComplexModel
            {
                NumericValue = 1,
                EnumeratedValue = [2, 3, 4, 5, 6]
            }
        ];
    }
}