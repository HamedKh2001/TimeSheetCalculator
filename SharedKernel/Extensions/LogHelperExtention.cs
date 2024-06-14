using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharedKernel.Common.Enums;
using System;
using System.Collections.Generic;

namespace SharedKernel.Extensions
{
    public static class LogHelperExtention
    {
        public static ILogger<T> LogCRUDEvent<T>(this ILogger<T> logger, LogEventType logEventType, object obj, Action<List<string>> args = null) where T : class
        {
            var logStr = string.Empty;
            var serializedObj = MySerialization(obj);
            string addArgs = GetArgs(args);
            if (logEventType == LogEventType.Read)
                logStr = $"Record Has Been Fetched Successfully In {nameof(T)} Class. Object:{serializedObj} <> Args:{addArgs}";
            else
                logStr = $"Record Has Been {Enum.GetName(logEventType)}d Successfully In {nameof(T)} Class. Object:{serializedObj} <> Args:{addArgs}";
            logger.LogInformation(logStr.ToString());
            return logger;
        }

        public static void LogHandleEvent<T>(this ILogger<T> logger, object obj, bool isBeforeExecution)
        {
            string logStr = isBeforeExecution ? "Mode: Before Execution <>" : "Mode: After Execution <>";
            var serializedObj = JsonConvert.SerializeObject(obj);
            logStr += $"Class: {typeof(T).Name} <> Object: {serializedObj}";
            logger.LogInformation(logStr.ToString());
        }

        public static ILogger<T> LogObjectBeforeUpdate<T>(this ILogger<T> logger, object obj, Action<List<string>> args = null)
        {
            var serializedObj = MySerialization(obj);
            string addArgs = GetArgs(args);
            string logStr = $"Object Prepared For Update. Object:{serializedObj} <> Args:{addArgs}";
            logger.LogInformation(logStr.ToString());
            return logger;
        }

        public static ILogger<T> LogPublishObjectToQueue<T>(this ILogger<T> logger, object obj, Action<List<string>> args = null)
        {
            var serializedObj = MySerialization(obj);
            string addArgs = GetArgs(args);
            string logStr = $"Object Published To Queue. Object:{serializedObj} <> Args:{addArgs}";
            logger.LogInformation(logStr.ToString());
            return logger;
        }

        #region Helper
        private static string MySerialization(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        private static string GetArgs<T>(Action<List<T>> args) where T : class
        {
            List<T> additionalArgs = new();
            args?.Invoke(additionalArgs);
            string addArgs = args != null ? string.Join("||", additionalArgs) : string.Empty;
            return addArgs;
        }
        #endregion
    }
}
