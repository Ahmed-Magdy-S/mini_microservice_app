using System.Collections;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var httpClient = new HttpClient();


app.MapPost("/events", async httpContext => {
    
    var eventReceived = await JsonSerializer.DeserializeAsync<Hashtable>(httpContext.Request.Body);
    await httpClient.PostAsJsonAsync(new Uri("http://localhost:4000/events"),eventReceived); // for Posts service
    await httpClient.PostAsJsonAsync(new Uri("http://localhost:4001/events"),eventReceived); // for Comments service
    await httpClient.PostAsJsonAsync(new Uri("http://localhost:4002/events"),eventReceived); // for Query service

    await httpContext.Response.WriteAsJsonAsync(new Hashtable{["status"] = "Ok"});

});


app.Run();