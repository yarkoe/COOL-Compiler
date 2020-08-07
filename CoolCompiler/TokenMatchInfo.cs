namespace CoolCompiler
{
    public class TokenMatchInfo
    {
        public bool IsMatch { get; set; }
        public string RemainingText { get; set; }
        public Token Token { get; set; }
    }
}