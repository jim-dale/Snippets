using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnippetsUnitTests
{
    internal class SimpleDataTransferObject
    {
        public int IntValue { get; set; }
        public long LongValue { get; set; }
        public decimal DecimalValue { get; set; }
        public DateTime DateTimeValue { get; set; }
        public DateTimeOffset DateTimeOffsetValue { get; set; }
        public TimeSpan TimeSpanValue { get; set; }

    }

    // bool
    // byte
    // sbyte
    // char
    // decimal
    // double
    // float
    // int
    // uint
    // long
    // ulong
    // short
    // ushort

    // string
    // DateTime
    // DateTimeOffset
    // TimeSpan
    // DBNull
    // null
    // GUID
    // Enum converter



    [TestClass]
    public class ObjectExtensionsSetPropertyValueUnitTests
    {
        [TestMethod]
        public void ConvertChangeType_StringToBool_True()
        {
            var actual = Convert.ChangeType("true", typeof(bool));

            Assert.AreEqual(true, actual as bool?);
        }

        [TestMethod]
        public void ConvertChangeType_StringToBool_Null()
        {
            var actual = Convert.ChangeType("True", typeof(bool));

            Assert.AreEqual(true, actual as bool?);
        }

        [TestMethod]
        public void TypeConverterConvertFrom_StringToBool_True()
        {
            var actual = TypeDescriptor.GetConverter(typeof(bool)).ConvertFrom("True");

            Assert.AreEqual(true, actual as bool?);
        }

        [TestMethod]
        public void TypeConverterConvertFrom_StringToDataTimeOffset_ValidValue()
        {
            var converter = TypeDescriptor.GetConverter(typeof(DateTimeOffset));

            var dto = DateTimeOffset.UtcNow;
            var str = converter.ConvertTo(dto, typeof(string));

            //Assert.AreEqual(true, actual as bool?);
        }
    }
}
