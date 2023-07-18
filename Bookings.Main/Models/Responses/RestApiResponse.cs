namespace Bookings.Web.Models.Responses
{
    public class RestApiResponse<T>
        where T : class
    {
        public bool IsSuccess { get; private set; }

        public string Error { get; private set; }

        public T? Result { get; private set; }

        /// <summary>
        /// s.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static RestApiResponse<T> Success(T data)
        {
            return new RestApiResponse<T>()
            {
                IsSuccess = true,
                Result = data,
            };
        }

        public static RestApiResponse<T> NonSuccess(string error)
        {
            return new RestApiResponse<T>()
            {
                IsSuccess = true,
                Error = error,
                Result = null,
            };
        }
    }
}
