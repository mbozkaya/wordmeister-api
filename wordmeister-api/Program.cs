using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;

namespace wordmeister_api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string connectionstring = "User ID =wordmeister;Password=!wordmeister123;Server=localhost;Port=5432;Database=wordmeister-db2;Integrated Security=true;Pooling=true;";

            string tableName = "Logs";

            //Used columns (Key is a column name) 
            //Column type is writer's constructor parameter
            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
                        {
                            {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                            {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                            {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                            {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                            {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                            {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                            {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                            {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") }
                        };

            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Debug()
                                .WriteTo.PostgreSQL(connectionstring, tableName, columnWriters)
                                .CreateLogger();

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
            Log.Debug("Starting web host");
            Log.Information("Starting web host");
            Log.Error("Starting web host");
            Log.Verbose("Starting web host");
            Log.Fatal("Starting web host");
            Log.Warning("Starting web host");
            Log.CloseAndFlush();
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .UseSerilog();
    }
}
