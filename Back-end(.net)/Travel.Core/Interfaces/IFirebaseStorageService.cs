namespace Travel.Core.Interfaces
{
    public interface IFirebaseStorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
    }
}
