namespace DocumentAPI.DTOs;
public class RandomUserResponseDto
{
    public List<Result> Results { get; set; }
    public Info Info { get; set; }
}

public class Result
{
    public string Gender { get; set; }
    public Name Name { get; set; }
    public Location Location { get; set; }
    public string Email { get; set; }
    public Picture Picture { get; set; }
    
}

public class Name
{
    public string Title { get; set; }
    public string First { get; set; }
    public string Last { get; set; }
}

public class Location
{
    public Street Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public object Postcode { get; set; }
}

public class Street
{
    public int Number { get; set; }
    public string Name { get; set; }
}

public class Picture
{
    public string Large { get; set; }
    public string Medium { get; set; }
    public string Thumbnail { get; set; }
}

public class Info
{
    public string Seed { get; set; }
    public int Results { get; set; }
    public int Page { get; set; }
    public string Version { get; set; }
}
