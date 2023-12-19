using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extentions
{
    public static class FromElevenToNineteenThousandsExtention
    {
        public static IApplicationBuilder UseFromElevenToNineteenThousands(this IApplicationBuilder builder) =>
            builder.UseMiddleware<FromElevenToNineteenThousandsMiddleware>();
    }
}
