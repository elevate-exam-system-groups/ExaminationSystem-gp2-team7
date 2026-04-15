namespace ExaminationSystem.Common
{
    public class ResultHandler
    {
        protected readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0;

        public bool IsFailure => !IsSuccess;
        protected IReadOnlyList<Error> Errors => _errors;

        protected ResultHandler() { }
        protected ResultHandler(Error error)
        {
            _errors.Add(error);
        }   
        protected ResultHandler(List<Error> errors)
        {
            _errors.AddRange(errors);
        }

        public static ResultHandler Success() => new ResultHandler(); 
        public static ResultHandler Failure(Error error) => new ResultHandler(error);
        public static ResultHandler Failure(List<Error> errors) => new ResultHandler(errors);

    }


    public class ResultHandler<TValue> : ResultHandler
    {
        private readonly TValue _value;
        public TValue value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access value of a failed result."); 
        private ResultHandler(TValue? value)
        {
            _value = value!;
        }
        private ResultHandler(Error error) : base(error) { 
            _value = default!;
        }
        private ResultHandler(List<Error> errors) : base(errors) {
            _value = default!;
        }
        public static ResultHandler<TValue> Success(TValue value) => new ResultHandler<TValue>(value);
        public static new ResultHandler<TValue> Failure(Error error) => new ResultHandler<TValue>(error);
        public static new ResultHandler<TValue> Failure(List<Error> errors) => new ResultHandler<TValue>(errors);
    }
}
