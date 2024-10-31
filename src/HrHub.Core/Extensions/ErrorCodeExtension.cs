using Newtonsoft.Json;

namespace HrHub.Core.Extensions
{
    public static class ErrorCodeExtensions
    {
        public static string GetAllInnerExceptionsAsJson(this Exception exception)
        {
            try
            {
                List<Exception> innerExceptions = new List<Exception>();
                innerExceptions.Add(exception);
                GetAllInnerExceptions(exception, innerExceptions);
                List<string> exceptionMessages = innerExceptions.Select(ex => ex.Message).ToList();

                return JsonConvert.SerializeObject(exceptionMessages);
            }
            catch (Exception exp)
            {

                return exception.Message;
            }
        }

        private static void GetAllInnerExceptions(Exception exception, List<Exception> innerExceptions)
        {
            if (exception.InnerException != null)
            {
                innerExceptions.Add(exception.InnerException);
                GetAllInnerExceptions(exception.InnerException, innerExceptions);
            }
        }
    }
}
