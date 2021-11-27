namespace Karolinska.Reporting.SDK
{
    public partial class HealthcareProviderClient
    {
        partial void PrepareRequest(System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, string url)
        {
            if (request.Method.Equals(new System.Net.Http.HttpMethod("PATCH")))
            {
                request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");
            }
        }
    }
}
