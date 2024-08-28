# obis-mapper

**obis-mapper** is a lightweight .NET library designed to simplify the mapping of values to model properties based on
logical names. It supports nested models, handles nullable types, and allows default values to be specified in case of
conversion errors.

## Features

- **Logical Name Mapping**: Map values to properties based on logical names with support for tags to distinguish between
  different mappings.
- **Nested Models**: Automatically handles nested models through recursion, simplifying complex object structures.
- **Default Values**: Specify default values for properties in case of conversion errors, ensuring robustness in data
  mapping.
- **Flexible Type Conversion**: Supports nullable types and provides customizable conversion handling.

## Installation

You can install obis-mapper via NuGet Package Manager or .NET CLI.

### NuGet Package Manager

```bash
Install-Package obis-mapper
```

### .NET CLI

```bash
dotnet add package obis-mapper
```

## Usage

### Basic Usage

To start using obis-mapper, annotate your model properties with the `LogicalNameMappingAttribute` and optionally the
`NestedModelAttribute` for nested models.

```csharp
using ObisMapper;

public class SimpleModel
{
    [LogicalNameMapping("1.1.1.1")] 
    public int FirstNumericData { get; set; }

    [LogicalNameMapping("1.1.1.2")] 
    public int SecondNumericData { get; set; }

    [LogicalNameMapping("1.1.2.1")] 
    public string FirstStringData { get; set; }

    [LogicalNameMapping("1.1.2.2")] 
    public string SecondStringData { get; set; }
}
```

### Mapping Values

Use the `FillObisModel` extension method to map values to your model based on logical names.

```csharp
var model = new SimpleModel();
model.FillObisModel("1.1.1.1", 100);
model.FillObisModel("1.1.2.1", "Example");
```

### Nested Models

For nested models, use the `NestedModelAttribute`.

```csharp
public class ComplexModel
{
    [NestedModel]
    public SimpleModel Nested { get; set; }

    [LogicalNameMapping("2.1.1.1")] 
    public double SomeData { get; set; }
}

var complexModel = new ComplexModel();
complexModel.FillObisModel("1.1.1.1", 100);
```

### Handling Conversion Errors and Default Values

You can specify a default value in the `LogicalNameMappingAttribute` to handle cases where conversion fails.

```csharp
public class ModelWithDefaults
{
    [LogicalNameMapping("1.1.1.1", DefaultValue = 42)] 
    public int ValueWithDefault { get; set; }
}

var model = new ModelWithDefaults();
model.FillObisModel("1.1.1.1", "InvalidValue");
```

In this example, `ValueWithDefault` will be set to `42` if the provided value is invalid.

## Documentation

In-development

## Contributing

Contributions are welcome! Please feel free to submit issues, feature requests, or pull requests.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.