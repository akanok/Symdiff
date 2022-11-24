
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symdiff;

namespace differentiateTests
{
    [TestClass]
    public class OperationsTest
    {
        private readonly Variable var;
        private readonly Constant c;
        private readonly FunctionsSum sum;
        private readonly FunctionsSub sub;
        private readonly FunctionsMul mul;
        private readonly FunctionsDiv div;
        private readonly FunctionsPow pow;

        public OperationsTest()
        {
            var = new Variable('x');
            c = new Constant(3);
            sum = new FunctionsSum(var,c);
            sub = new FunctionsSub(c,var);
            mul = new FunctionsMul(c,var);
            div = new FunctionsDiv(var,c);
            pow = new FunctionsPow(var,c);
        }


        [TestMethod]
        public void SumTest()
        {
            Assert.IsTrue(
                sum.differentiate()
                    .Equals(
                        new FunctionsSum( var.differentiate(), c.differentiate() )
                    )
            );
        }

        [TestMethod]
        public void SubTest()
        {
            Assert.IsTrue(
                sub.differentiate()
                    .Equals(
                        new FunctionsSub( c.differentiate(), var.differentiate() )
                    )
            );
        }

        [TestMethod]
        public void MulTest()
        {
            Assert.IsTrue(
                mul.differentiate()
                    .Equals(
                        new FunctionsSum(
                            new FunctionsMul( c.differentiate(), var ),
                            new FunctionsMul( c, var.differentiate() ) 
                        )
                    )
            );
        }

        [TestMethod]
        public void DivTest()
        {
            Assert.IsTrue(
                div.differentiate()
                    .Equals(
                        new FunctionsDiv(
                            new FunctionsSub(
                                new FunctionsMul( var.differentiate(), c ),
                                new FunctionsMul( var, c.differentiate() )
                            ),
                            new FunctionsPow( c, new Constant(2) )
                        ) ) );
            
            var ex = Assert.ThrowsException<ArgumentException>(() => new FunctionsDiv(var,new Constant(0)));
            Assert.AreEqual(ex.Message,"You can NOT divide by zero");
        }

        [TestMethod]
        public void PowTest()
        {
            Assert.IsTrue(
                pow.differentiate()
                    .Equals(
                        new FunctionsMul(
                            new FunctionsMul(
                                c,
                                new FunctionsPow(var, new FunctionsSub( c, new Constant(1) )) ),
                            var.differentiate()
                        ) ) );
        }

    }
}