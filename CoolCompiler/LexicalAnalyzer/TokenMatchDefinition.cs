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

            return new TokenMatchInfo()
            {
                IsMatch = true,
                MatchText = match.Value,
            };
        }
    }
}