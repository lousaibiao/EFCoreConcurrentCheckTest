using System;
using EFCoreDemo.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Xunit.Abstractions;

namespace EFCoreDemo.Test
{
    public class DatabaseFixture : IDisposable
    {
        public static readonly LoggerFactory MyLoggerFactory
            = new LoggerFactory(new[] {new ConsoleLoggerProvider((_, __) => true, true)});

        public JhwContext JhwContext { get; private set; }


        public DatabaseFixture()
        {
            Console.WriteLine("init DatabaseFixture");
            var dbOptions = ConfigureDBContenxt().Options;
            
            JhwContext = new JhwContext(dbOptions);
        }

        public void Dispose()
        {
        }

        DbContextOptionsBuilder<JhwContext> ConfigureDBContenxt()
        {
            string connStr = Environment.GetEnvironmentVariable("MYSQLCONNSTR");
            Console.WriteLine($"连接字符串是:{connStr}");
            var optionsBuilder = new DbContextOptionsBuilder<JhwContext>();
            optionsBuilder.UseLoggerFactory(MyLoggerFactory);
            optionsBuilder.UseMySql(connStr, builder =>
            {
                builder.ServerVersion("5.7.24");
                builder.AnsiCharSet(CharSet.Utf8mb4);
                builder.CharSetBehavior(CharSetBehavior.AppendToAllUnicodeColumns);
                builder.UnicodeCharSet(CharSet.Utf8mb4);
            });
            optionsBuilder.EnableSensitiveDataLogging();
            return optionsBuilder;
        }
    }
}