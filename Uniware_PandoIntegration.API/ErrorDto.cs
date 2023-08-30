using System.Text.Json;

internal class ErrorDto
{
    public string status { get; set; }
    public string message { get; set; }
    public string reason { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}