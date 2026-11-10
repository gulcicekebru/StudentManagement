using Microsoft.EntityFrameworkCore;
using StudentManagement.Components;
using StudentManagement.Data;
using StudentManagement.Models;
using StudentManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StudentDbContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<StudentService>();

builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddInteractiveServerComponents();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

//Student student1 = new Student(1, "Ebru", "Türkmen", 78, 78);
//Student student2 = new Student(1, "Ayşe", "Yılmaz", 45, 100);
//Student student3 = new Student(1, "Ayşe", "Yılmaz", 45, 100);


//StudentService.AddStudent(student1);
//StudentService.AddStudent(student2);
//StudentService.AddStudent(student3);

app.Run();





