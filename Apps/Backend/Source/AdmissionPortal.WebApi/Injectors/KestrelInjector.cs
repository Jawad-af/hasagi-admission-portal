namespace AdmissionPortal.WebApi.Injectors
{
    public static class KestrelInjector
    {
        public static void InjectKestrel(this IWebHostBuilder builder, IConfiguration configuration)
        {
            Console.WriteLine("Started InjectKestrel");

            builder.ConfigureKestrel(serverOptions =>
            {
                serverOptions.Configure(configuration.GetSection("Kestrel"));
            });

            Console.WriteLine("Finished InjectKestrel");
        }
    }
}
