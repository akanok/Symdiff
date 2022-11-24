
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symdiff;

namespace differentiateTests
{
    [TestClass]
    public class BaseFunctionsTest
    {
        private readonly Constant c;
        private readonly Variable var;
        private readonly Litteral l;
        private readonly Pi pi;

        public BaseFunctionsTest()
        {
            c = new Constant(5);
            var = new Variable('x');
            l = 'a';
            pi = new Pi();
        }


        [TestMethod]
        public void ConstantDifferentiateTest() => Assert.IsTrue( c.differentiate().Equals(new Constant(0)) );

        [TestMethod]
        public void VariableDifferentiateTest() => Assert.IsTrue( var.differentiate().Equals(new Constant(1)) );

        [TestMethod]
        public void LitteralDifferentiateTest() => Assert.IsTrue( l.differentiate().Equals(new Constant(0)) );

        [TestMethod]
        public void PIDifferentiateTest() => Assert.IsTrue( pi.differentiate().Equals(new Constant(0)) );
    }
}