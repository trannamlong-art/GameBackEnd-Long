namespace WebApplication1.Models;

public class ResponseApi
{
    public bool IsSuccess { get; set; }
    public string Notification { get; set; } = string.Empty;
    public object? Data { get; set; }

    public ResponseApi()
    {
        IsSuccess = false;
        Notification = "";
        Data = null;
    }
}
