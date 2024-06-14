using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using SharedKernel.Common.Settings;
using SharedKernel.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedKernel.Common.Filters
{
    public class CollaborativeAsyncActionFilter : IAsyncActionFilter
    {
        private IOptions<List<CollaborativeSettingConfigurationModel>> _setting;
        private readonly string _keys;

        public CollaborativeAsyncActionFilter(IOptions<List<CollaborativeSettingConfigurationModel>> setting, string keys)
        {
            _setting = setting;
            _keys = keys;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var headers = context.HttpContext.Request.Headers;

            if (headers.ContainsKey("ClientKey") && headers.ContainsKey("SecretKey"))
            {
                var clientKey = headers["ClientKey"].ToString();
                if (_keys.Length > 0 && _keys.Split(',').Any(k => k == clientKey) == false)
                    throw new BadRequestException("You do not have access to this method.");

                var collaborativeSettings = _setting.Value;
                var secretKey = headers["SecretKey"];

                var foundRecord = collaborativeSettings?.FirstOrDefault(c => c.ClientKey == clientKey);
                if (foundRecord != null && foundRecord.SecretKey == secretKey)
                {
                    await next();
                    return;
                }
            }

            throw new BadRequestException("Your request is invalid");
        }
    }
}