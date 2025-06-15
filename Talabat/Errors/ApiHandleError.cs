namespace Talabat.Errors
{
    public class ApiHandleError
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public ApiHandleError(int code,string? message=null)
        {
            Code = code;
            Message = message ?? ApplyMessage(code);
        }
        private string ApplyMessage(int code)
        {
            return code switch
            {
                400 => "BadRequest Error",
                404 => "NotFound Error",
                500 => "Server Error",
                _ => null
            };


        }
    }
}
