using SharedKernel.Common;
using SharedKernel.Common.CacheModels;

namespace HCM.Application.Common
{
    public class UserIdentityHCMModel : UserIdentitySharedModel
    {
        public UserCacheModel UserInfo { get; set; }
    }
}
