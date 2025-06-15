namespace Talabat.Errors
{
    public class ApiServerError:ApiHandleError
    {
        private int internalServerError;

        public string Details { get; set; }
        public ApiServerError(int code, string? details=null, string? message=null):base(code,message)
        {
            Details = details;
        }

     
    }
}
