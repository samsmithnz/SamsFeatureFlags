namespace FeatureFlags.Web2.Data.Models
{
    public class Payload<T>
    {
        public Payload()
        {
            ServiceMessage = "";
            ServiceError = "";
        }

        public T Data { get; set; }
        public string ServiceURL { get; set; }
        public string ServiceMessage { get; set; }
        public string ServiceError { get; set; }
    }
}
