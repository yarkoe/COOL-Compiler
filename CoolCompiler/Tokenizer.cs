using System.Collections.Generic;
using System.Collections.Immutable;

namespace CoolCompiler
{
    public class Tokenizer
    {
        private int _commentDepth = 0;

        private const int MaxStrConst = 1025;
        private int _stringLength = 0;
        
        private List<TokenMatchDefinition> _tokenMatchDefinitions = new List<TokenMatchDefinition>();
        
        public StringTable StringTable { get; } = new StringTable();
        public StringTable IdTable { get; } = new StringTable();
        public StringTable IntTable { get; } = new StringTable();

        public Tokenizer()
        {
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:class)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Class)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:else)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Else)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:if)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.If)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:fi)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Fi)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:in)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.In)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:Inherits)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Inherits)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:let)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Let)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:loop)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Loop)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:pool)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Pool)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:then)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Then)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:while)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.While)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:case)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Case)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:esac)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Esac)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:of)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Of)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:new)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.New)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:isvoid)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Isvoid)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:not)", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = _ => new Token(TokenType.Not)
                }));
            
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^[0-9]+", 
                new TokenMatchRule()
                {
                    StatusSet = ImmutableHashSet.Create<RuleStatus>(RuleStatus.Initial),
                    Value = (matchString) =>
                    {
                        var intIndex = IntTable.addString(matchString);

                        return new Token(TokenType.IntConst, intIndex);
                    }
                }));    
        }
    }
}