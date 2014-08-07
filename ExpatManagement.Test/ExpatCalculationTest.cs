using ExpatManager.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace ExpatManagement.Test
{


    /// <summary>
    ///This is a test class for ExpatCalculationTest and is intended
    ///to contain all ExpatCalculationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExpatCalculationTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ProRataPayment
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        public void ProRataPaymentTest()
        {
            DateTime StartDate = new DateTime(2012, 07, 09); // TODO: Initialize to an appropriate value
            Decimal MonthlyPayment = new Decimal(2276.70); // TODO: Initialize to an appropriate value
            Decimal expected = new Decimal(2351.55); // TODO: Initialize to an appropriate value
            Decimal actual;
            actual = ExpatCalculation.ProRataPayment(StartDate, MonthlyPayment);
            Assert.AreEqual(expected, Decimal.Round(actual, 2));
            //Assert.Inconclusive("Verify the correctness of this test method.");
        }



        /// <summary>
        ///A test for FinalPayment
        ///</summary>
        // TODO: Ensure that the UrlToTest attribute specifies a URL to an ASP.NET page (for example,
        // http://.../Default.aspx). This is necessary for the unit test to be executed on the web server,
        // whether you are testing a page, web service, or a WCF service.
        [TestMethod()]
        public void FinalPaymentTest()
        {
            Decimal MonthlyPayment = new Decimal(2276.70); // TODO: Initialize to an appropriate value
            Decimal ProRataPayment = new Decimal(2351.55); // TODO: Initialize to an appropriate value
            //Decimal expected = new Decimal(2201.85); // TODO: Initialize to an appropriate value
            Decimal expected = new Decimal(2201.85); // TODO: Initialize to an appropriate value
            Decimal actual;
            actual = ExpatCalculation.FinalPayment(MonthlyPayment, ProRataPayment);
            Assert.AreEqual(expected, actual);
            //Assert.IsTrue();
        }
    }
}
