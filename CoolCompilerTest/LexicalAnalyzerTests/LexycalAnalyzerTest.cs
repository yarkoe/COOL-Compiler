using System.Collections.Immutable;
using System.IO;
using System.Linq;
using NUnit.Framework;
using CoolCompiler;

namespace CoolCompilerTest
{
    public class Tests
    {
        private readonly string _projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        
        [Test]
        public void HelloWorldTest()
        {
            var correctTokenTypes = ImmutableList.Create<TokenType>(
                TokenType.Class, TokenType.TypeId, TokenType.Inherits, TokenType.TypeId, TokenType.OpenBrace,
                TokenType.ObjectId, TokenType.OpenParenthesis, TokenType.CloseParenthesis, TokenType.Colon,
                TokenType.TypeId, TokenType.OpenBrace, TokenType.ObjectId, TokenType.OpenParenthesis,
                TokenType.StrConst, TokenType.CloseParenthesis, TokenType.CloseBrace, TokenType.SemiColon, 
                TokenType.CloseBrace, TokenType.SemiColon
                );
            
            var text = File.ReadAllText(Path.Combine(_projectFolder, @"examples/hello_world.cl"));

            var tokenizer = new Tokenizer();

            var testTokens = tokenizer.Tokenize(text);
            
            Assert.IsTrue(correctTokenTypes.Count == testTokens.Count);

            var correctTestTokensTypeZip = correctTokenTypes.Zip(testTokens, (type, token) => new
            {
                correctTokenType = type,
                testTokenType = token.TokenType
            });

            foreach (var item in correctTestTokensTypeZip)
            {
                Assert.AreEqual(item.correctTokenType, item.testTokenType);
            }

            var type1 = tokenizer.IdTable.GetString(testTokens[1].Index);
            var type2 = tokenizer.IdTable.GetString(testTokens[3].Index);
            var object1 = tokenizer.IdTable.GetString(testTokens[5].Index);
            var string1 = tokenizer.StringTable.GetString(testTokens[13].Index);

            Assert.AreEqual("Main", type1);
            Assert.AreEqual("IO", type2);
            Assert.AreEqual("main", object1);
            Assert.AreEqual("Hello, World.\n", string1);

        }
    }
}