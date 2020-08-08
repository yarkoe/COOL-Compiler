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
        public int Value { get; }

        public Token(TokenType tokenType, int value = -1)
        {
            TokenType = tokenType;
            Value = value;
        }
    }

    public class NullToken : IToken
    {
    }
}
