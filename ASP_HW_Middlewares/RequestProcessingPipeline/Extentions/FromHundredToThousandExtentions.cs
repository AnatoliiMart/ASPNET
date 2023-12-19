using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extentions
{
    public static class FromHundredToThousandExtentions
    {
        public static IApplicationBuilder UseFromHundredToThousand(this IApplicationBuilder builder) =>
            builder.UseMiddleware<FromHundredToThousandMiddleware>();
    }
}
