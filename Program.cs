using Microsoft.EntityFrameworkCore;
using TodoAPI;

var builder = WebApplication.CreateBuilder(args);

// Add DI - AddServices
builder.Services.AddDbContext<TodoDB>(opt => opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

// Configure pipeline - Use and Map Methods
app.MapGet("/todoitems", async (TodoDB db) =>
    await db.Todos.ToListAsync());

app.MapGet("/todoitems/{id}", async (TodoDB db, int id) =>
    await db.Todos.FindAsync(id));

app.MapGet("/todoitemByname/{name}", async (TodoDB db, string name) =>
    await db.Todos
    .Where(x => x.Name == name)
    .FirstOrDefaultAsync());

app.MapGet("/todoitemsByBoolean/{state}", async (TodoDB db, bool state) =>
     await db.Todos
        .Where(x => x.IsComplete == state)
        .ToListAsync());

app.MapPost("/todoitems", async (TodoDB db, TodoItem todo) =>
{
    db.Todos.Add(todo);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todo.Id}", todo);
});

app.MapPut("/todoitems/{id}", async (TodoDB db, int id, TodoItem inputTodo) =>
{
    var todo = await db.Todos.FindAsync(id);
    if(todo == null) return Results.NotFound();
    todo.Name = inputTodo.Name;
    todo.IsComplete = inputTodo.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/todoitems/{id}", async (TodoDB db, int id) =>
{
    if(await db.Todos.FindAsync(id) is TodoItem todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});

app.Run();
