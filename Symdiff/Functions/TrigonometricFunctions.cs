
namespace Symdiff
{

    public class Sin : SimpleFunction
    {
        public Sin(Functions arg) : base(arg) {}

        public override Functions differentiate() => new FunctionsMul( new Cos(arg), arg.differentiate() );

        public override string ToString() => "sin(" + base.arg + ")";
        
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Sin fun = (Sin) obj;
            return base.arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "sin".GetHashCode() + arg.GetHashCode();
    }

    public class Cos : SimpleFunction
    {
        public Cos(Functions arg) : base(arg) {}

        public override Functions differentiate()
            => new FunctionsMul(
                        new FunctionsMul( new Constant(-1), new Sin(arg) ),
                        arg.differentiate() );

        public override string ToString() => "cos(" + base.arg + ")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Cos fun = (Cos) obj;
            return base.arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "cos".GetHashCode() + arg.GetHashCode();
    } 
    
    public class Tan : SimpleFunction
    {
        public Tan(Functions arg) : base(arg) {}

        public override Functions differentiate()
            => new FunctionsDiv(
                    arg.differentiate(),
                    new FunctionsPow( new Cos(arg), new Constant(2) ) );

        public override string ToString() => "tan(" + base.arg + ")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Tan fun = (Tan) obj;
            return base.arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "tan".GetHashCode() + arg.GetHashCode();
    }


    public class ASin : SimpleFunction
    {
        public ASin(Functions arg) : base(arg) {}

        public override Functions differentiate()
            => new FunctionsDiv(
                    arg.differentiate(),
                    new FunctionsPow(
                        new FunctionsSub( new Constant(1), new FunctionsPow( arg, new Constant(2) ) ),
                        new Constant(1.0/2.0)
                    ) );

        public override string ToString() => "arcsin(" + base.arg + ")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ASin fun = (ASin) obj;
            return base.arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "asin".GetHashCode() + arg.GetHashCode();
    }

    public class ACos : SimpleFunction
    {
        public ACos(Functions arg) : base(arg) {}

        public override Functions differentiate()
            => new FunctionsMul(
                    new Constant(-1),
                    new FunctionsDiv(
                        arg.differentiate(),
                        new FunctionsPow(
                            new FunctionsSub(new Constant(1), new FunctionsPow( arg, new Constant(2) ) ),
                            new Constant(1.0/2.0) )
                    ) );

        public override string ToString() => "arccos(" + base.arg + ")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ACos fun = (ACos) obj;
            return base.arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "acos".GetHashCode() + arg.GetHashCode();
    } 

    public class ATan : SimpleFunction
    {
        public ATan(Functions arg) : base(arg) {}

        public override Functions differentiate()
            => new FunctionsDiv(
                    arg.differentiate(),
                    new FunctionsSum(
                        new FunctionsPow( arg, new Constant(2) ),
                        new Constant(1)
                    ) );

        public override string ToString() => "arctan(" + base.arg + ")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            ATan fun = (ATan) obj;
            return base.arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "atan".GetHashCode() + arg.GetHashCode();
    }

}

