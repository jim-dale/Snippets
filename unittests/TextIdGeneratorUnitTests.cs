using System.Diagnostics;
using __Snippets__;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnippetsUnitTests
{
    [TestClass]
    public class TextIdGeneratorUnitTests
    {
        [TestMethod]
        public void TextIdGenerator_GetNextId_ReturnsValidString()
        {
            var sut = new TextIdGenerator();

            var id = sut.GetNextId();

            Debug.WriteLine(id);

            Assert.AreEqual(13, id.Length);
            Assert.IsFalse(string.IsNullOrWhiteSpace(id));
        }

        [TestMethod]
        public void TextIdGenerator_GetTwoIds_ReturnsDifferentStrings()
        {
            var sut = new TextIdGenerator();

            var id1 = sut.GetNextId();
            var id2 = sut.GetNextId();

            Debug.WriteLine(id1);
            Debug.WriteLine(id2);

            Assert.AreNotEqual(id1, id2);
        }

        [TestMethod]
        public void TextIdGenerator_WithZeroSeed_Returns0000000000001()
        {
            var sut = new TextIdGenerator(0);

            var id = sut.GetNextId();

            Assert.AreEqual("0000000000001", id);
        }

        [TestMethod]
        public void TextIdGenerator_Instance_GetNextId_ReturnsValidString()
        {
            var id = TextIdGenerator.Instance.GetNextId();

            Debug.WriteLine(id);

            Assert.AreEqual(13, id.Length);
            Assert.IsFalse(string.IsNullOrWhiteSpace(id));
        }
    }
}
