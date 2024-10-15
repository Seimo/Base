namespace WoS.Extensions
{
    public static class ExceptionExtensions
    {

        public static string GetBaseExceptionMessage(this Exception value)
        {
            return value.GetBaseException().Message;
        }

        public static string GetInnerExceptionMessage(this Exception value)
        {
            return value.InnerException != null ? $"{value.Message} -> {value.InnerException.GetInnerExceptionMessage()}" : value.Message;
        }

    }

}
