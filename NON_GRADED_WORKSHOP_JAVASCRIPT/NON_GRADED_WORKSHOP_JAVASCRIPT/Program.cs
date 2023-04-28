using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Game/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Game}/{action=Index}/{id?}");

InitCache(app.Services);

app.Run();

void InitCache(IServiceProvider serviceProvider)
{
    var cache = serviceProvider.GetRequiredService<IMemoryCache>();

    // player 1 always starts first
    cache.Set("turn", "player1");

    // initialize the board to empty
    cache.Set("board", new char[,] {
        { ' ', ' ', ' ' },
        { ' ', ' ', ' ' },
        { ' ', ' ', ' ' },
    });
}

/*
When an HTTP request comes in, 
the app.MapControllerRoute middleware component is executed first to determine which controller and action method should handle the request. 
Once the controller and action method have been determined, 
the middleware pipeline moves on to the next middleware component in the pipeline, 
which may be a middleware component that uses the cache that was initialized by the InitCache method.

Once the InitCache method is finished executing, the middleware pipeline continues where it left off, 
which is the next middleware component after the InitCache method call in the code. 

The app.Run() method is the final middleware component in the ASP.NET Core middleware pipeline. 
It is the component that actually handles the incoming request and generates the HTTP response.

This method waits for an incoming HTTP request and then handles it by executing the appropriate controller and action method 
that was determined by the app.MapControllerRoute middleware component.

So, the process of generating the view goes like this:
(1) The app.MapControllerRoute middleware component maps the incoming request to the Index action method of the GameController class.

(2) The Index action method retrieves the data it needs from the in-memory cache (which was initialized by the InitCache method), 
    and passes that data to the view.

(3) The view is generated using the data passed to it from the action method, 
    and the resulting HTML is returned as the HTTP response to the client's browser.

So, in summary, the InitCache method initializes the cache used to store the state of the Tic Tac Toe game. 
The app.MapControllerRoute middleware component maps the incoming request to the appropriate controller and action method, 
and the Index action method of the GameController class generates the HTML view that is returned to the client's browser.
 */

