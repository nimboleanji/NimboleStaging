using System.Text;
using System.Web.Http.ExceptionHandling;
using NIMBOLE.Service.Results;

namespace NIMBOLE.Service.ExceptionHandling
{
    public class GenericTextExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            context.Result = new ChallengeResult("An unhandled exception occurred; check the log for more information.", Encoding.UTF8, context.Request);
        }
    }
}