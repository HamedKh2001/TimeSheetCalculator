using System.Collections.Generic;

namespace SharedKernel.Common.CacheModels
{
    public class UserCacheModel
    {
        public long UserId { get; set; }
        public int EmployeeCode { get; set; }
        public long? MainOrganizationId { get; set; }
        public long? MainJobId { get; set; }
        public List<UserPositionCacheModel> Positions { get; set; } = new List<UserPositionCacheModel>();
        public long EmployeeId { get; set; }
    }
}
