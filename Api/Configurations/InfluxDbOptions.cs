namespace Api.Configurations
{
    public class InfluxDbOptions
    {
        public string Url { get; init; }

        public string Token { get; init; }

        public string Org { get; init; }

        public string Bucket { get; init; }
    }
}