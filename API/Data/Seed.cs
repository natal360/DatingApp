using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class Seed
  {
    public static async Task SeedUsers(DataContext context)
    {
      // ユーザーが登録されている場合何もしない
      if (await context.Users.AnyAsync()) return;

      // シードの取得
      var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
      // List<AppUser>型に変換
      var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
      // Listに登録
      foreach (var user in users)
      {
        // ユーザー名とパスワードを jsonデータから上書きして登録
        using var hmac = new HMACSHA512();

        user.UserName = user.UserName.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
        user.PasswordSalt = hmac.Key;

        context.Users.Add(user);
      }

      await context.SaveChangesAsync();
    }
  }
}