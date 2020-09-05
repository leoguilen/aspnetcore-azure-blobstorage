using AutoMapper;
using Azure.Storage.Blobs.Models;
using aspnetcore_azure_blobStorage.Contracts.Responses;

namespace aspnetcore_azure_blobStorage.Mapping
{
  public class ModelToResponse : Profile
  {
    public ModelToResponse()
    {
      CreateMap<BlobItem, BlobItemResponse>()
        .ForMember(dest => dest.Name, act => act.MapFrom(src => src.Name))
        .ForMember(dest => dest.LastModifield, act => act.MapFrom(src => src.Properties.LastModified))
        .ForMember(dest => dest.Length, act => act.MapFrom(src => src.Properties.ContentLength));
    }
  }
}