using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SuzumesDeepDungeon.Data;

namespace SuzumesDeepDungeon.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {
        private const string API_KEY_HEADER = "X-API-Key";
        private const string API_KEY_QUERY = "api";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Проверяем ключ в заголовке
            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER, out var extractedKey))
            {
                // Если нет в заголовке, проверяем в query string
                if (!context.HttpContext.Request.Query.TryGetValue(API_KEY_QUERY, out extractedKey))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "API Key is missing"
                    };
                    return;
                }
            }

            // Получаем контекст базы данных
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<DatabaseContext>();

            // Получаем все активные ключи
            var apiKeys = await dbContext.Api
                .Where(k => k.IsActive)
                .Select(k => k.HashKey).ToListAsync();

            // Проверяем ключ
            var isValid = apiKeys.Any(hash => VerifyKey(extractedKey.ToString(), hash));

            if (!isValid)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Invalid API Key"
                };
                return;
            }

            await next();
        }

        private bool VerifyKey(string key, string hash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(key, hash);
        }
    }
}
