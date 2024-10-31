namespace HrHub.Abstraction.Result
{
    public class Response<T> where T : class
    {
        public Response()
        {
            Header = new ResponseHeader();
            Body = default;
        }

        public Response(T body, ResponseHeader header)
        {
            Body = body;
            Header = header;
        }

        public Response(T body, bool isSuccess)
        {
            Body = body;
            Header = new ResponseHeader { Result = isSuccess };
        }

        public Response(bool isSuccess, string message)
        {
            Header = new ResponseHeader { Result = isSuccess, Msg = message };
        }

        public Response(bool isSuccess, string message, int responseCode)
        {
            Header = new ResponseHeader { Result = isSuccess, ResCode = responseCode, Msg = message };
        }

        public Response(T body, bool isSuccess, string message, int responseCode)
        {
            Body = body;
            Header = new ResponseHeader { Result = isSuccess, ResCode = responseCode, Msg = message };
        }

        public ResponseHeader Header { get; set; }

        public T Body { get; set; }
        public bool IsSuccess()
        {
            return Header.Result;
        }

        // ReSharper disable once CA1000
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static Response<TBody> Success<TBody>(TBody body) where TBody : class => new Response<TBody>(body, true);
#pragma warning restore CA1000 // Do not declare static members on generic types
        // ReSharper disable once CA1000
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static Response<TBody> Success<TBody>(TBody body, ResponseHeader header = null) where TBody : class => new Response<TBody>(body, header);
#pragma warning restore CA1000 // Do not declare static members on generic types

        public static Response<TBody> Success<TBody>(TBody body, string message) where TBody : class
            => new Response<TBody>(body, true, message, 200);

        // ReSharper disable once CA1000
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static Response<TBody> Fail<TBody>(TBody body) where TBody : class => new Response<TBody>(body, false);
#pragma warning restore CA1000 // Do not declare static members on generic types

        // ReSharper disable once CA1000
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static Response<TBody> Fail<TBody>(string message) where TBody : class => new Response<TBody>(false, message);
#pragma warning restore CA1000 // Do not declare static members on generic types

        // ReSharper disable once CA1000
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static Response<TBody> Fail<TBody>(string message, int responseCode) where TBody : class => new Response<TBody>(false, message, responseCode);
#pragma warning restore CA1000 // Do not declare static members on generic types


        // ReSharper disable once CA1000
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static Response<TBody> Fail<TBody>(TBody body, string message, int responseCode) where TBody : class => new Response<TBody>(body, false, message, responseCode);
#pragma warning restore CA1000 // Do not declare static members on generic types
    }
}
