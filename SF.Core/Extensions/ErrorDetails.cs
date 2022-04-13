using Newtonsoft.Json;

namespace SF.Core.Extensions
{
    public class ErrorDetails
    {
        public string Message { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
