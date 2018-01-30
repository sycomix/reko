// <copyright file="IHex32ReaderTest.cs">Copyright ©  2018</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;
using Reko.ImageLoaders.IHex32;

namespace Reko.ImageLoaders.IHex32.Tests
{
    /// <summary>This class contains parameterized unit tests for IHex32Reader</summary>
    [PexClass(typeof(IHex32Reader))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestFixture]
    public partial class IHex32ReaderTest
    {
        /// <summary>Test stub for ParseHexRecord(String, Int32)</summary>
        [PexMethod]
        public IHex32Record ParseHexRecordTest(
            [PexAssumeUnderTest]IHex32Reader target,
            string hexRecord,
            int lineNum
        )
        {
            IHex32Record result = target.ParseHexRecord(hexRecord, lineNum);
            return result;
            // TODO: add assertions to method IHex32ReaderTest.ParseHexRecordTest(IHex32Reader, String, Int32)
        }
    }
}
