using RequestProcessingPipeline.Middlewares;

namespace RequestProcessingPipeline.Extentions
{
    public static class FromOneToTenThousandsExtention
    {
        public static IApplicationBuilder UseFromOneToTenThousands(this IApplicationBuilder builder) =>
           builder.UseMiddleware<FromOneToTenThousandsMiddleware>();
    }
}
