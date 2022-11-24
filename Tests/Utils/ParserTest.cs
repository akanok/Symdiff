using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symdiff;

namespace differentiateTests
{
    [TestClass]
    public class ParserTest
    {
        private readonly char var = 'x';
        private readonly Variable x;
        private readonly Litteral a;
        private readonly Litteral b;
        private readonly Pi pi;
        private Constant c1;
        private Constant c2;
        private ASin asin;
        private ACos acos;
        private ATan atan;
        private Sin sin;
        private Cos cos;
        private Tan tan;
        private Ln ln;
        private Exp exp;
        string expr;

        public ParserTest()
        {
            c1   = new Constant(-510);
            c2   = new Constant(7.5);
            x    = new Variable(var);
            a    = 'a';
            b    = new Litteral('b');
            pi   = new Pi();
            asin = new ASin(x);
            acos = new ACos(x);
            atan = new ATan(x);
            sin  = new Sin(x);
            cos  = new Cos(x);
            tan  = new Tan(x);
            ln   = new Ln(x);
            exp  = new Exp(x);
            expr = "";
        }

        [TestMethod]
        public void ParsingSqrtTest()
        {
            expr = " sin( sqrt(x-1) +1 ) ";
            Constant c = 1;
            FunctionsSub sub = new FunctionsSub(x,c);
            FunctionsPow sqrt = new FunctionsPow( sub, new Constant(0.5)) ;
            FunctionsSum sum = new FunctionsSum( sqrt,c);
            Assert.IsTrue( Parser.Parse(expr,var).Equals(new Sin(sum)) );
        }

        [TestMethod]
        public void ParsingNonXVariableTest()
        {
            expr = "a + x";
            Variable ax = 'a';
            Litteral xx = 'x';
            Assert.IsTrue( Parser.Parse(expr,'a').Equals( new FunctionsSum(ax,xx) ) );
        }

        [TestMethod]
        public void ParsingBaseFunctionsTest()
        {
            expr = " x ";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(x) );
            expr = "-510";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(c1) );
            expr = "7.5";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(c2) );
            
            expr = "-510 x";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(new FunctionsMul(c1,x)) );

            expr = " -a * x";
            Constant cn = -1;
            FunctionsMul mul = new FunctionsMul(cn,a);
            Assert.IsTrue( Parser.Parse(expr,var).Equals(new FunctionsMul(mul,x)) );

            expr = "pi*x-pi";
            mul = new FunctionsMul(pi,x);
            Assert.IsTrue( Parser.Parse(expr,var).Equals(new FunctionsSub(mul,pi)) );
        }

        [TestMethod]
        public void ParsingTrigonometricFunctionsTest()
        {
            expr = "sin( x)";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(sin) );
            expr = " cos(x)";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(cos) );
            expr = " tan( x ) ";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(tan) );

            expr = "asin(x)";
            Assert.IsFalse( Parser.Parse(expr,var).Equals(sin) );
            Assert.IsTrue( Parser.Parse(expr,var).Equals(asin) );
            expr = "acos(x)";
            Assert.IsFalse( Parser.Parse(expr,var).Equals(cos) );
            Assert.IsTrue( Parser.Parse(expr,var).Equals(acos) );
            expr = "arctan(x)";
            Assert.IsFalse( Parser.Parse(expr,var).Equals(tan) );
            Assert.IsTrue( Parser.Parse(expr,var).Equals(atan) );
        }

        [TestMethod]
        public void ParsingLogExpFunctionsTest()
        {
            expr = " ln(x) ";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(ln) );
            expr = "log(x)";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(ln) );

            expr = "e^x";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(exp) );
            expr = "exp(x)";
            Assert.IsTrue( Parser.Parse(expr,var).Equals(exp) );
        }

        [TestMethod]
        public void ParsingComplexExpressionTest()
        {
            expr = "asin (ln(7.5 *e^(x - 9)))* sin(acos( x ^ 2) )";

            c1 = new Constant(9);
            Functions x1 = new FunctionsSub(x,c1);
            exp = new Exp(x1);
            Functions e5 = new FunctionsMul(c2,exp);
            ln = new Ln(e5);
            asin = new ASin(ln);
            Assert.IsTrue( Parser.Parse("asin (ln(7.5 *e^(x - 9)))",var).Equals(asin) );

            Constant c3 = new Constant(2);
            Functions xx = new FunctionsPow(x,c3);
            acos = new ACos(xx);
            sin = new Sin(acos);
            Assert.IsTrue( Parser.Parse("sin(acos( x ^ 2) )",var).Equals(sin) );

            Functions fun = new FunctionsMul(asin,sin);
            Assert.IsTrue( Parser.Parse(expr,var).Equals(fun) );



            expr = "+asin(sin(ln(e^x))) + arcsin(cos(arccos(x))) - 5 * atan(x^(-2))";

            exp  = new Exp(x);
            ln   = new Ln(exp);
            sin  = new Sin(ln);
            asin = new ASin(sin);
            Assert.IsTrue( Parser.Parse("+asin(sin(ln(e^x)))",var).Equals(asin) );

            acos = new ACos(x);
            cos  = new Cos(acos);
            ASin asin2 = new ASin(cos);
            Assert.IsTrue( Parser.Parse("arcsin(cos(arccos(x)))",var).Equals(asin2) );
            FunctionsSum sum = new FunctionsSum(asin,asin2);
            Assert.IsTrue( Parser.Parse("+asin(sin(ln(e^x))) + arcsin(cos(arccos(x)))",var).Equals(sum) );

            c2 = new Constant(-2);
            FunctionsPow pow = new FunctionsPow(x,c2);
            atan = new ATan(pow);
            c1 = new Constant(5);
            FunctionsMul mul = new FunctionsMul(c1,atan);
            Assert.IsTrue( Parser.Parse("5 * atan(x^(-2))",var).Equals(mul) );

            fun = new FunctionsSub(sum,mul);
            Assert.IsTrue( Parser.Parse(expr,var).Equals(fun) );
        }

        [TestMethod]
        public void ParsingNegativeFunctionsTest()
        {
            expr = " -x -5 +(-x + x^(-3)) - x^ 5";
            c1 = new Constant(5);
            c2 = new Constant(-3);
            Constant cn = new Constant(-1);
            FunctionsMul mul = new FunctionsMul(cn,x);
            FunctionsSub sub = new FunctionsSub(mul,c1);
            Assert.IsTrue( Parser.Parse(" -x -5 ",var).Equals(sub) );

            FunctionsPow pow = new FunctionsPow(x,c2);
            FunctionsSum sum = new FunctionsSum(mul,pow);
            Assert.IsTrue( Parser.Parse("(-x + x^(-3))",var).Equals(sum) );

            FunctionsSum sum2 = new FunctionsSum(sub,sum);
            Assert.IsTrue( Parser.Parse(" -x -5 +(-x + x^(-3))",var).Equals(sum2) );

            FunctionsPow pow2 = new FunctionsPow(x,c1);
            FunctionsSub sub2 = new FunctionsSub(sum2, pow2);
            Assert.IsTrue( Parser.Parse(expr,var).Equals(sub2) );
        }

        [TestMethod]
        public void ParsingNotValidFunctionsTest()
        {
            expr = "sin(a*x +1) * caos(x^2)";
            var ex = Assert.ThrowsException<ArgumentException>(() => Parser.Parse(expr,var));
            Assert.AreEqual(ex.Message,"Not a valid substring 'caos' starting at position 11");

            expr = "sin(ax +1) * cos(x^2)";
            ex = Assert.ThrowsException<ArgumentException>(() => Parser.Parse(expr,var));
            Assert.AreEqual(ex.Message,"Not a valid substring 'ax' starting at position 4");
        }
    }
}