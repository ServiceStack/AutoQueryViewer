using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace AutoQueryViewer.ServiceModel
{

    [Route("/services")]
    public class GetAutoQueryServices : IReturn<GetAutoQueryServicesResponse>
    {
        public bool Reload { get; set; }
    }

    public class GetAutoQueryServicesResponse
    {
        public List<AutoQueryService> Results { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    [Route("/services/register")]
    public class RegisterAutoQueryService
    {
        public string BaseUrl { get; set; }
    }

    public class RegisterAutoQueryServiceResponse
    {
        public AutoQueryService Result { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    public class AutoQueryService
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string ServiceBaseUrl { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceIconUrl { get; set; }

        public bool IsPublic { get; set; }
        public bool OnlyShowAnnotatedServices { get; set; }
        public List<Property> ImplicitConventions { get; set; }

        public string DefaultSearchField { get; set; }
        public string DefaultSearchType { get; set; }
        public string DefaultSearchText { get; set; }

        public string BrandUrl { get; set; }
        public string BrandImageUrl { get; set; }
        public string TextColor { get; set; }
        public string LinkColor { get; set; }
        public string BackgroundColor { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string IconUrl { get; set; }

        public string OwnerId { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        [IgnoreDataMember]
        public DateTime? LastSync { get; set; }
        [IgnoreDataMember]
        public string SyncError { get; set; }
        [IgnoreDataMember]
        public int TimesFailed { get; set; }
        [IgnoreDataMember]
        public bool IsActive { get; set; }
    }
}