using System.Text;
using System.Net;
using System.Net.Mail;


namespace TicketsAvailabilityAlerting
{
    internal class Program
    {
        private static WebClient client = new();
        private static string[]? arrayOfKeywords;
        private static string[]? arrayOfEmails;
        private static string url = "";
        private static string ticketsSiteHtmlCode = "";
        private static bool mailSent = false;
        private static int timerInSec;


        static void Main(string[] args)
        {
            try
            {
                ConsoleWriteIntroduction();

                if (args.Length == 0)
                {
                    ConsoleWriteNoProperUsage();
                    return; 
                }

                switch (args[0])
                {
                    case "--testingmode":
                        if (args.Length == 2 && args[1] == "--soundcheck")
                        {
                            Beep();
                            ConsoleWriteSuccess("Soundcheck is completed.");
                        }
                        else if (args.Length == 3 && args[1] == "--mailcheck" && !string.IsNullOrWhiteSpace(args[2]))
                        {
                            arrayOfEmails = Array.ConvertAll(args[2].Split(','), p => p.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                            SendMail();
                            ConsoleWriteSuccess("Mailcheck is completed.");
                        }
                        else
                        {
                            ConsoleWriteNoProperUsage();
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
                            arrayOfKeywords = Array.ConvertAll(args[Array.IndexOf(args, "--keywords") + 1].Split(','), p => p.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                            arrayOfEmails = Array.ConvertAll(args[Array.IndexOf(args, "--emails") + 1].Split(','), p => p.Trim()).Where(i => !string.IsNullOrEmpty(i)).ToArray();
                            url = args[Array.IndexOf(args, "--url") + 1];

                            Timer t = new(TimerCallback, null, 0, 1000 * timerInSec);
                            
                            ConsoleWriteExit();
                            while (Console.Read() != 'q');
                        }
                        else
                        {
                            ConsoleWriteNoProperUsage();
                        }
                        break;
                
                    default:
                        ConsoleWriteNoProperUsage();
                        break;
                }
            }
            catch (Exception ex)
            {
                ConsoleWriteException(ex);
            }
        }


        private static void TimerCallback(object? sender)
        {
            try
            {
                ticketsSiteHtmlCode = client.DownloadString(url);

                if (arrayOfKeywords.Any(ticketsSiteHtmlCode.Contains))
                {
                    ConsoleWriteSuccess("Tickets are available to the public.");
                    Beep();
                    
                    if (!mailSent)
                    {
                        SendMail();
                        mailSent = true;
                    }
                }
                else
                {
                    ConsoleWriteFailure();
                }
            }
            catch (Exception ex)
            {
                ConsoleWriteException(ex);
            }
        }


        private static void SendMail()
        {
            using MailMessage mail = new();

            // From
            mail.From = new MailAddress("konstantinos.rammos@haf.gr", "TicketsAvailabilityAlerting");

            // To
            foreach (var email in arrayOfEmails)
            {
                mail.To.Add(email);
            }

            // Subject
            mail.Subject = "----- Email by TicketsAvailabilityAlerting App -----";

            // Body
            StringBuilder template = new();
            template.AppendLine("Tickets are available to the public!");
            mail.IsBodyHtml = false;
            mail.Body = template.ToString();

            // SMTP settings
            using SmtpClient smtp = new("mail.haf.gr", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("kr@haf.gr", "12345");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }


        private static void Beep()
        {
            for (var i = 0; i < 5; i++)
                Console.Beep();
        }


        private static void ConsoleWriteIntroduction()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("***********************************************************************************************************");
            Console.WriteLine("*                                                                                                         *");
            Console.WriteLine("*                                    TICKETS AVAILABILITY ALERTING APP                                    *");
            Console.WriteLine("*                                                                                                 v1.2.0  *");
            Console.WriteLine("***********************************************************************************************************");
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine("     TicketsAvailabilityAlerting --testingmode --soundcheck");
            Console.WriteLine();
            Console.WriteLine("     TicketsAvailabilityAlerting --testingmode --mailcheck <comma-separated-users-emails>");
            Console.WriteLine();
            Console.WriteLine("     TicketsAvailabilityAlerting --normalmode");
            Console.WriteLine("                                 --url <URL>");
            Console.WriteLine("                                 --timer <timer-in-seconds>");
            Console.WriteLine("                                 --keywords <comma-separated-search-keywords>");
            Console.WriteLine("                                 --emails <comma-separated-users-emails>");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("     --testingmode                Testing mode of the app (this option always goes first).");
            Console.WriteLine("     --soundcheck                 Makes a beep sound.");
            Console.WriteLine("     --mailcheck <list-of-emails> Sends a testing e-mail.");
            Console.WriteLine("     --normalmode                 Normal mode of the app (this option always goes first).");
            Console.WriteLine("     --url <URL>                  The url of the website to search.");
            Console.WriteLine("     --timer <timer-in-seconds>   Set every how many seconds the website is searched.");
            Console.WriteLine("     --keywords <keywords>        The keywords which the website is searched with (comma-separated).");
            Console.WriteLine("     --emails <list-of-emails>    The e-mails of the users to be notified if tickets are available.");
            Console.WriteLine();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteFailure()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} - Keywords not found.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteNoProperUsage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No proper usage of the app.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteExit()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press \'q\' and \'enter\' for exit.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteException(Exception ex)
        {
            string error = "";
            do
            {
                error += ex.Message + " ";
                ex = ex.InnerException;
            } while (ex != null);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;

            Environment.Exit(0);
        }


    } // End of Class
} // End of Namespace
