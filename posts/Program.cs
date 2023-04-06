using System.Collections;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();


var app = builder.Build();

HttpClient httpClient = new ();


app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod() );

Hashtable posts = new();

app.MapGet("/posts",  async httpContext => {
    await httpContext.Response.WriteAsJsonAsync(posts);
});

app.MapPost("/posts", async httpContext => {
    var id = Guid.NewGuid();
    var post = await JsonSerializer.DeserializeAsync<Hashtable>(httpContext.Request.Body)?? new Hashtable();
    post["id"] = id;
    //Add every post to posts 
    posts[id] = post;

    var eventEmitted = new Hashtable
    {
        ["type"] = "PostCreated",
        ["data"] = posts[id]
    };

    await httpClient.PostAsJsonAsync("http://localhost:4005/events", eventEmitted);

    httpContext.Response.StatusCode = 201;
    await httpContext.Response.WriteAsJsonAsync(posts[id]);
});

app.MapPost("/events", async httpContext =>
{
    try
    {
     //   await httpContext.Response.WriteAsJsonAsync(httpContext.Request.Body);
        Console.WriteLine("event Reveived");
    }
    catch (Exception ex)
    {
        Console.WriteLine("error");

    }
});


app.Run();