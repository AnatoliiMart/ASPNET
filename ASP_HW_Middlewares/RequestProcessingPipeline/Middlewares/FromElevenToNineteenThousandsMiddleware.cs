namespace RequestProcessingPipeline.Middlewares
{
    public class FromElevenToNineteenThousandsMiddleware
    {
        private readonly RequestDelegate _next;
        public FromElevenToNineteenThousandsMiddleware(RequestDelegate next) => _next = next;
        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                if (number < 11000 || number > 19999)
                    await _next.Invoke(context);
                else
                {
                    string[] Numbers = ["eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];

                    if (number % 1000 == 0)
                        await context.Response.WriteAsync("Your number is " + Numbers[number / 1000 - 11] + " thousands");
                    else
                    {
                        await _next.Invoke(context);
                        string? res = string.Empty;
                        res = context.Session.GetString("number");
                        await context.Response.WriteAsync("Your number is " + Numbers[number / 1000 - 11] + " thousands " + res);
                    }
                }
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}
