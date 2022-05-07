// Created with Janda.Go http://github.com/Jandini/Janda.Go
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Serilog;

namespace Janda.IO.RunMe
{
    class Program
    {
        static void Main()
        {
            try
            {
                using var provider = new ServiceCollection()
                    .AddTransient<IMain, Main>()
                    .AddLogging(builder => builder
                    .AddSerilog(new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger(), dispose: true))
                    .BuildServiceProvider();

                try
                {
                    provider
                        .GetRequiredService<IMain>()
                        .Run();
                }
                catch (Exception ex)
                {
                    provider.GetRequiredService<ILogger<Program>>()
                        .LogCritical(ex, ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}