namespace EnglishClass.Models
{
    public class Word
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LanguageId { get; set; }
        public string WordText { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
