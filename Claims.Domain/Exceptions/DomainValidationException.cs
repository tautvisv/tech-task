namespace Claims.Domain.Exceptions
{
    public class DomainValidationException: DomainException
    {
        public string ParamName { get; }

        public DomainValidationException(string message, string paramName) : base(message)
        {
            ParamName = paramName;
        }
    }
}
