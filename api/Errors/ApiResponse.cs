namespace api.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message=null)
        {
            StatusCode = statusCode;

            Message = message ?? GetDefaultMessageForStatusCode(statusCode);

        }
        public int StatusCode { get; set; }
        public string Message { get;set; }
        private string GetDefaultMessageForStatusCode(int StatusCode)
        {
            return StatusCode switch { 400 => "Bad Request, You Have Made",
                401 => "Autorized You Are Not",
                404 => "Response is not found",
                500 => "server Error",
                _=> null,
            };
        }
    }
}
