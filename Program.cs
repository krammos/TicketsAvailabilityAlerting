using System.Net;

namespace TicketsAvailabilityAlerting
{
    internal class Program
    {
        static WebClient client = new();
        static string[]? arrayOfKeywords;
        static int timerInSec = 5;
        static string url = "";
        static string ticketsSiteHtmlCode = "";


        static void Main(string[] args)
        {
            ConsoleWriteIntroduction();

            if (args.Length == 1 && args[0] == "soundtest")
            {
                Beep();
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


        private static void TimerCallback(object? sender)
        {
            ticketsSiteHtmlCode = client.DownloadString(url);

            if (arrayOfKeywords.Any(ticketsSiteHtmlCode.Contains))
            {
                ConsoleWriteSuccess();
                Beep();
            }
            else
            {
                ConsoleWriteFailure();
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
            Console.WriteLine("*                                                                                                 v1.0.0  *");
            Console.WriteLine("***********************************************************************************************************");
            Console.WriteLine();
            Console.WriteLine("Usage: TicketsAvailabilityAlerting soundtest");
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

    } // End of Class
} // End of Namespace