using Bill_Master.ApplicationContext;
using Bill_Master.Interfaces;
using Bill_Master.Model;
using Bill_Master.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ISoftwareSettings, SoftwareSettingsRepository>();
builder.Services.AddScoped<IFinancialYear, FinancialYearRepository>();
builder.Services.AddScoped<IStaffMaster, StaffMasterRepository>();
builder.Services.AddScoped<IProductCategory, ProductCategoryRepository>();
builder.Services.AddScoped<IVendor, VendorRepository>();
builder.Services.AddScoped<IClientMaster, ClientMasterRepository>();
builder.Services.AddScoped<IProductMaster, ProductMasterRepository>();
builder.Services.AddScoped<IPurchaseMaster, PurchaseMasterRepository>();
builder.Services.AddScoped<IPurchaseItems, PurchaseItemsRepository>();
builder.Services.AddScoped<IPurchasePayment, PurchasePaymentRepository>();
builder.Services.AddScoped<IInwardStock, InwardStockRepository>();
builder.Services.AddScoped<IOutward, OutwardRepository>();
builder.Services.AddScoped<IStockUsed, StockUsedRepository>();
builder.Services.AddScoped<IInvoiceMaster, InvoiceMasterRepository>();
builder.Services.AddScoped<IInvoiceItems, InvoiceItemsRepository>();
builder.Services.AddScoped<IInvoicePayment, InvoicePaymentRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<StockRepository>();

builder.Services.AddScoped<EmailService>();

//builder.Services.AddDbContext<ApplicationDBContext>(o => o.UseSqlServer("Data Source=apnatiffin.cn62q6e8yukm.ap-south-1.rds.amazonaws.com;Initial Catalog=BillMasterDB;Persist Security Info=True;User ID=admin;Password=chand_2026;Trust Server Certificate=True"));

builder.Services.AddDbContext<ApplicationDBContext>(o => o.UseSqlServer("Data Source=.;Initial Catalog=BillMasterDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"));
builder.Services.AddControllers();

builder.Services.AddOpenApi();

/* ================= CORS ================= */

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

/* ================= USE CORS ================= */



app.UseAuthorization();

app.MapControllers();

app.UseCors("ReactPolicy");

app.Run();