namespace App.Services
{
    public interface IFileService
    {
        void CreateDirectory(string DirectoryPath);
        void CreateNewFile(string filePath, string fileContent);
        string GetProjectRootDirectory();
    }
}