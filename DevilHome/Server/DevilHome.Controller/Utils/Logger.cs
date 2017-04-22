using System;
using System.Threading.Tasks;
using Windows.Storage;
using DevilHome.Common.Interfaces.Enums;

namespace DevilHome.Controller.Utils
{
    internal static class Logger
    {
        private static bool m_IsInitialized;
        private static StorageFolder m_StorageFolder;

        public static async Task InitializeLogger()
        {
            if (!m_IsInitialized)
            {
                //using KnownFolders.PicturesLibrary because DocumentLibrary isn't avaible
                StorageFolder documentsFolder = KnownFolders.PicturesLibrary;
                try
                {
                    m_StorageFolder = await documentsFolder.CreateFolderAsync("DevilHome");
                }
                catch
                {
                    m_StorageFolder = await documentsFolder.GetFolderAsync("DevilHome");
                }
                
                m_IsInitialized = true;
            }
        }

        public static bool IsInitialized()
        {
            return m_IsInitialized;
        }

        private static async Task<StorageFile> GetOrCreateFile()
        {
            string fileName = $"{DateTime.Now:yyyyMMdd}_DevilHome.txt";
            StorageFile storageFile = null;

            try
            {
                storageFile = await m_StorageFolder.GetFileAsync(fileName);
            }
            catch
            {
                // ignored
            }

            return storageFile ?? (await m_StorageFolder.CreateFileAsync(fileName));
        }

        public static async Task LogError(Exception exception, PluginEnum pluginEnum)
        {
            StorageFile storageFile = await GetOrCreateFile();
            string fileContent = await FileIO.ReadTextAsync(storageFile);
            fileContent += $"\r\n[ERROR in {pluginEnum.GetStringValue()}]\r\n{DateTime.Now:dd.MM.yyyy HH:mm}\t{exception.Message}";
            await FileIO.WriteTextAsync(storageFile, fileContent);
        }

        public static async Task LogInformation(string information, PluginEnum pluginEnum)
        {
            StorageFile storageFile = await GetOrCreateFile();
            string fileContent = await FileIO.ReadTextAsync(storageFile);
            fileContent += $"\r\n[INFORMATION in {pluginEnum.GetStringValue()}]\r\n{DateTime.Now:dd.MM.yyyy HH:mm}\t{information}";
            await FileIO.WriteTextAsync(storageFile, fileContent);
        }
    }
}