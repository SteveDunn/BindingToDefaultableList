using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

var options = new MyImageProcessingOptions();

var config = builder.Build();
config.Bind("MyImageProcessingOptions", options);

Console.WriteLine(string.Join(",", options.ResizeWidths));

public class MyImageProcessingOptions
{
    public DefaultableCollection<int> ResizeWidths { get; } = new(100, 200, 400, 800);
}