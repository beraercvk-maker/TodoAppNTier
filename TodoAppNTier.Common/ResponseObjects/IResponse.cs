namespace TodoAppNTier.Common.ResponseObjects
{
    public interface IResponse
    {
        string Message { get; set; }
        ResponseType ResponseType { get; set; }

        public List<CustomValidationError> ValidationErrors { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T Data { get; set; }
    }
}