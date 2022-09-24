//using MetanitAppEmpty;
//using System.Text;
//using System.Text.RegularExpressions;

var users = new List<Person>
{
    new() { Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 41 },
    new() { Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 24 }
};


var builder = WebApplication.CreateBuilder();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/users", () =>
{
	return users;
});

app.MapGet("/api/users/{id}", (string id) =>
{
	Person? user = users.Find(x => x.Id == id);

	if (user == null) return Results.NotFound(new { message = "User not found" });

	return Results.Json(user);
});

app.MapDelete("/api/users/{id}", (string id) =>
{
	Person? user = users.Find(users => users.Id == id);

	if (user == null) return Results.NotFound(new { message = "User does not exist" });

	users.Remove(user);
	return Results.Json(user);
});

app.MapPost("/api/users/", (Person user) =>
{
	user.Id = Guid.NewGuid().ToString();
	users.Add(user);
	return Results.Json(user);
});

app.MapPut("/api/users", (Person userData) =>
{
	Person? user = users.Find(users => users.Id == userData.Id);

	if (user == null) return Results.NotFound(new { message = "User does not exist" });

	user.Name = userData.Name;
	user.Age = userData.Age;

	return Results.Json(user);
});

app.Run();
public class Person
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Age { get; set; }
}

//var builder = WebApplication.CreateBuilder(
//        new WebApplicationOptions { WebRootPath = "wwwroot"}
//    );

////builder.Services.Configure<RouteOptions>(options =>
////    options.ConstraintMap.Add("secretcode", typeof(SecretCodeConstraint)));

//// альтернативное добавление класса ограничения
////builder.Services.AddRouting(options => options.ConstraintMap.Add("invalidnames", typeof(InvalidNamesConstraint)));

//builder.Services.AddTransient<ITimer, Timer>();
//builder.Services.AddTransient<TimeService>();
////builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
////{
////    {"name", "Tom" },
////    {"age", "Surv" }
////});



//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.Cookie.Name = ",MyApp.Session";
//    options.IdleTimeout = TimeSpan.FromSeconds(3600);
//    options.Cookie.IsEssential = true;
//});

//builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

//builder.Configuration.AddJsonFile("config.json");

//var app = builder.Build();

//app.UseStatusCodePages(async statusCodeContext =>
//{
//    var response = statusCodeContext.HttpContext.Response;
//    var path = statusCodeContext.HttpContext.Request.Path;

//    response.ContentType = "text/plain; charset=utf-8";
//    if (response.StatusCode == 403)
//    {
//        await response.WriteAsync($"Path: {path}. Access Denied");
//    }
//    else if (response.StatusCode == 404)
//    {
//        await response.WriteAsync($"Resource {path} Not found");
//    }
//}); //http error handler


//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler(app => app.Run(async context =>
//    {
//        context.Response.StatusCode = 500;
//        await context.Response.WriteAsync($"Error 500. DivideByZeroException occurred! Path: {context.Request.Path}");
//    }));
//}

//app.UseSession();
//app.UseStaticFiles();

////app.Run(async (context) =>
////{
////    app.Logger.LogInformation($"Path: {context.Request.Path}  Time:{DateTime.Now.ToLongTimeString()}");
////    app.Logger.Log(LogLevel.Warning, "TEST");
////    await context.Response.WriteAsync("Hello World!");
////});

//app.Map("/upload", async () =>
//{
//    string path = "uploads/External_service.png";
//    byte[] fileContent = await File.ReadAllBytesAsync(path);
//    string contentType = "image/png";
//    string downloadName = "bred.png";
//    return Results.File(fileContent, contentType, downloadName);
//});

//app.Map("/hello", () => Results.Text("Hello world"));

//app.Map("/", (ILogger<Program> logger) =>
//{
//    logger.Log(LogLevel.Warning, $"Path: / {DateTime.Now.ToShortTimeString()}");
//    return "Hello";
//});


//app.Map("/time", (TimeService service) => $"Time: {service.GetTime()}");

//app.Run();


////app.Map("/users/{name:invalidnames}", (string name) => $"Name: {name}");

////app.Map(
////        "/users/{name}/{token:secretcode(123466)}/",
////        (string name, int token) => $"Name: {name} \nToken: {token}"
////    );
////app.Map(
////    "/phonebook/{phone:regex(^7-\\d{{3}}-\\d{{3}}-\\d{{4}}$)}/",
////    (string phone) => $"Phone: {phone}"
////);
////app.Map("/", () => "Index Page");


////app.MapGet("/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
////{
////    var sb = new StringBuilder();
////    var endpoints = endpointSources.SelectMany(es => es.Endpoints);
////    foreach (var endpoint in endpoints)
////    {
////        sb.AppendLine(endpoint.DisplayName);

////        // получим конечную точку как RouteEndpoint
////        if (endpoint is RouteEndpoint routeEndpoint)
////        {
////            sb.AppendLine(routeEndpoint.RoutePattern.RawText);
////        }

////        // получение метаданных
////        // данные маршрутизации
////        var routeNameMetadata = endpoint.Metadata.OfType<Microsoft.AspNetCore.Routing.RouteNameMetadata>().FirstOrDefault();
////        var routeName = routeNameMetadata?.RouteName;
////        // данные http - поддерживаемые типы запросов
////        var httpMethodsMetadata = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault();
////        var httpMethods = httpMethodsMetadata?.HttpMethods; // [GET, POST, ...]
////    }
////    return sb.ToString();
////});


//public class InvalidNamesConstraint : IRouteConstraint
//{
//    string[] names = new[] { "Tom", "Sam", "Bob" };
//    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey,
//        RouteValueDictionary values, RouteDirection routeDirection)
//    {
//        return !names.Contains(values[routeKey]?.ToString());
//    }
//}

//public class SecretCodeConstraint : IRouteConstraint
//{
//    string secretCode;

//    public SecretCodeConstraint(string secretCode)
//    {
//        this.secretCode = secretCode;
//    }

//    public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
//    {
//        return values[routeKey]?.ToString() == secretCode;
//    }
//}

////List<Person> users = new List<Person>
////{
////    new() {Id = Guid.NewGuid().ToString(), Name = "Tom", Age = 37},
////    new() {Id = Guid.NewGuid().ToString(), Name = "Bob", Age = 23},
////    new() {Id = Guid.NewGuid().ToString(), Name = "Sam", Age = 33}
////};

////var builder = WebApplication.CreateBuilder(args);

////Dva objekta dlja odnoj zavisimosti!!!!!!!!!!!!
////builder.Services.AddTransient<IHelloService, RuHelloService>();
////builder.Services.AddTransient<IHelloService, EnHelloService>();

////var app = builder.Build();

////app.UseMiddleware<HelloMiddleware>();

////Odin object dlja neskolkih zavisimostrej!!!!!!!!!!!!!!
////builder.Services.AddSingleton<ValueStorage>();
////builder.Services.AddSingleton<IGenerator>(service => service.GetRequiredService<ValueStorage>());
////builder.Services.AddSingleton<IReader>(service => service.GetRequiredService<ValueStorage>());

////var app = builder.Build();

////app.UseMiddleware<GeneratorMiddleware>();
////app.UseMiddleware<ReaderMiddleware>();

////app.Run(async (context) => await context.Response.WriteAsync("Metanit"));

////app.Run();

//class GeneratorMiddleware
//{
//    RequestDelegate next;
//    IGenerator generator;

//    public GeneratorMiddleware(RequestDelegate next, IGenerator generator)
//    {
//        this.next = next;
//        this.generator = generator;
//    }
//    public async Task InvokeAsync(HttpContext context)
//    {
//        if (context.Request.Path == "/generate")
//            await context.Response.WriteAsync($"New Value: {generator.GenerateValue()}");
//        else
//            await next.Invoke(context);
//    }
//}
//class ReaderMiddleware
//{
//    IReader reader;

//    public ReaderMiddleware(RequestDelegate _, IReader reader) => this.reader = reader;

//    public async Task InvokeAsync(HttpContext context)
//    {
//        await context.Response.WriteAsync($"Current Value: {reader.ReadValue()}");
//    }
//}

//interface IGenerator
//{
//    int GenerateValue();
//}
//interface IReader
//{
//    int ReadValue();
//}
//class ValueStorage : IGenerator, IReader
//{
//    int value;
//    public int GenerateValue()
//    {
//        value = new Random().Next();
//        return value;
//    }

//    public int ReadValue() => value;
//}

//interface IHelloService
//{
//    string Message { get; }
//}

//class RuHelloService : IHelloService
//{
//    public string Message => "Привет METANIT.COM";
//}
//class EnHelloService : IHelloService
//{
//    public string Message => "Hello METANIT.COM";
//}

//class HelloMiddleware
//{
//    readonly IEnumerable<IHelloService> helloServices;

//    public HelloMiddleware(RequestDelegate _, IEnumerable<IHelloService> helloServices)
//    {
//        this.helloServices = helloServices;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        context.Response.ContentType = "text/html; charset=utf-8";
//        string responseText = "";
//        foreach (var service in helloServices)
//        {
//            responseText += $"<h3>{service.Message}</h3>";
//        }
//        await context.Response.WriteAsync(responseText);
//    }
//}

//public interface ITimer
//{
//    string Time { get; }
//}
//public class Timer : ITimer
//{
//    public Timer()
//    {
//        Time = DateTime.Now.ToLongTimeString();
//    }
//    public string Time { get; }
//}
//public class TimeService
//{
//    private ITimer timer;
//    public TimeService(ITimer timer)
//    {
//        this.timer = timer;
//    }
//    public string GetTime() => timer.Time;
//}

//public interface ICounter
//{
//    int Value { get; }
//}

//public class RandomCounter : ICounter
//{
//    static Random rnd = new Random();
//    private int _value;
//    public RandomCounter()
//    {
//        _value = rnd.Next(0, 1000000);
//    }
//    public int Value
//    {
//        get => _value;
//    }
//}

//public class CounterService
//{
//    public ICounter Counter { get;}

//    public CounterService(ICounter counter)
//    {
//        Counter = counter;
//    }
//}

//public class CounterMiddleware
//{
//    RequestDelegate next;
//    int i = 0;
//    public CounterMiddleware(RequestDelegate next)
//    {
//        this.next = next;
//    }

//    public async Task InvokeAsync(HttpContext httpContext, ICounter counter, CounterService counterService)
//    {
//        i++;
//        httpContext.Response.ContentType = "text/html;charset=utf-8";
//        await httpContext.Response.WriteAsync($"Request {i}; Counter: {counter.Value}; Service: {counterService.Counter.Value}");
//    }
//}

////async Task DeleteUser(string id, HttpResponse response)
////{

////        Person? user = users.FirstOrDefault(u => u.Id == id);
////        if (user != null)
////        {
////            users.Remove(user);
////            await response.WriteAsJsonAsync(user);
////        }
////        else
////        {
////            response.StatusCode = 400;
////            await response.WriteAsJsonAsync(new { message = "Wrong Data(delete)" });
////        }

////}

////async Task CreatePerson(HttpResponse response, HttpRequest request)
////{
////    try
////    {
////        Person? user = await request.ReadFromJsonAsync<Person>();
////        if (user != null)
////        {
////            user.Id = Guid.NewGuid().ToString();
////            users.Add(user);
////            await response.WriteAsJsonAsync(user);
////        }
////        else
////        {
////            throw new Exception("Wrong data(create)");
////        }
////    }
////    catch (Exception ex)
////    {
////        response.StatusCode = 400;
////        await response.WriteAsJsonAsync(new { message = "Wrong data(create)" });
////    }
////}

////async Task GetAllPeople(HttpResponse response)
////{
////    await response.WriteAsJsonAsync(users);
////}
////async Task GetPerson(string? id, HttpResponse response)
////{
////    Person? user = users.FirstOrDefault((u) => u.Id == id);

////    if (user != null)
////    {
////        await response.WriteAsJsonAsync(user);
////    }
////    else
////    {
////        response.StatusCode = 404;
////        await response.WriteAsJsonAsync(new {message = "User not found"});
////    }
////}

////async Task UpdatePerson(HttpResponse response, HttpRequest request)
////{
////    try
////    {
////        Person? userData = await request.ReadFromJsonAsync<Person>();
////        if (userData != null)
////        {
////            var user = users.FirstOrDefault(u => u.Id == userData.Id);

////            if (user != null)
////            {
////                user.Age = userData.Age;
////                user.Name = userData.Name;
////                await response.WriteAsJsonAsync(user);
////            }
////            else
////            {
////                response.StatusCode = 404;
////                await response.WriteAsJsonAsync(new { message = "User not found" });
////            }
////        }
////        else
////        {
////            throw new Exception("Wrong data(update)");
////        }
////    }
////    catch (Exception ex)
////    {
////        response.StatusCode = 400;
////        await response.WriteAsJsonAsync(new { message = "Wrong data" });
////    }
////}

////public class Person
////{
////    public string Id { get; set; } = "";
////    public string Name { get; set; } = "";

////    public int Age { get; set; }

////}
