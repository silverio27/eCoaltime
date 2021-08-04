using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Posts.Api.SeedWork
{
    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var errors = filterContext.ModelState.SelectMany(x =>  x.Value.Errors.Select(e => e.ErrorMessage ));
                filterContext.Result = new BadRequestObjectResult(new Response("Erro de validação", false, errors));
            }
        }


        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
    
}
