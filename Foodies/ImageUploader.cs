using Firebase.Storage;

namespace Foodies
{
    public  class ImageUploader
    {
        private readonly FirebaseStorage _firebaseStorage;

        public ImageUploader(FirebaseStorage firebase)
        {
            _firebaseStorage = firebase;
          //   = new FirebaseStorage("");
        }
        public  async Task<string?> UploadImageAsync(IFormFile image)
        {
            string? imageUrl = null;

            if (image != null && image.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var path = Path.GetTempFileName();

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                try
                {
                    await _firebaseStorage
                        .Child("images")
                        .Child(fileName)
                        .PutAsync(System.IO.File.OpenRead(path));

                    // System.IO.File.Delete(path);

                    imageUrl = await _firebaseStorage.Child("images").Child(fileName).GetDownloadUrlAsync();
                }
                catch (FirebaseStorageException ex)
                {
                    throw new Exception($"Error uploading image: {ex.Message}");
                }
            }

            return imageUrl;
        }
    }

}