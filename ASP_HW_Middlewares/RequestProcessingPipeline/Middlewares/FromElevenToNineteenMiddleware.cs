namespace RequestProcessingPipeline.Middlewares
{
    public class FromElevenToNineteenMiddleware
    {
        private readonly RequestDelegate _next;

        public FromElevenToNineteenMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            string? token = context.Request.Query["number"];
            try
            {
                int number = Convert.ToInt32(token);
                number = Math.Abs(number);
                string[] Numbers = ["eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen"];

                if (number < 11 || number > 19)
                    await _next.Invoke(context);
                else
                    await context.Response.WriteAsync("Your number is " + Numbers[number - 11]);
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("Incorrect parameter");
            }
        }
    }
}
