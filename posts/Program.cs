using System.Collections;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();


var app = builder.Build();


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
    httpContext.Response.StatusCode = 201;
    await httpContext.Response.WriteAsJsonAsync(posts[id]);
});

app.Run();