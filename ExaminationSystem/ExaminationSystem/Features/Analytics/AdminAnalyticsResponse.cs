using static ExaminationSystem.Features.Analytics.Dtos;

namespace ExaminationSystem.Features.Analytics
{
    public class AdminAnalyticsResponse
    {
        public List<PassRateByQuizDto> PassRateByQuiz { get; set; } = [];
        public List<AvgScoreByDiplomaDto> AvgScoreByDiploma { get; set; } = [];
        public List<AttemptsOverTimeDto> AttemptsOverTime { get; set; } = [];
        //public List<TopFailedQuestionDto> TopFailedQuestions { get; set; } = [];
    }
}
