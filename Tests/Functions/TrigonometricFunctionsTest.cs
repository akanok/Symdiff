
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symdiff;

namespace differentiateTests
{
    [TestClass]
    public class TrigonometricFunctionsTest
    {
        private readonly Variable var;
        private readonly Sin sinx;
        private readonly Cos cosx;
        private readonly Tan tanx;
        private readonly ASin asinx;
        private readonly ACos acosx;
        private readonly ATan atanx;

        public TrigonometricFunctionsTest()
        {
            var = new Variable('x');
            sinx = new Sin(var);
            cosx = new Cos(var);
            tanx = new Tan(var);
            asinx = new ASin(var);
            acosx = new ACos(var);
            atanx = new ATan(var);
        }


        [TestMethod]
        public void SinDifferentiateTest()
        {
           Assert.IsTrue(
            sinx.differentiate()
                .Equals(
                    new FunctionsMul(
                        new Cos(var),
                        var.differentiate()
                    ) ) );
        }

        [TestMethod]
        public void CosDifferentiateTest()
        {
           Assert.IsTrue(
            cosx.differentiate()
                .Equals(
                    new FunctionsMul(
                        new FunctionsMul( new Constant(-1), new Sin(var) ),
                        var.differentiate()
                    ) ) );
        }

        [TestMethod]
        public void TanDifferentiateTest()
        {
           Assert.IsTrue(
            tanx.differentiate()
                .Equals(
                    new FunctionsDiv(
                        var.differentiate(),
                        new FunctionsPow( new Cos(var), new Constant(2) )
                    ) ) );
        }

        [TestMethod]
        public void ASinDeivateTest()
        {
           Assert.IsTrue(
            asinx.differentiate()
                .Equals(
                    new FunctionsDiv(
                        var.differentiate(),
                        new FunctionsPow(
                            new FunctionsSub( new Constant(1), new FunctionsPow( var, new Constant(2) ) ),
                            new Constant(1.0/2.0)
                        ) ) ) );
        }

        [TestMethod]
        public void ACosDifferentiateTest()
        {
           Assert.IsTrue(
            acosx.differentiate()
                .Equals(
                    new FunctionsMul(
                        new Constant(-1),
                        new FunctionsDiv(
                                var.differentiate(),
                                new FunctionsPow(
                                    new FunctionsSub(new Constant(1), new FunctionsPow( var, new Constant(2) ) ),
                                    new Constant(1.0/2.0) )
                        ) ) ) );
        }

        [TestMethod]
        public void ATanDifferentiateTest()
        {
           Assert.IsTrue(
            atanx.differentiate()
                .Equals(
                    new FunctionsDiv(
                        var.differentiate(),
                        new FunctionsSum(
                            new FunctionsPow( var, new Constant(2) ),
                            new Constant(1)
                        ) ) ) );
        }
    }
}