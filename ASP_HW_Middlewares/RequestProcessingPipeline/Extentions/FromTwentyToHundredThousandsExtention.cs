using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extentions
{
    public static class FromTwentyToHundredThousandsExtention
    {
        public static IApplicationBuilder UseFromTwentyToHundredThousands(this IApplicationBuilder builder) =>
            builder.UseMiddleware<FromTwentyToHundredThousandsMiddleware>();
    }
}
