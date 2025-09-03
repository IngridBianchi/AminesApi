namespace SharedLibrary.Models
{
  public class Child
  {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public string? ImageURL { get; set; }
  }
}