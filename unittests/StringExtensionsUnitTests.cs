using System;
using __Snippets__;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnippetsUnitTests
{
    [TestClass]
    public class StringExtensionsUnitTests
    {
        [TestMethod]
        public void WithEnvionmentVariables_StringHasEnvironmentVariables_ReturnsModifiedNonEmptyString()
        {
            string input = "%OS%";

            var actual = input.WithEnvionmentVariables();

            Assert.IsFalse(String.IsNullOrWhiteSpace(actual));
            Assert.AreNotEqual(input, actual);
        }

        [TestMethod]
        public void WithEnvionmentVariables_StringDoesNotHaveEnvionmentVariables_ReturnsInputString()
        {
            string input = "Helo World!";

            var actual = input.WithEnvionmentVariables();

            Assert.AreEqual(input, actual);
        }

        class FormatWithSource
        {
            public int IntValue { get; } = 101;
            public string StringValue { get; } = "Hello World!";
        }

        [TestMethod]
        public void WithFormat_StringHasSourceProperties_ReturnsFormattedString()
        {
            var source = new FormatWithSource();
            var input = "int={IntValue},string={StringValue}";

            var actual = input.FormatWith(source);

            Assert.AreEqual("int=101,string=Hello World!", actual);
        }

        [TestMethod]
        public void WithFormat_StringDoesNotHaveSourceProperties_ReturnsInputString()
        {
            var source = new FormatWithSource();
            var input = "int=202,string=Goodbye World!";

            var actual = input.FormatWith(source);

            Assert.AreEqual(input, actual);
        }

        [TestMethod]
        public void WithFormat_StringWithoutMatchingSourceProperties_ReturnsInputString()
        {
            var source = new FormatWithSource();
            var input = "int=202,string={DateValue}";

            var actual = input.FormatWith(source);

            Assert.AreEqual(input, actual);
        }

        [TestMethod]
        public void WithFormat_StringWithAndWithoutMatchingSourceProperties_ReturnsFormattedString()
        {
            var source = new FormatWithSource();
            var input = "int={IntValue},string={DateValue}";

            var actual = input.FormatWith(source);

            Assert.AreEqual("int=101,string={DateValue}", actual);
        }
    }
}
