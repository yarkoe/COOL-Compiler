﻿using System.Collections.Generic;
using System.Collections.Immutable;

namespace CoolCompiler
{
    public class Tokenizer
    {
        private RuleState _ruleState = RuleState.Initial;
        private int _commentDepth = 0;

        private const int MaxStrConst = 1025;
        private string _stringBuffer = string.Empty;

        private int _lineNumber = 0;

        public int LineNumber => _lineNumber;

        private List<TokenMatchDefinition> _tokenMatchDefinitions = new List<TokenMatchDefinition>();
        
        public StringTable StringTable { get; } = new StringTable();
        public StringTable IdTable { get; } = new StringTable();
        public StringTable IntTable { get; } = new StringTable();

        public Tokenizer()
        {
            /* Key words */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:class)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Class)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:else)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Else)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:if)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.If)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:fi)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Fi)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:in)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.In)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:Inherits)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Inherits)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:let)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Let)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:loop)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Loop)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:pool)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Pool)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:then)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Then)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:while)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.While)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:case)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Case)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:esac)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Esac)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:of)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Of)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(?i:new)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.New)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^t(?i:rue)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.BoolConst, true)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^f(?i:alse)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.BoolConst, false)
                }));
            
            /* Operations */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\.", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Dot)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^@", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.At)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^~", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Tilde)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^isvoid", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Isvoid)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\*", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Asterisk)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\/", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Slash)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\+", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Plus)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^-", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Minus)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^=>", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Darrow)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^<=", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Le)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^<", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.LessThan)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^=", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Equal)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^not", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Not)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^<-", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Assign)
                }));
            
            /* Braces */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^{", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.OpenBrace)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^}", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.CloseBrace)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^(", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.OpenParenthesis)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^)", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.CloseParenthesis)
                }));
            
            
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^:", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Colon)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^;", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.SemiColon)
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^,", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Comma)
                }));
   
            
            /* Integers */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^[0-9]+", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = (matchString) =>
                    {
                        var intIndex = IntTable.addString(matchString);

                        return new Token(TokenType.IntConst, intIndex);
                    }
                }));
            
            /* Type identifiers */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^[A-Z][A-Za-z0-9_]*", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = (matchString) =>
                    {
                        var idIndex = IdTable.addString(matchString);

                        return new Token(TokenType.Typeid, idIndex);
                    }
                })); 
            
            /* Object identifiers */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^[a-z][A-Za-z0-9_]*", 
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = (matchString) =>
                    {
                        var idIndex = IdTable.addString(matchString);

                        return new Token(TokenType.Objectid, idIndex);
                    }
                }));

            /* Comments */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^--.*$",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new NullToken()
                }));
            
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\(\*",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial, RuleState.Comment),
                    Value = _ =>
                    {
                        _commentDepth += 1;
                        _ruleState = RuleState.Comment;

                        return new NullToken();
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^[^\*\n]+",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Comment),
                    Value = _ => new NullToken()
                }));
            
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\*",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Comment),
                    Value = _ => new NullToken()
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\*\)",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Comment),
                    Value = _ =>
                    {
                        _commentDepth -= 1;
                        if (_commentDepth == 0)
                        {
                            _ruleState = RuleState.Initial;
                        }
                        
                        return new NullToken();
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\z",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Comment),
                    Value = _ => new Token(TokenType.Error, "Unexpected EOF in comment")
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\*\)",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => new Token(TokenType.Error, "Unmatched *)")
                }));
            
            /* String constants */
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^\"",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial),
                    Value = _ => 
                    { 
                        _ruleState = RuleState.String;
                        
                        return new NullToken(); 
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^[^\\\n\0\" + "\"]+",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = (matchString) =>
                    {
                        _stringBuffer += matchString;

                        if (IsStringTooLong())
                        {
                            return ProvokeStringLengthErrorToken();
                        }

                        _stringBuffer += matchString;

                        return new NullToken();
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\\\n",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = _ =>
                    {
                        _stringBuffer += "\n";
                        _lineNumber += 1;
                        
                        if (IsStringTooLong())
                        {
                            return ProvokeStringLengthErrorToken();
                        }

                        return new NullToken();
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\\[ntbf]",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = (matchString) =>
                    {
                        switch (matchString)
                        {
                            case @"\n": 
                                _stringBuffer += "\n";
                                break;
                            case @"\t":
                                _stringBuffer += "\t";
                                break;
                            case @"\b":
                                _stringBuffer += "\t";
                                break;
                            case @"\f":
                                _stringBuffer += "\f";
                                break;
                        }

                        if (IsStringTooLong())
                        {
                            return ProvokeStringLengthErrorToken();
                        }
                        
                        return new NullToken();
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\\",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = _ =>
                    {
                        _stringBuffer += @"\";

                        if (IsStringTooLong())
                        {
                            return ProvokeStringLengthErrorToken();
                        }

                        return new NullToken();
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\0",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = _ =>
                    {
                        _ruleState = RuleState.BrokenString;

                        return new Token(TokenType.Error, "String contains null character");
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\n",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = _ =>
                    {
                        _ruleState = RuleState.BrokenString;

                        return new Token(TokenType.Error, "Unterminated string constant");
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^.*[\\\"\\n]",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.BrokenString),
                    Value = _ =>
                    {
                        _ruleState = RuleState.Initial;

                        return new Token(TokenType.Error, "String contains null character");
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^\\\"",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = _ =>
                    {
                        var stringIndex = StringTable.addString(_stringBuffer);
                        
                        _stringBuffer = string.Empty;
                        _ruleState = RuleState.Initial;

                        return new Token(TokenType.StrConst, stringIndex);
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\z",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = _ =>
                    {
                        _ruleState = RuleState.Initial;
                        
                        return new Token(TokenType.Error, "EOF in string constant;");
                    }
                }));



            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\n",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.Initial, RuleState.Comment),
                    Value = _ =>
                    {
                        _lineNumber += 1;

                        return new NullToken();
                    }
                }));
            _tokenMatchDefinitions.Add(new TokenMatchDefinition("^.",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = matchString => new Token(TokenType.Error, matchString)
                }));
            
            _tokenMatchDefinitions.Add(new TokenMatchDefinition(@"^\s+",
                new TokenMatchRule()
                {
                    States = ImmutableList.Create<RuleState>(RuleState.String),
                    Value = _ => new NullToken()
                }));
            
        }

        private bool IsStringTooLong() => _stringBuffer.Length + 1 >= MaxStrConst;

        private Token ProvokeStringLengthErrorToken()
        {
            _stringBuffer = string.Empty;
            
            return new Token(TokenType.Error, "String constant too long");
        }
    }
}
