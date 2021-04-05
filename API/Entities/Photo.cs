using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
  // 
  [Table("Photos")]
  public class Photo
  {
    public int Id { get; set; }
    public string Url { get; set; }
    public bool IsMain { get; set; }
    public string PublicId { get; set; }

    // ユーザーを削除すると全て消えるようにするため追加
    // AppUser はデータベースに登録されない
    public AppUser AppUser { get; set; }
    public int AppUserId { get; set; }
  }
}