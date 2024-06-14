namespace SharedKernel.Common.CacheModels
{
    public class UserPositionCacheModel
    {
        public long OrganizationId { get; set; }
        public long JobId { get; set; }
        public bool HasOrganizationAccess { get; set; }
    }
}
