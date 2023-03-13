using System.ComponentModel.DataAnnotations;

namespace GUIDE.Models;

public class Yahoo
{
    public Chart chart { get; set; }
}
public class Chart
{
    public List<Result> result { get; set; }
}

public class Indicators
{
    public List<Quote> quote { get; set; }
}

public class Quote
{
    public List<double> open { get; set; }
}

public class Result
{
    public List<int> timestamp { get; set; }
    public Indicators indicators { get; set; }
}