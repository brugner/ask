namespace Ask.Models
{
    public class Answer
    {
        public string Text { get; set; } = default!;
        public bool Success { get; set; }

        public Answer(bool success, string text)
        {
            Success = success;
            Text = text;
        }

        public static Answer Error(string text)
        {
            return new Answer(false, text);
        }

        public static Answer Ok(string text)
        {
            return new Answer(true, text);
        }
    }
}
