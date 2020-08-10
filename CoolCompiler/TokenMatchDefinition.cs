using System;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace CoolCompiler
{
    public class TokenMatchRule
    {
        public ImmutableList<RuleState> States;
        public Func<string, IToken> Value;
    }

    public class TokenMatchDefinition
    {
        private Regex _regex;
        public TokenMatchRule Rule;

        public TokenMatchDefinition(string regexString, TokenMatchRule rule)
        {
            _regex = new Regex(regexString);
            Rule = rule;
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
                CurrentText = match.Value,
                RemainingText = remainingText
            };
        }
    }
}