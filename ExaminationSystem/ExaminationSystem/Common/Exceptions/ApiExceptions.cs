namespace ExaminationSystem.Common.Exceptions
{
    // بنرميها لما الحاجة مش موجودة في الداتابيز → بتتحول لـ 404
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    // بنرميها لما المستخدم مش من حقه يعمل العملية دي → بتتحول لـ 403
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) : base(message) { }
    }

    // بنرميها لما فيه تعارض (مثلاً الامتحان اتسلم قبل كده) → بتتحول لـ 409
    public class ConflictException : Exception
    {
        public ConflictException(string message) : base(message) { }
    }

    // بنرميها لما البيانات غلط في الـ Business Logic (مثلاً OTP غلط) → بتتحول لـ 400
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) { }
    }
}
