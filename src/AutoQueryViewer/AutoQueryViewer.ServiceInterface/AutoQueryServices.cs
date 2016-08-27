using System;
using ServiceStack;
using AutoQueryViewer.ServiceModel;
using ServiceStack.OrmLite;

namespace AutoQueryViewer.ServiceInterface
{
    public class AutoQueryServices : Service
    {
        public ContentCache ContentCache { get; set; }

        public object Any(Ping request)
        {
            return "OK";
        }

        public object Any(GetAutoQueryServices request)
        {
            var key = ContentCache.GetAutoQueryServices(clear: request.Reload);
            return base.Request.ToOptimizedResultUsingCache(ContentCache.Client, key, () =>
            {
                return new GetAutoQueryServicesResponse
                {
                    Results = Db.Select<AutoQueryService>()
                };
            });
        }

        public object Any(RegisterAutoQueryService request)
        {
            var client = new JsonServiceClient(request.BaseUrl);
            var response = client.Get(new AutoQueryMetadata());

            if (!response.Config.IsPublic)
                throw HttpError.Conflict("This service does not permit publishing");

            var service = response.Config.ConvertTo<AutoQueryService>();

            var existingService = Db.Single<AutoQueryService>(q => q.ServiceBaseUrl == request.BaseUrl);
            if (existingService != null)
            {
                existingService.PopulateWith(response.Config);
                existingService.LastModified = DateTime.UtcNow;
                Db.Update(existingService);
                service = existingService;
            }
            else
            {
                service.Created = service.LastModified = DateTime.UtcNow;
                service.IsActive = true;
                Db.Save(service);
            }

            ContentCache.ClearAll();

            return new RegisterAutoQueryServiceResponse
            {
                Result = service
            };
        }

        public object Any(Dummy request)
        {
            return request;
        }
    }

    public class Dummy
    {
        public AutoQueryMetadata AutoQueryMetadata { get; set; }
        public AutoQueryMetadataResponse AutoQueryMetadataResponse { get; set; }
    }
}