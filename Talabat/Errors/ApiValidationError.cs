namespace Talabat.Errors
{
    public class ApiValidationError
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationError()
        {
            Errors = new List<string>();
        }
    }
}
