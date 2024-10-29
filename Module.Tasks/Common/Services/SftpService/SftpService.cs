using Renci.SshNet;

namespace Services
{
    public class SftpService
    {
        private readonly SftpConfig _config;

        public SftpService(SftpConfig config)
        {
            _config = config;
        }

        public List<string> DownloadFiles(string remoteDirectory, string prefix, string localDirectory, string attachmentType)
        {
            List<string> downloadedFiles = new List<string>();

            using (var sftp = new SftpClient(_config.Host, _config.Port, _config.Username, _config.Password))
            {
                sftp.Connect();

                // Znajdź wszystkie pliki zaczynające się od określonego prefiksu
                List<string> filesToDownload = sftp.ListDirectory(remoteDirectory)
                                        .Where(f => !f.IsDirectory && f.Name.StartsWith(prefix))
                                        .Select(f => f.Name)
                                        .ToList();

                foreach (var filename in filesToDownload)
                {
                    string remoteFilePath = Path.Combine(remoteDirectory, filename);
                    string localFilePath = Path.Combine(localDirectory, filename + "." + attachmentType);

                    if (sftp.Exists(remoteFilePath))
                    {
                        Console.WriteLine($"Kopiuję plik {filename}");
                        using (Stream fileStream = File.Create(localFilePath))
                        {
                            sftp.DownloadFile(remoteFilePath, fileStream);
                        }
                        downloadedFiles.Add(localFilePath);
                    }
                    else
                    {
                        Console.WriteLine($"Nie pobrano pliku {filename}");
                    }
                }
                sftp.Disconnect();
            }
            return downloadedFiles;
        }
    }
}