using System.Collections.Generic;
using System.Threading.Tasks;
using aspnetcore_azure_blobStorage.Contracts;
using aspnetcore_azure_blobStorage.Contracts.Responses;
using aspnetcore_azure_blobStorage.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_azure_blobStorage.Controllers
{
  [ApiController]
  public class BlobStorageController : ControllerBase
  {
    private readonly IBlobStorageService _blobStorageService;
    private readonly IMapper _mapper;
    public BlobStorageController(IBlobStorageService blobStorageService, IMapper mapper)
    {
      _blobStorageService = blobStorageService;
      _mapper = mapper;
    }

    [HttpGet(Routes.BlobStorage.GetBlobs)]
    public async Task<IActionResult> GetBlobs()
    {
      var blobsItems = await _blobStorageService
        .GetFilesAsync();

      return Ok(_mapper
        .Map<List<BlobItemResponse>>(blobsItems));
    }

    [HttpPost(Routes.BlobStorage.PostBlob)]
    public async Task<IActionResult> UploadBlob(IFormFile[] files)
    {
      var insertedBlobs = await _blobStorageService
        .UploadFilesAsync(files);

      if (!insertedBlobs)
        return BadRequest();

      return Ok(new
      {
        message = $"{files.Length} arquivos foram salvos no storage com sucesso"
      });
    }

    [HttpDelete(Routes.BlobStorage.DeleteContainer)]
    public async Task<IActionResult> DeleteContainer()
    {
      if (!await _blobStorageService.DeleteContainerAsync())
        return BadRequest();

      return NoContent();
    }
  }
}