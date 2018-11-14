using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestEAS
{
    [TestClass]
    public class MrMapiTester
    {
        [TestMethod]
        [DeploymentItem("MrMapi.exe")]
        [DeploymentItem("MrMapi_x64.exe")]
        public void MimeToMapiConversion()
        {
            throw new NotImplementedException();
        }
    }
}
