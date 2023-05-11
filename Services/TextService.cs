namespace TicketsAvailabilityAlerting.Services
{
    public interface ITextService
    {
        void ConsoleWriteIntroduction();
        void ConsoleWriteSuccess(string message);
        void ConsoleWriteFailure();
        void ConsoleWriteNoProperUsage();
        void ConsoleWriteExit();
        void ConsoleWriteException(Exception ex);
        void Beep();
    }

    /*******************************************************************************************************************************************/

    public class TextService : ITextService
    {
        public void ConsoleWriteIntroduction()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("***********************************************************************************************************");
            Console.WriteLine("*                                                                                                         *");
            Console.WriteLine("*                                    TICKETS AVAILABILITY ALERTING APP                                    *");
            Console.WriteLine("*                                                                                                 v1.2.1  *");
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


        public void ConsoleWriteSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now} - {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }


        public void ConsoleWriteFailure()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now} - Keywords not found.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        public void ConsoleWriteNoProperUsage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No proper usage of the app.");
            Console.ForegroundColor = ConsoleColor.White;
        }


        public void ConsoleWriteExit()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Press \'q\' and \'enter\' for exit.");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
        }


        public void ConsoleWriteException(Exception ex)
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


        public void Beep()
        {
            for (var i = 0; i < 5; i++)
                Console.Beep();
        }


    } // End of Class
} // End of Namespace