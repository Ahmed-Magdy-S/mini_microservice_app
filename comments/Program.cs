using System.Collections;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod() );

//will be like this:
// {
//   "329428ggsId" : [{id: "24er3f34g" , content:"some content here"},comment2,...]
//   "g23g48ggsId" : [comment1,comment2,comment3]
// }
var commentsByPostId = new Hashtable();

//get all comments of specific id
app.MapGet("/posts/{id}/comments", async (httpContext) =>
{
    var id = httpContext.Request.RouteValues["id"];
    var comments = id != null && commentsByPostId.Contains(id)? (ArrayList)(commentsByPostId[id]) : new ArrayList();
    await httpContext.Response.WriteAsJsonAsync(comments);
});

app.MapPost("/posts/{id}/comments", async context => {
    var id = Guid.NewGuid();
    var comment = await JsonSerializer.DeserializeAsync<Hashtable>(context.Request.Body)?? new Hashtable();
    comment["id"] = id;

    var postId = context.Request.RouteValues["id"];

    var comments = postId != null && commentsByPostId.Contains(postId) ? (ArrayList)commentsByPostId[postId] : new ArrayList();

    comments.Add(comment);

    commentsByPostId[postId] = comments;

    context.Response.StatusCode = 201;
    await context.Response.WriteAsJsonAsync(commentsByPostId);
});



app.Run();