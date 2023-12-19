namespace RequestProcessingPipeline.Middlewares
{
    public class FromHundredToThousandMiddleware
    {
        private readonly RequestDelegate _next;

        public FromHundredToThousandMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                string[] hndrds = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
                string? res = string.Empty;

                if (number < 100)
                    await _next.Invoke(context);
                else if (number > 1000)
                {
                    number %= 1000;

                    if (number % 100 == 0)
                        context.Session.SetString("number", hndrds[number / 100 - 1] + " hundred");
                    else if (number / 100 != 0)
                    {
                        await _next.Invoke(context);
                        res = context.Session.GetString("number");
                        context.Session.SetString("number", hndrds[number / 100 - 1] + " hundred " + res);
                    }
                    else
                    {
                        await _next.Invoke(context);
                        res = context.Session.GetString("number");
                        context.Session.SetString("number", res);
                    }
                }
                else
                {
                    if (number % 100 == 0)
                        await context.Response.WriteAsync("Your number is " + hndrds[number / 100 - 1] + " hundred");
                    else
                    {
                        await _next.Invoke(context);
                        res = context.Session.GetString("number");
                        await context.Response.WriteAsync("Your number is " + hndrds[number / 100 - 1] + " hundred " + res);

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
