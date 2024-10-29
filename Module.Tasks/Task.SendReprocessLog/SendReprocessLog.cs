using Helpers;
using Services;

namespace Task.SendReprocessLog
{ 
    public class SendReprocessLog
    {
        public void Run() 
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("start copy_auto_files = " + DateTime.Now);

            // Ścieżka do katalogu tymczasowego, do przechowywania logów w czasie trwania aplikacji
            string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp", "commands", "send_reprocess_log");
            Directory.CreateDirectory(destinationPath);

            string filesToCopy = "w_reprocess_";
            //string configFilePath = "C:/kerp_dotnet/KERP/Module.Tasks/config.json";
            string configFilePath = "C:/Users/winowlu/Documents/KERP/Module.Tasks/config.json";

            // Użycie ConfigLoader do załadowania konfiguracji SFTP
            SftpConfig sftpConfig = ConfigLoader.LoadSectionFromFile<SftpConfig>(configFilePath, "Sftp");

            // sftpService to instancja klasy SftpService, która obsługuje połączenie z serwerem SFTP.
            SftpService sftpService = new SftpService(sftpConfig);
            List<string> downloadedFiles = sftpService.DownloadFiles("/home/jobdm241/", filesToCopy, destinationPath, "txt");

            // Analiza poszczególnych logów reprocesów
            Console.WriteLine("Analiza logów.");
            AnalyzeReprocessLog analyzeReprocessLog = new AnalyzeReprocessLog();
            analyzeReprocessLog.GetProductionOrdersWithError(downloadedFiles);
            Console.WriteLine("Załączenie raportu z analizą logów");

            // Użycie ConfigLoader do załadowania konfiguracji SMTP
            SmtpConfig smtpConfig = ConfigLoader.LoadSectionFromFile<SmtpConfig>(configFilePath, "Smtp");

            // Inicjalizacja SmtpService z ustawieniami
            SmtpService smtpService = new SmtpService(smtpConfig);

            Console.WriteLine("Wysyłam maile.");

            string subject = "Log reprocesów " + DateTime.Now.ToString("yyyy-MM-dd");
            string mailBody = $"W załączniku log reprocesów z dnia {DateTime.Now.ToString("yyyy-MM-dd")}.";
            //List<string> recipients = new List<string> { "lukasz.winowicz@kalmarglobal.com", "dl.kamosproductionplanning.plsts1@kalmarglobal.com", "erpln.stargard@kalmarglobal.com" };
            List<string> recipients = new List<string> { "lukasz.winowicz@kalmarglobal.com" };
            smtpService.SendMail(new List<EmailMessage>
        {
            new EmailMessage
            {
                SendFrom = smtpConfig.Email,
                SendTo = recipients,
                Subject = subject,
                MailBody = mailBody,
                Files = downloadedFiles
            }
            });

            Console.WriteLine("Maile wysłane.");
            Console.WriteLine("koniec copy_auto_files = " + DateTime.Now);
        }
    }
}
