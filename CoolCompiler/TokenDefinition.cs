using System;
using System.Text.RegularExpressions;

namespace CoolCompiler
{
    public class TokenDefinition
    {
        private readonly Regex _regex;
        private readonly Func<Token> _rule;

        public TokenDefinition(Regex regex, Func<Token> rule)
        {
            _regex = regex;
            _rule = rule;
        }

        public TokenMatchInfo Match(string inputText)
        {
            var match = _regex.Match(inputText);

            if (!match.Success)
            {
                return new TokenMatchInfo()
                {
                    IsMatch = false
                };
            }

            var remainingText = string.Empty;
            if (match.Length != inputText.Length)
            {
                remainingText = inputText.Substring(match.Length);
            }

            return new TokenMatchInfo()
            {
                IsMatch = true,
                RemainingText = remainingText,
                Token = _rule()
            };
        }
    }
}