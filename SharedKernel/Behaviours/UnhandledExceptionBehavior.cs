using MediatR;
using Microsoft.Extensions.Logging;
using SharedKernel.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Behaviours
{
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<TRequest> _logger;

        public UnhandledExceptionBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                // _logger.LogHandleEvent(request, true);
                var response = await next();
                // _logger.LogHandleEvent(response, false);
                return response;
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, $"Application Request: Unhandled Exception for Request {requestName} {request}");
                throw;
            }
        }
    }
}
