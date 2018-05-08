using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsTechFeeEvalApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsTechFeeEvalApp.Tests.Controllers
{
    [TestClass]
    class PeriodsControllerTest
    {
        [TestMethod]
        public void TestIndexView()
        {
            var controller = new PeriodsController();
            var result = controller.Index();
            Assert.AreEqual("Index", result);
        }
    }
}
