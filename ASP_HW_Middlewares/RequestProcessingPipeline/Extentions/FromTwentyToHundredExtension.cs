using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extentions
{
    public static class FromTwentyToHundredExtension
    {
        public static IApplicationBuilder UseFromTwentyToHundred(this IApplicationBuilder builder) =>
            builder.UseMiddleware<FromTwentyToHundredMiddleware>();
    }
}
