using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using aspnetcore_azure_blobStorage.Options;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace aspnetcore_azure_blobStorage.Services
{
  public class BlobStorageService : IBlobStorageService
  {
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobStorageOptions _blobStorageOptions;

    public BlobStorageService(BlobStorageOptions blobStorageOptions)
    {
      _blobStorageOptions = blobStorageOptions;
      _blobServiceClient = new BlobServiceClient(_blobStorageOptions.ConnectionString);
    }

    /// <summary>
    /// Método para deletar um container existente
    /// </summary>
    /// <returns>True se container for deletado com sucesso</returns>
    /// <returns>False se houver algum erro ao deletar o container</returns>
    public async Task<bool> DeleteContainerAsync()
    {
      var name = _blobStorageOptions.ContainerName;
      // Abaixo verifica se existe um container com o nome configurado
      var containerExist = await _blobServiceClient
        .GetBlobContainerClient(name)
        .ExistsAsync();

      if (containerExist)
      {
        // Se existe, então é deletado
        var deletedContainer = await _blobServiceClient
          .DeleteBlobContainerAsync(name);
        return deletedContainer.Status == 202 ? true : false;
      }

      return true;
    }

    /// <summary>
    /// Método para retornar todos os blobs salvos no container
    /// </summary>
    /// <returns>Retorna um array com todos os blobs salvos</returns>
    public async Task<IEnumerable<BlobItem>> GetFilesAsync()
    {
      var blobItemsList = new List<BlobItem>();
      var containerClient = await GetOrCreateContainerAsync();

      await foreach (BlobItem item in containerClient.GetBlobsAsync())
        blobItemsList.Add(item);

      return blobItemsList;
    }

    /// <summary>
    /// Método para subir novos arquivos no storage
    /// </summary>
    /// <param name="files">Arquivos passados no corpo da requisição</param>
    /// <returns>True se upload dos arquivos for executado com sucesso</returns>
    /// <returns>False se houver algum erro ao fazer upload dos arquivos</returns>
    public async Task<bool> UploadFilesAsync(IFormFile[] files)
    {
      var blobClient = await GetOrCreateContainerAsync();
      bool? blobsInserted = null;

      foreach (var file in files)
      {
        var insertedBlob = await blobClient
          .UploadBlobAsync($"{Guid.NewGuid().ToString()}{file.FileName}", file.OpenReadStream());
        blobsInserted = insertedBlob
          .GetRawResponse().Status == 201 ? true : false;
      }

      return blobsInserted.Value;
    }

    private async Task<BlobContainerClient> GetOrCreateContainerAsync()
    {
      string containerName = _blobStorageOptions.ContainerName;

      var container = _blobServiceClient
        .GetBlobContainerClient(containerName);

      if (!container.Exists())
        return await _blobServiceClient
            .CreateBlobContainerAsync(containerName);

      return container;
    }
  }
}