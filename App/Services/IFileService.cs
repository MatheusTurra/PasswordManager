namespace App.Services
{
    public interface IFileService
    {
        void createDirectory(string DirectoryPath);
        void createNewFile(string filePath, string fileContent);
        string getProjectRootDirectory();
    }
}