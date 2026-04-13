using Azure.Core;
using ExaminationSystem.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExaminationSystem.Common
{
    public static class CacheKeys
    {
        public static string GetDiplomaQuizzesCacheKey (Guid diplomaId, Guid studentId)
            => $"DiplomaQuizzes_{diplomaId}_Student_{studentId}";

        public static class Durations {

            // Rarely changing data (10 minutes)
            public static readonly TimeSpan Medium = TimeSpan.FromMinutes(10);

            // Frequently changing data like quiz attempts(2 minutes)
            public static readonly TimeSpan Short = TimeSpan.FromMinutes(2); 

        }
    }
}
