namespace Travel.Core.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<bool> UploadFile(Stream fileStream, string fileName);
        Task Delete(string path);
    }
}
