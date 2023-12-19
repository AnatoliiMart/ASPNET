namespace RequestProcessingPipeline.Middlewares
{
    public class FromTwentyToHundredMiddleware
    {
        private readonly RequestDelegate _next;

        public FromTwentyToHundredMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);

                string[] Tens = ["twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"];
                string? res = string.Empty;

                if (number < 20)
                    await _next.Invoke(context);
                else if (number > 100)
                {
                    number %= 100;

                    if (number % 10 == 0)
                        context.Session.SetString("number", Tens[number / 10 - 2]);
                    else if (number / 10 != 0 && number / 10 != 1)
                    {
                        await _next.Invoke(context);
                        res = context.Session.GetString("number");
                        context.Session.SetString("number", Tens[number / 10 - 2] + " " + res);
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

                    if (number % 10 == 0)
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2]);
                    else
                    {
                        await _next.Invoke(context);
                        res = context.Session.GetString("number");
                        await context.Response.WriteAsync("Your number is " + Tens[number / 10 - 2] + " " + res);
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
