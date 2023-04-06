using System.Collections;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});

var posts = new Hashtable();


app.MapGet("/posts", async (HttpRequest req, HttpResponse res) =>
{
    await res.WriteAsJsonAsync(posts);

});

app.MapPost("/events", async (HttpRequest req, HttpResponse res) =>
{
    Console.WriteLine("Event received in Query");
    var body = JsonSerializer.Deserialize<Hashtable>(await (new StreamReader(req.Body).ReadToEndAsync())); ;
   

    string type = body["type"].ToString();
    Hashtable? data = JsonSerializer.Deserialize<Hashtable>(body["data"].ToString());


    if (type == "PostCreated")
    {
        var id = data["id"].ToString();
        string title = data["title"].ToString();

        posts[id] = new Hashtable { ["title"] = title, ["id"] = id, ["comments"] = new ArrayList() };
    }

    if (body["type"].ToString() == "CommentCreated")
    {

        var id = data["id"].ToString();
        var postId = body["postId"].ToString();
        string content = data["content"].ToString();

        var post = posts[postId] as Hashtable;

        ((ArrayList)post["comments"]).Add(new Hashtable { ["id"] = id, ["content"] = content });
    }

    Console.WriteLine("ok");
});






app.Run();