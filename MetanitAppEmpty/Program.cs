using System.Text.RegularExpressions;


List<Person> users = new List<Person>
{
    new() {Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37},
    new() {Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 23},
    new() {Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 33}
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    string expressionForGuid = @"^/api/users/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";
    if (path == "/api/users" && request.Method == "GET")
    {
        await GetAllPeople(response);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        string? id = path.Value?.Split("/")[3];
        await GetPerson(id, response);
    }
    else if (path == "/api/users" && request.Method == "PUT")
    {
        await UpdatePerson(response, request);
    }
    else if (path == "/api/users" && request.Method == "POST")
    {
        await CreatePerson(response, request);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "DELETE")
    {
        string? id = path.Value?.Split("/")[3];
        await DeleteUser(id, response);
    }

});



app.Run();


async Task DeleteUser(string id, HttpResponse response)
{
    try
    {
        Person? user = users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            users.Remove(user);
        }
        else
        {
            throw new Exception("User not found(delete)");
        }
    }
    catch (Exception ex)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Wrong Data(delete)" });
    }
}

async Task CreatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        Person? user = await request.ReadFromJsonAsync<Person>();
        if (user != null)
        {
            user.Id = Guid.NewGuid().ToString();
            users.Add(user);
            await response.WriteAsJsonAsync(user);
        }
        else
        {
            throw new Exception("Wrong data(create)");
        }
    }
    catch (Exception ex)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Wrong data(create)" });
    }
}

async Task GetAllPeople(HttpResponse response)
{
    await response.WriteAsJsonAsync(users);
}
async Task GetPerson(string? id, HttpResponse response)
{
    Person? user = users.FirstOrDefault((u) => u.Id == id);

    if (user != null)
    {
        await response.WriteAsJsonAsync(user);
    }
    else
    {
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new {message = "User not found"});
    }
}

async Task UpdatePerson(HttpResponse response, HttpRequest request)
{
    try
    {
        Person? userData = await request.ReadFromJsonAsync<Person>();
        if (userData != null)
        {
            var user = users.FirstOrDefault(u => u.Id == userData.Id);

            if (user != null)
            {
                user.Age = userData.Age;
                user.Name = userData.Name;
                await response.WriteAsJsonAsync(user);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
    catch (Exception ex)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "Wrong data" });
    }
}

public class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
        
    public int Age { get; set; }
    
}
