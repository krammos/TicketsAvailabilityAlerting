namespace TicketsAvailabilityAlerting
{
    internal class Program
    {
        private static readonly HttpClient httpClient = new();
        private static readonly Startup startup = new();
        private static string ticketsSiteHtmlCode = "";
        private static bool mailSent = false;
        private static int timerInSec;
        private static string url = "";
        private static string[]? arrayOfKeywords;
        private static string[]? arrayOfEmails;


        static void Main(string[] args)
        {
            try
            {
                // Show app general info.
                startup.TextService.ConsoleWriteIntroduction();

                // Checks for app's right usage.
                if (args.Length == 0)
                {
                    startup.TextService.ConsoleWriteNoProperUsage();
                    return; 
                }

                switch (args[0])
                {
                    case "--testingmode":
                        if (args.Length == 2 && args[1] == "--soundcheck")
                        {
                            startup.TextService.Beep();
                            startup.TextService.ConsoleWriteSuccess("Soundcheck is completed.");
                        }
                        else if (args.Length == 3 && args[1] == "--mailcheck" && !string.IsNullOrWhiteSpace(args[2]))
                        {
                            // Get values from parameters.
                            arrayOfEmails = Array.ConvertAll(args[2].Split(','), p => p.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                            
                            startup.MailService.SendMail(arrayOfEmails, startup.MailSettings);
                            startup.TextService.ConsoleWriteSuccess("Mailcheck is completed.");
                        }
                        else
                        {
                            startup.TextService.ConsoleWriteNoProperUsage();
                        }
                        break;
                
                    case "--normalmode":
                        if ( args.Length == 9 && 
                             args.Contains("--url") &&
                             !string.IsNullOrWhiteSpace(args[Array.IndexOf(args, "--url") + 1]) &&
                             args.Contains("--emails") &&
                             !string.IsNullOrWhiteSpace(args[Array.IndexOf(args, "--emails") + 1]) &&
                             args.Contains("--keywords") &&
                             !string.IsNullOrWhiteSpace(args[Array.IndexOf(args, "--keywords") + 1]) &&
                             args.Contains("--timer") &&
                             int.TryParse(args[Array.IndexOf(args, "--timer") + 1], out timerInSec)
                           )
                        {
                            // Get values from parameters.
                            url = args[Array.IndexOf(args, "--url") + 1];
                            arrayOfEmails = Array.ConvertAll(args[Array.IndexOf(args, "--emails") + 1].Split(','), p => p.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                            arrayOfKeywords = Array.ConvertAll(args[Array.IndexOf(args, "--keywords") + 1].Split(','), p => p.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();

                            // Create and initiate the timer.
                            Timer t = new(TimerCallback, null, 0, 1000 * timerInSec);

                            startup.TextService.ConsoleWriteExit();
                            while (Console.Read() != 'q');
                        }
                        else
                        {
                            startup.TextService.ConsoleWriteNoProperUsage();
                        }
                        break;
                
                    default:
                        startup.TextService.ConsoleWriteNoProperUsage();
                        break;
                }
            }
            catch (Exception ex)
            {
                startup.TextService.ConsoleWriteException(ex);
            }
        }


        private static async void TimerCallback(object? sender)
        {
            try
            {
                // Download webpage code.
                ticketsSiteHtmlCode = await httpClient.GetStringAsync(url);

                // Check if there are any keywords inside the page.
                // If so, (1) write success in console, (2) make beep and (3) send email.
                if (arrayOfKeywords.Any(ticketsSiteHtmlCode.Contains))
                {
                    startup.TextService.ConsoleWriteSuccess("Tickets are available to the public.");
                    startup.TextService.Beep();
                    
                    if (!mailSent)
                    {
                        startup.MailService.SendMail(arrayOfEmails, startup.MailSettings);
                        mailSent = true;
                    }
                }
                else
                {
                    startup.TextService.ConsoleWriteFailure();
                }
            }
            catch (Exception ex)
            {
                startup.TextService.ConsoleWriteException(ex);
            }
        }


    } // End of Class
} // End of Namespace