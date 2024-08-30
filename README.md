# obis-mapper

**obis-mapper** is a lightweight .NET library designed to simplify the process of mapping values to model properties
based on logical names. The library supports complex object structures with nested models, handles nullable types,
allows for default values in case of conversion errors, and provides customizable conversion logic through custom
property mappings.

## Features

- **Logical Name Mapping**: Effortlessly map values to model properties using logical names, with support for tags to
  differentiate between multiple mappings.
- **Nested Model Support**: Seamlessly handles nested models using recursion, enabling easy management of complex object
  hierarchies.
- **Custom Property Mapping**: Flexibly define custom conversion logic for model properties, allowing tailored value
  transformations.
- **Default Value Handling**: Set default values for properties to ensure robustness, especially when encountering
  conversion errors.
- **Efficient Type Conversion**: Supports nullable types with built-in type conversion, optimizing performance through
  caching and reflection.

## Installation

You can install **obis-mapper** using the NuGet Package Manager or the .NET CLI.

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

To start using **obis-mapper**, annotate your model properties with the `LogicalNameMappingAttribute`. For nested
models, you can use the `NestedModelAttribute`.

```csharp
using ObisMapper;
using ObisMapper.Attributes;

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

Use the `FillObisModel` method to map values to your model based on logical names.

```csharp
var model = new SimpleModel();
var mapper = new LogicalNameMapper();
mapper.FillObisModel(model, "1.1.1.1", 100);
mapper.FillObisModel(model, "1.1.2.1", "Example");
```

### Nested Models

For models with nested structures, use the `NestedModelAttribute` to define nested properties.

```csharp
public class ComplexModel
{
    [NestedModel]
    public SimpleModel Nested { get; set; }

    [LogicalNameMapping("2.1.1.1")] 
    public double SomeData { get; set; }
}

var complexModel = new ComplexModel();
var mapper = new LogicalNameMapper();
mapper.FillObisModel(complexModel, "1.1.1.1", 100);
```

### Custom Property Mapping

Define custom conversion logic for properties using the `CustomPropertyMapping` class to handle specific conversion
scenarios.

```csharp
var customMapping = new CustomPropertyMapping<SimpleModel>()
    .CreateMapping(
        m => m.FirstNumericData, 
        (currentValue, value) => currentValue + Convert.ToInt32(value)
    );

var model = new SimpleModel();
var mapper = new LogicalNameMapper();
mapper.FillObisModel(model, "1.1.1.1", 100, customMapping: customMapping);
```

### Handling Conversion Errors and Default Values

You can specify default values in the `LogicalNameMappingAttribute` to handle conversion errors gracefully.

```csharp
public class ModelWithDefaults
{
    [LogicalNameMapping("1.1.1.1", DefaultValue = 42)] 
    public int ValueWithDefault { get; set; }
}

var model = new ModelWithDefaults();
var mapper = new LogicalNameMapper();
mapper.FillObisModel(model, "1.1.1.1", "InvalidValue");
```

In this example, `ValueWithDefault` will be set to `42` if the provided value cannot be converted.

## Documentation

Detailed documentation is under development and will be available soon.

## Contributing

Contributions are highly encouraged! Feel free to submit issues, feature requests, or pull requests to improve the
library.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.