using AFRA.Members;
using AFRA.Members.database;
using LinqToDB;
using LinqToDB.Data;

DataConnection.DefaultSettings = new Database.Settings();

Migrations.RunMigrations();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSession(options => {
	options.IdleTimeout        = TimeSpan.MaxValue;
	options.Cookie.HttpOnly    = true;
	options.Cookie.IsEssential = true;
});

#if (DEBUG)
builder.Services.AddControllers().AddRazorRuntimeCompilation();
builder.Services.AddControllers();
#else
builder.Services.AddControllers();
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
	endpoints.MapRazorPages();
	endpoints.MapControllers();
});

app.Run();
