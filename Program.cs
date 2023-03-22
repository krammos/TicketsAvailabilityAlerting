using System.Net;
using System.Net.Mail;
using System.Text;


namespace TicketsAvailabilityAlerting
{
    internal class Program
    {
        static WebClient client = new();
        static string[]? arrayOfKeywords;
        static string email = "";
        static string url = "";
        static string ticketsSiteHtmlCode = "";
        static int timerInSec = 5;
        static bool mailSent = false;


        static void Main(string[] args)
        {
            ConsoleWriteIntroduction();

            if (args.Length == 1 && args[0] == "soundtest")
            {
                Beep();
            }
            else if (args.Length == 1 && args[0] == "mailtest")
            {
                SendMail();
            }
            else if (args.Length == 4 && Int32.TryParse(args[2], out timerInSec))
            {
                arrayOfKeywords = Array.ConvertAll(args[0].Split(','), p => p.Trim());
                email = args[1];
                url = args[3];

                Timer t = new(TimerCallback, null, 0, 1000 * timerInSec);

                ConsoleWriteExit();
                while (Console.Read() != 'q');
            }
            else
            {
                ConsoleWriteNotRightUsage();
            }
        }


        private static void TimerCallback(object? sender)
        {
            try
            {
                ticketsSiteHtmlCode = client.DownloadString(url);

                if (arrayOfKeywords.Any(ticketsSiteHtmlCode.Contains))
                {
                    ConsoleWriteSuccess();
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
            mail.To.Add(email);

            // Subject
            mail.Subject = "----- Email by TicketsAvailabilityAlerting App -----";

            // Body
            StringBuilder template = new();
            template.AppendLine("Άνοιξαν τα εισιτήρια!");
            mail.IsBodyHtml = false;
            mail.Body = template.ToString();

            // SMTP settings
            using SmtpClient smtp = new("mail.haf.gr", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("konstantinos.rammos@haf.gr", "Qp0i1400()");
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
            Console.WriteLine("*                                                                                                 v1.0.1  *");
            Console.WriteLine("***********************************************************************************************************");
            Console.WriteLine();
            Console.WriteLine("Usage: TicketsAvailabilityAlerting soundtest");
            Console.WriteLine("or");
            Console.WriteLine("Usage: TicketsAvailabilityAlerting mailtest");
            Console.WriteLine("or");
            Console.WriteLine("Usage: TicketsAvailabilityAlerting <comma-separated-search-keywords> <e-mail> <timer-in-seconds> <URL>");
            Console.WriteLine("Example: TicketsAvailabilityAlerting \"ΟΛΥΜΠΙΑΚΟΣ, ΟΣΦΠ, 19/03/2023\" \"kr@gmail.com\" 10 \"https://www.ticketmaster.gr/aek\"");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} - Άνοιξαν τα εισιτήρια.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteFailure()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} - Δεν βρέθηκαν οι λέξεις-κλειδιά.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteNotRightUsage()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Όχι σωστή χρήση εφαρμογής. Δες usage.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteExit()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Πάτα \'q\' και \'enter\' για έξοδο.");
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
        }


    } // End of Class
} // End of Namespace