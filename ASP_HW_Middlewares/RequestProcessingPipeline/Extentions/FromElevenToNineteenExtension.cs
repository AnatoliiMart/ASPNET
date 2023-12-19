using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extentions
{
    public static class FromElevenToNineteenExtension
    {
        public static IApplicationBuilder UseFromElevenToNineteen(this IApplicationBuilder builder) =>
            builder.UseMiddleware<FromElevenToNineteenMiddleware>();
    }
}
