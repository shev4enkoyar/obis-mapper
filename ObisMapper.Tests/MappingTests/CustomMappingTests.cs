using ObisMapper.Models;
using ObisMapper.Tests.Models;

namespace ObisMapper.Tests.MappingTests;

public sealed class CustomMappingTests
{
    [Theory]
    [MemberData(nameof(GetCorrectTestDataForCustomMapping))]
    public void FillComplexModel_ShouldUseTransmittedValues_WhenCorrectDataProvided(List<ObisDataItem> data,
        ComplexModel expectedModel)
    {
        // Given
        var model = new ComplexModel();
        var mapper = new LogicalNameMapper();

        var customPropertyMapping = new CustomPropertyMapping<ComplexModel>()
            .CreateMapping(x => x.NumericValue, (_, newValue) => (int)newValue * 25)
            .CreateMapping(x => x.NumericValue, (_, newValue) => (int)newValue * 25)
            .CreateMapping(x => x.EnumeratedValue, (propertyData, newValue) =>
            {
                propertyData.Add((int)newValue);
                return propertyData;
            });

        // When
        foreach (var dataItem in data)
            mapper.FillObisModel(model, dataItem.LogicalName, dataItem.Value, customMapping: customPropertyMapping);

        // Then
        Assert.Equal(model, expectedModel);
    }

    [Theory]
    [MemberData(nameof(GetTestDataForDefaultCustomMapping))]
    public void FillComplexModel_ShouldUseDefaultValues_WhenExceptionThrownInMapping(List<ObisDataItem> data,
        ComplexModel expectedModel)
    {
        // Given
        var model = new ComplexModel();
        var mapper = new LogicalNameMapper();

        var customPropertyMapping = new CustomPropertyMapping<ComplexModel>()
            .CreateMapping(x => x.NumericValue, (_, _) => throw new Exception())
            .CreateMapping(x => x.EnumeratedValue, (_, _) => throw new Exception());

        // When
        foreach (var dataItem in data)
            mapper.FillObisModel(model, dataItem.LogicalName, dataItem.Value, customMapping: customPropertyMapping);

        // Then
        Assert.Equal(model, expectedModel);
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
                NumericValue = 25,
                EnumeratedValue = [2, 3, 4, 5, 6]
            }
        ];
        yield return
        [
            new List<ObisDataItem>
            {
                new("1.1.1.1", 14),
                new("1.1.1.2", 5),
                new("1.1.1.2", -2),
                new("1.1.1.2", 4),
                new("1.1.1.2", 5),
                new("1.1.1.2", 6)
            },
            new ComplexModel
            {
                NumericValue = 350,
                EnumeratedValue = [5, -2, 4, 5, 6]
            }
        ];
    }

    public static IEnumerable<object[]> GetTestDataForDefaultCustomMapping()
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
                NumericValue = 25,
                EnumeratedValue = null!
            }
        ];
    }
}