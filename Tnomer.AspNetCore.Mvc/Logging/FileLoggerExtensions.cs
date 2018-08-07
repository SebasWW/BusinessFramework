using Microsoft.Extensions.Logging;
 
namespace Tnomer.AspNetCore.Mvc.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFileProvider(this ILoggerFactory factory,
                                        string directory)
        {
            factory.AddProvider(new FileLoggerProvider(directory));
            return factory;
        }
    }
}