using Newtonsoft.Json;

namespace Itixo.Timesheets.Shared.ErrorHandling;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }
    public string Detail { get; set; }


    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}
