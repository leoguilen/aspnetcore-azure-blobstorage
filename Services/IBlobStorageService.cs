using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace aspnetcore_azure_blobStorage.Services
{
  public interface IBlobStorageService
  {
    Task<bool> UploadFilesAsync(IFormFile[] files);
    Task<IEnumerable<BlobItem>> GetFilesAsync();
    Task<bool> DeleteContainerAsync();
  }
}