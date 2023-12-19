namespace HW_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            app.UseStaticFiles();
            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html; charset=utf-8";
                await context.Response.SendFileAsync("wwwroot/index.html");
            });

            app.Run();
        }
    }
}
