using System.Collections.Generic;

namespace TodoAppNTier.Common.ResponseObjects
{
    public class Response : IResponse
    {
        public string Message { get; set; }
        public ResponseType ResponseType { get; set; }
        
        // YENİ EKLENEN CEP
        public List<CustomValidationError> ValidationErrors { get; set; }

        public Response(ResponseType responseType)
        {
            ResponseType = responseType;
        }

        public Response(ResponseType responseType, string message)
        {
            ResponseType = responseType;
            Message = message;
        }

        // DİKKAT: FluentValidation Hatalarını Taşımak İçin Özel Metot!
        public Response(ResponseType responseType, List<CustomValidationError> validationErrors)
        {
            ResponseType = responseType;
            ValidationErrors = validationErrors;
        }
    }

    public class Response<T> : Response, IResponse<T>
    {
        public T Data { get; set; }

        public Response(ResponseType responseType, T data) : base(responseType)
        {
            Data = data;
        }

        public Response(ResponseType responseType, string message) : base(responseType, message)
        {
        }

        // Hem Data (DTO) hem de Hataları aynı anda taşımak gerekirse diye:
        public Response(ResponseType responseType, T data, List<CustomValidationError> validationErrors) : base(responseType, validationErrors)
        {
            Data = data;
        }
    }

    public enum ResponseType
    {
        Success,
        Error,
        NotFound,
        ValidationError
    }
}