namespace aspnetcore_azure_blobStorage.Contracts
{
  public class Routes
  {
    public static class BlobStorage
    {
      const string defaultRoute = "/blobstorage";

      public const string GetBlobs = defaultRoute;
      public const string PostBlob = defaultRoute;
      public const string DeleteContainer = defaultRoute;
    }
  }
}