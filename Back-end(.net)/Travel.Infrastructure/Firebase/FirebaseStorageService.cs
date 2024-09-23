using Firebase.Storage;
using Travel.Core.Interfaces;
using Travel.Core.Interfaces.IServices;

namespace Travel.Infrastructure.Firebase
{
    public class FirebaseStorageService : IFirebaseStorageService, IService
    {
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            fileName = DateTime.Now.ToString("dd-mm-yyyy")+ "-" + fileName;
            var task = new FirebaseStorage("imagetravel-bff0d.appspot.com").Child(fileName).PutAsync(fileStream);

            try
            {
                string downloadUrl = await task;
                return downloadUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading file: {ex.Message}");
            }

            return string.Empty;
        }
    }
}
