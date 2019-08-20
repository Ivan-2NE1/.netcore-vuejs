namespace VSAND.Data.ViewModels
{
    public class ApiResult<T>
    {
        public int Id { get; set; } = 0;
        public bool Success { get; set; } = false;
        public string Message { get; set; } = "";

        public ApiResult()
        {

        }

        public ApiResult(ServiceResult<T> serviceResult)
        {
            Id = serviceResult.Id;
            Success = serviceResult.Success;
            Message = serviceResult.Message;
        }
    }
}
