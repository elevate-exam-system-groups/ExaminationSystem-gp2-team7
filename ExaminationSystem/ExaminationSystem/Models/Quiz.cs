using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExaminationSystem.Models
{
    [Table("Quizzes")]
    public class Quiz : BaseEntity
    {
        public Guid DiplomaId { get; set; }

        public string Title { get; set; }

        public int DurationMinutes { get; set; }

        public decimal PassScore { get; set; } = 60m;

        public int? MaxAttempts { get; set; }

        public string? Instructions { get; set; }

        public QuizStatus Status { get; set; } = QuizStatus.Draft;

        public int TotalQuestionsCache { get; set; } = 0;

        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(DiplomaId))]
        public virtual Diploma Diploma { get; set; }

        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

        public virtual ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();
    }

    public enum QuizStatus
    {
        Draft,
        Published
    }
}
