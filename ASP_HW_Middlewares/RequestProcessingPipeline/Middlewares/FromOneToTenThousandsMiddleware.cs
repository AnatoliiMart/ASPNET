namespace RequestProcessingPipeline.Middlewares
{
    public class FromOneToTenThousandsMiddleware
    {
        private readonly RequestDelegate _next;

        public FromOneToTenThousandsMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                if (number < 1000)
                    await _next.Invoke(context);
                else
                {
                    string[] Thousands = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"];
                    string? res = string.Empty;

                    if (number > 11_000 && number < 20_000)
                    {
                        await _next.Invoke(context);
                        res = context.Session.GetString("number");
                        context.Session.SetString("number", res);
                    }
                    else if (number > 20_000)
                    {
                        number %= 10_000;
                        if (number % 1000 == 0)
                            context.Session.SetString("number", Thousands[number / 1000 - 1] + " thousands ");
                        else
                        {
                            await _next.Invoke(context);
                            res = context.Session.GetString("number");
                            context.Session.SetString("number", Thousands[number / 1000 - 1] + " thousands " + res);
                        }
                    }
                    else
                    {
                        if (number % 1000 == 0)
                            await context.Response.WriteAsync("Your number is " + Thousands[number / 1000 - 1] + " thousands ");
                        else
                        {
                            await _next.Invoke(context);
                            res = context.Session.GetString("number");
                            await context.Response.WriteAsync("Your number is " + Thousands[number / 1000 - 1] + " thousand" + " " + res);
                        }
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
