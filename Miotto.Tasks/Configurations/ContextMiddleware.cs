using Miotto.Tasks.Infra;

namespace Miotto.Tasks.API.Configurations;

public class ContextMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var dbContext = context.RequestServices.GetRequiredService<TasksContext>();

        await next.Invoke(context);

        await dbContext.SaveChangesAsync();
    }
}