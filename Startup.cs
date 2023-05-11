using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketsAvailabilityAlerting.Models;
using TicketsAvailabilityAlerting.Services;


namespace TicketsAvailabilityAlerting
{
    public class Startup
    {
        public MailSettings MailSettings { get; private set; }
        public IMailService MailService { get; private set; }
        public ITextService TextService { get; private set; }


        public Startup()
        {
            // Set up IConfiguration.
            var builder = new ConfigurationBuilder()
                      .SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.json", optional: false);
            IConfiguration config = builder.Build();
            MailSettings = config.GetSection("MailSettings").Get<MailSettings>();
            
            
            // Set up Dependency Injection.
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ITextService, TextService>()
                .AddSingleton<IMailService, MailService>()
                .BuildServiceProvider();
            TextService = serviceProvider.GetService<ITextService>();
            MailService = serviceProvider.GetService<IMailService>();
        }

    } // End of Class
} // End of Namespace