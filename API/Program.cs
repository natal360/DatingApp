using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
  public class Program
  {
    // void -> async Task
    //.Run() 削除 var host =  追加
    public static async Task Main(string[] args)
    {
      var host = CreateHostBuilder(args).Build();

      using var scope = host.Services.CreateScope();
      var services = scope.ServiceProvider;
      try
      {
        var context = services.GetRequiredService<DataContext>();
        // マイグレーションの実行しデータベースに保存
        await context.Database.MigrateAsync();
        // シードの呼び出し
        await Seed.SeedUsers(context);
      }
      catch (Exception ex)
      {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during migration");
      }

      // プログラムの起動
      await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
