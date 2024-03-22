namespace Api.WeatherModule.Services;

public class NullObjectResponse<TOutput>
{
    public TOutput? Value { get; }

    private HttpContext _context;

    public NullObjectResponse(TOutput? value, HttpContext context)
    {
        Value = value;
        _context = context;
    }

    public async Task ToResponse()
    {
        if (Value is null)
        {
            Console.WriteLine($"Requested resource not found: {_context.Request.Path}");
            _context.Response.StatusCode = 404;
            await _context.Response.WriteAsJsonAsync(new { Message = "Not found" });
        }
        else
        {
            await _context.Response.WriteAsJsonAsync(Value);
        }
    }
}