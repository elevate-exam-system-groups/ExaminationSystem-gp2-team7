namespace ExaminationSystem.Features.Analytics
{
    public class Dtos
    {
        public record PassRateByQuizDto(Guid QuizId, string QuizTitle, double PassRate);
        public record AvgScoreByDiplomaDto(Guid DiplomaId, string DiplomaTitle, decimal? AvgScore);
        public record AttemptsOverTimeDto(DateTime Date, int Count);
        //public record TopFailedQuestionDto(Guid QuestionId, string Content, double CorrectRate);
    }
}
