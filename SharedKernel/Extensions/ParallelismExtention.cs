using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedKernel.Extensions
{
    public static class ParallelismExtention
    {
        private static readonly object IsExistLock = new object();
        private static bool IsExist = true;
        private static int MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.60) * 2.0));

        public static bool ExistanceChecker(this List<long> ids, Func<long, object> body)
        {
            var result = Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism }, id =>
                {
                    if (body.Invoke(id) == null)
                    {
                        lock (IsExistLock)
                            IsExist = false;
                    }
                });
            if (result.IsCompleted)
                return IsExist;
            else
                return false;
        }
    }
}
