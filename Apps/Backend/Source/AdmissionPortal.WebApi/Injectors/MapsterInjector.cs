using Mapster;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class MapsterInjector
    {
        public static void InjectMapster(this IServiceCollection services)
        {
            Console.WriteLine("Started InjectMapster");

            TypeAdapterConfig.GlobalSettings.Default
                .ShallowCopyForSameType(true)   // help with circular refs
                .PreserveReference(true);

            Console.WriteLine("Finished InjectMapster");
        }
    }
}
