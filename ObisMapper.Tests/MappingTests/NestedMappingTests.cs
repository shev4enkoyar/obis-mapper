using ObisMapper.Tests.Models;

namespace ObisMapper.Tests.MappingTests;

public sealed class NestedMappingTests
{
    [Theory]
    [MemberData(nameof(GetCorrectTestDataForModelWithNestedModel))]
    public void FillNestedModel_ShouldUseTransmittedValues_WhenCorrectDataProvided(List<ObisDataItem> data,
        ModelWithNestedModel expectedModelWithNestedModel)
    {
        // Given
        var model = new ModelWithNestedModel();
        var mapper = new LogicalNameMapper();

        // When
        foreach (var dataItem in data)
            mapper.FillObisModel(model, dataItem.LogicalName, dataItem.Value);

        // Then
        Assert.Equal(model, expectedModelWithNestedModel);
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
}