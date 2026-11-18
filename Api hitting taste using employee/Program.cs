using Api_hitting_taste_using_employee;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

class Program
{
    private static readonly HttpClient client = new();

    private static int successCount = 0;
    private static int failedCount = 0;

    static async Task Main()
    {
        Stopwatch sw = Stopwatch.StartNew();

        string token = await GetJwtToken();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        List<Task> tasks = new();

        for (int i = 1; i <= 5; i++)
        {
            int requestNo = i;
            tasks.Add(HitEmployeeApi(requestNo));
        }

        await Task.WhenAll(tasks);

        sw.Stop();

        Console.WriteLine("\n============ RESULT ============");
        Console.WriteLine($"Success : {successCount}");
        Console.WriteLine($"Failed  : {failedCount}");
        Console.WriteLine($"Total Time : {sw.ElapsedMilliseconds} ms");
    }

    static async Task<string> GetJwtToken()
    {
        var loginRequest = new
        {
            username = "sanskar@example.com",
            password = "sanskar@123"
        };

        string json =
            JsonSerializer.Serialize(loginRequest);

        StringContent content =
            new(json, Encoding.UTF8, "application/json");

        HttpResponseMessage response =
            await client.PostAsync(
                "https://localhost:7146/api/Auth/login",
                content);

        response.EnsureSuccessStatusCode();

        string responseJson =
            await response.Content.ReadAsStringAsync();

        using JsonDocument doc =
            JsonDocument.Parse(responseJson);

        string token =
            doc.RootElement
               .GetProperty("token")
               .GetString()!;

        return token;
    }

    static async Task HitEmployeeApi(int requestNo)
    {
        try
        {
            HttpResponseMessage response =
                await client.GetAsync(
                    "https://localhost:7146/api/Employees");

            if (response.IsSuccessStatusCode)
            {
                string json =
                    await response.Content.ReadAsStringAsync();

                var employees =
                    JsonSerializer.Deserialize<List<Employee>>(json);

                Interlocked.Increment(ref successCount);

                string employeeName =
                    employees?.FirstOrDefault()?.name ?? "No Employee";

                Console.WriteLine(
                    $"Request {requestNo} Success | Employee: {employeeName}");
            }
            else
            {
                Interlocked.Increment(ref failedCount);

                Console.WriteLine(
                    $"Request {requestNo} Failed | Status: {(int)response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Interlocked.Increment(ref failedCount);

            Console.WriteLine(
                $"Request {requestNo} Error | {ex.Message}");
        }
    }
}