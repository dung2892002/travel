using Firebase.Storage;
using Microsoft.Extensions.Configuration;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Infrastructure.Firebase
{
    public class FirebaseStorageService(IConfiguration configuration) : IFirebaseStorageService, IService
    {
        private readonly IConfiguration _configuration = configuration;
        public async Task Delete(string path)
        {
            try
            {
                var bucket = _configuration["Firebase:Bucket"];
                var task = new FirebaseStorage(bucket).Child(path).DeleteAsync();
                await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file {path}: {ex.Message}");
            }
        }

        public async Task<bool> UploadFile(Stream fileStream, string fileName)
        {
            try
            {
                var bucket = _configuration["Firebase:Bucket"];
                var task = new FirebaseStorage(bucket).Child(fileName).PutAsync(fileStream);
                string downloadUrl = await task;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return false;
            }
        }
    }
}
