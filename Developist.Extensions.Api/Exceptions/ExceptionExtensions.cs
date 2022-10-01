using System.Text;

namespace Developist.Extensions.Api.Exceptions
{
    public static class ExceptionExtensions
    {
        public static string DetailMessage(this Exception exception, bool includeInnerExceptions = true)
        {
            StringBuilder detailMessageBuilder = new(exception.BuildDetailMessage());
            if (includeInnerExceptions)
            {
                exception.AppendInnerExceptionsTo(detailMessageBuilder);
            }
            return detailMessageBuilder.ToString();
        }

        private static string BuildDetailMessage(this Exception exception)
        {
            string detailMessage = $"{exception.GetType().FullName}: {exception.Message}";
            if (!string.IsNullOrEmpty(exception.StackTrace))
            {
                detailMessage += Environment.NewLine + exception.StackTrace;
            }
            return detailMessage;
        }

        private static void AppendInnerExceptionsTo(this Exception exception, StringBuilder detailMessageBuilder, int depth = 0)
        {
            if (exception.InnerException is not null)
            {
                depth++;
                detailMessageBuilder.Append($" [{nameof(exception.InnerException)} ({depth}): {exception.InnerException.BuildDetailMessage()}]");
                exception.InnerException.AppendInnerExceptionsTo(detailMessageBuilder, depth);
            }
        }
    }
}
