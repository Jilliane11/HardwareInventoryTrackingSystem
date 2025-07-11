
using System.ComponentModel.DataAnnotations.Schema;
using HardwareInventoryTrackingSystem.Account;
using HardwareInventoryTrackingSystem.Commands;
using HardwareInventoryTrackingSystem.Interfaces;
using HardwareInventoryTrackingSystem.Pages.Account;
using HardwareInventoryTrackingSystem.Services;
using HardwareInventoryTrackingSystem.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Path to your login page
        options.AccessDeniedPath = "/Error"; // Redirect when access is denied
        options.LogoutPath = "/Account/Logout"; // Path for logout
    });

builder.Services.AddAuthorization();

builder.Services.Configure<RouteOptions>(o =>
{
    o.LowercaseUrls = true;
    o.LowercaseQueryStrings = true;
    o.AppendTrailingSlash = true;
});

string? conn = builder.Configuration.GetConnectionString("default");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(conn);
});

builder.Services.AddScoped<HashGenerator>();
builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<StudentRegistrationService>();
builder.Services.AddScoped<BorrowingFormService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<IPlaceFormAction, PlaceFormAction>();

builder.Services.AddProblemDetails();

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Ensure the login page is the first page loaded
app.MapGet("/", context =>
{
    context.Response.Redirect("/Account/Login");
    return Task.CompletedTask;
});

// Map Razor Pages and other routes
app.MapRazorPages();
app.MapFallbackToPage("/Account/Login");

#region ITEM
app.MapGet("/items", async (ItemService service) => await service.GetItems());

app.MapGet("/item/{id}", async (int id, ItemService service) =>
{
    ItemDetailViewModel name;
    try
    {
        name = await service.GetItem(id);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest("");
    }

    return Results.Ok(name);
}).WithName("view-item");

app.MapDelete("/item/delete/{id}", async (int id, ItemService service) =>
{
    try
    {
        await service.DeleteItem(id);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest("Id does not exist");
    }

    return Results.NoContent();
});

app.MapPut("/item/update/{id}", async (int id, UpdateItemCommand cmd, ItemService service) =>
{
    int updatedItemId = await service.UpdateItem(id, cmd);


    return TypedResults.CreatedAtRoute("view-item", new { id = updatedItemId });
});


app.MapPost("/item/create", async (CreateItemCommand cmd, ItemService service) =>
    {
        int id = await service.CreateItem(cmd);

        return TypedResults.CreatedAtRoute("view-item", new { id });

    }

);


#endregion

#region STUDENT
//app.MapGet("/students", async (StudentService service) => { return await service.GetStudents(); });

app.MapGet("/student/{id}", async (int id, StudentService service) =>
{
    StudentDetailViewModel name;
    try
    {
        name = await service.GetStudent(id);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest("");
    }

    return Results.Ok(name);
}).WithName("view-student");

app.MapDelete("/student/delete/{id}", async (int id, StudentService service) =>
{
    try
    {
        await service.DeleteStudent(id);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest("Id does not exist");
    }

    return Results.NoContent();
});

app.MapPut("/student/update/{id}", async (int id, UpdateStudentCommand cmd, StudentService service) =>
{
    int updatedStudentId = await service.UpdateStudent(id, cmd);


    return TypedResults.CreatedAtRoute("view-student", new { id = updatedStudentId });
});


app.MapPost("/student/create", async (CreateStudentCommand cmd, StudentService service) =>
    {
        int id = await service.CreateStudent(cmd);

        return TypedResults.CreatedAtRoute("view-student", new { id });

    }

);


#endregion

#region ADMIN
app.MapGet("/admin/{id}", async (int id, AdminService service) =>
{
    AdminViewModel name;
    try
    {
        name = await service.GetAdmin(id);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest("");
    }

    return Results.Ok(name);
}).WithName("view-admin");

app.MapDelete("/admin/delete/{id}", async (int id, AdminService service) =>
{
    try
    {
        await service.DeleteAdmin(id);
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest("Id does not exist");
    }

    return Results.NoContent();
});

//NOT WORKING

app.MapPut("/admin/update/{id}", async (int id, UpdateAdminCommand cmd, AdminService service) =>
{
    int updatedAdminId = await service.UpdateAdmin(id, cmd);


    return TypedResults.CreatedAtRoute("view-admin", new { id = updatedAdminId });
});


app.MapPost("/admin/create", async (CreateAdminCommand cmd, AdminService service) =>
    {
        int id = await service.CreateAdmin(cmd);

        return TypedResults.CreatedAtRoute("view-admin", new { id });

    }

);


#endregion

#region BORROWING-FORM
app.MapGet("/forms", async (BorrowingFormService service) => { return await service.GetForms(); });

app.MapGet("/form/{id}", async (int id, BorrowingFormService service) =>
{
    var form = await service.GetForm(id);
    return form is not null ? Results.Ok(form) : Results.NotFound();
}).WithName("view-form");


app.MapPost("/form/create", async (CreateBorrowingFormCommand cmd, BorrowingFormService service) =>
    {
        if (cmd.ItemList == null || !cmd.ItemList.Any())
        {
            return Results.BadRequest("No items selected.");
        }

        try
        {
            int id = await service.CreateForm(cmd);
            return Results.Created($"/form/{id}", id);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }

    }

);

#endregion


app.Run();


