using System.Text;
using System.Text.Json;

namespace FC.Codeflix.Catalog.EndToEndTests.Base;

public class ApiClient(HttpClient httpClient)
{
    public async Task<(HttpResponseMessage?, TOutput?)> Post<TOutput>(
        string route,
        object payload
    )
    {
        var response = await httpClient.PostAsync(
            route,
            new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            )
        );
        var outputString = await response.Content.ReadAsStringAsync();
        var output = JsonSerializer.Deserialize<TOutput>(outputString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }
        );
        return (response, output);
    }
}