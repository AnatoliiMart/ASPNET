using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extentions
{
    public static class FromOneToTenExtension
    {
        public static IApplicationBuilder UseFromOneToTen(this IApplicationBuilder builder) =>
            builder.UseMiddleware<FromOneToTenMiddleware>();
    }
}
