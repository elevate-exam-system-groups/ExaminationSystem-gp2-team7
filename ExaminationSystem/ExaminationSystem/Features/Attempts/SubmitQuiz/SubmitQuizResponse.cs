namespace ExaminationSystem.Features.Attempts.SubmitQuiz
{
    public class SubmitQuizResponse
    {
        public Guid AttemptId { get; set; }

        public decimal Score { get; set; }
        public bool IsPassed { get; set; }

        public string Status { get; set; }



    }
}
