namespace RequestProcessingPipeline.Middlewares
{
    public class FromTwentyToHundredThousandsMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTwentyToHundredThousandsMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                if (number < 20_000)
                    await _next.Invoke(context);
                else if (number == 100000)
                    await context.Response.WriteAsync("Your number is one hundred thousands");
                else
                {
                    string[] Tens = ["twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];

                    if (number % 10_000 == 0)
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10_000 - 2] + " thousands");
                    else
                    {
                        await _next.Invoke(context);
                        string? res = string.Empty;
                        res = context.Session.GetString("number");
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10_000 - 2] + " " + res);
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
