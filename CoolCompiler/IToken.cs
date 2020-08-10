using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoolCompiler
{
    public interface IToken
    {
    }
    public class Token : IToken
    {
        public TokenType TokenType { get; }
        
        public int Index { get; }
        public bool BoolValue { get; }
        public string ErrorMsg { get; }

        public Token(TokenType tokenType)
        {
            TokenType = tokenType;
        }
        
        public Token(TokenType tokenType, int index)
        {
            TokenType = tokenType;
            Index = index;
        }
        
        public Token(TokenType tokenType, bool boolValue)
        {
            TokenType = tokenType;
            BoolValue = boolValue;
        }
        
        public Token(TokenType tokenType, string errorMsg)
        {
            TokenType = tokenType;
            ErrorMsg = errorMsg;
        }
    }

    public class NullToken : IToken
    {
    }
}
