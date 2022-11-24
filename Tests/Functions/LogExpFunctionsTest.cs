
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symdiff;

namespace differentiateTests
{
    [TestClass]
    public class LogExpFunctionsTest
    {
        private readonly Variable var;
        private readonly Ln lnx;
        private readonly Exp expx;

        public LogExpFunctionsTest()
        {
            var = new Variable('x');
            lnx = new Ln(var);
            expx = new Exp(var);
        }


        [TestMethod]
        public void LnDifferentiateTest()
        {
            Assert.IsTrue(
                lnx.differentiate().Equals( new FunctionsDiv(var.differentiate(), var) )
            );
        }

        [TestMethod]
        public void ExpDifferentiateTest()
        {
            Assert.IsTrue( 
                expx.differentiate().Equals( new FunctionsMul(expx, var.differentiate()) )
            );
        }

    }
}