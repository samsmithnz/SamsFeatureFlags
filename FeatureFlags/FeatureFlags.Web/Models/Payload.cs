namespace FeatureFlags.Web.Models
{
    public class Payload<T>
    {
        public Payload()
        {
            Message = "";
            Error = "";
        }

        public T Data { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }
}
