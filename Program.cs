using System.Net;
using System.Net.Mail;
using System.Text;


namespace TicketsAvailabilityAlerting
{
    internal class Program
    {
        static WebClient client = new();
        static string[]? arrayOfKeywords;
        static int timerInSec = 5;
        static string url = "";
        static string ticketsSiteHtmlCode = "";
        static bool mailSent = false;


        static void Main(string[] args)
        {
            try
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
                else if (args.Length == 3 && Int32.TryParse(args[1], out timerInSec))
                {
                    url = args[0];
                    arrayOfKeywords = Array.ConvertAll(args[2].Split(','), p => p.Trim());

                    Timer t = new(TimerCallback, null, 0, 1000 * timerInSec);

                    ConsoleWriteExit();
                    while (Console.Read() != 'q') ;
                }
                else
                {
                    ConsoleWriteNotRightUsage();
                }
            }
            catch (Exception ex)
            {
                ConsoleWriteFailure(ex.Message);
            }
        }


        private static void TimerCallback(object? sender)
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
                ConsoleWriteFailure("Δεν βρέθηκαν οι λέξεις-κλειδιά.");
            }
        }


        private static void SendMail()
        {
            try
            {
                using MailMessage mail = new();

                // From
                mail.From = new MailAddress("konstantinos.rammos@haf.gr", "TicketsAvailabilityAlerting");

                // To
                mail.To.Add("k.rammos1@gmail.com");

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
            catch (Exception ex)
            {
                throw new Exception($"Σφάλμα κατά την αποστολή του email. Λεπτομέρειες: {ex.Message}");
            }
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
            Console.WriteLine("Usage: TicketsAvailabilityAlerting <URL> <timer-in-seconds> <comma-separated-search-keywords>");
            Console.WriteLine("Example: TicketsAvailabilityAlerting \"https://www.ticketmaster.gr/aek\" 1 \"ΟΛΥΜΠΙΑΚΟΣ, ΟΣΦΠ, 19/03/2023\"");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} - Άνοιξαν τα εισιτήρια.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        private static void ConsoleWriteFailure(string error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} - {error}");
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


    } // End of Class
} // End of Namespace