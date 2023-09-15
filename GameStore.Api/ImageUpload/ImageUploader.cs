using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace GameStore.Api.ImageUpload;

public class ImageUploader : IImageUploader
{
    private readonly BlobContainerClient containerClient;

    public ImageUploader(BlobContainerClient containerCLient)
    {
        this.containerClient = containerCLient;
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        var blobClient = containerClient.GetBlobClient(file.FileName);
        await blobClient.DeleteIfExistsAsync();

        using var fileStream = file.OpenReadStream();
        await blobClient.UploadAsync(
            fileStream,
            new BlobHttpHeaders { ContentType = file.ContentType });

        return blobClient.Uri.ToString();
    }
}