namespace ObisMapper.Tests.MappingTests;

public class Country
{
    public string Name { get; set; }
    public string Code { get; set; }
}

public class Address
{
    public string City { get; set; }
    public string Street { get; set; }
    public Country Country { get; set; }
}

public class Source
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public Address Address { get; set; }
}

public class DestinationCountry
{
    public string Name { get; set; }
    public string Code { get; set; }
}

public class DestinationAddress
{
    public string City { get; set; }
    public string Street { get; set; }
    public DestinationCountry Country { get; set; }
}

public class Destination
{
    public string FullName { get; set; }
    public int Age { get; set; }
    public DestinationAddress Address { get; set; }
}
