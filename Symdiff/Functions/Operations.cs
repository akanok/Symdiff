
namespace Symdiff
{

    public class FunctionsSum : Function
    {
        public FunctionsSum(Functions fun1, Functions fun2) : base(fun1,fun2) {}

        public override Functions differentiate() => new FunctionsSum( base.fun1.differentiate(), base.fun2.differentiate() );

        public override string ToString() => base.fun1 + "+" + base.fun2;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            FunctionsSum funS = (FunctionsSum) obj;
            return fun1.Equals(funS.fun1) && fun2.Equals(funS.fun2);
        }
            
        public override int GetHashCode() => "sum".GetHashCode() + fun1.GetHashCode() + fun2.GetHashCode();
    }

    public class FunctionsSub : Function
    {
        public FunctionsSub(Functions fun1, Functions fun2) : base(fun1,fun2) {}

        public override Functions differentiate() => new FunctionsSub( base.fun1.differentiate(), base.fun2.differentiate() );

        public override string ToString() => base.fun1 + "-" + base.fun2 ;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            FunctionsSub funS = (FunctionsSub) obj;
            return fun1.Equals(funS.fun1) && fun2.Equals(funS.fun2);
        }
            
        public override int GetHashCode() => "sub".GetHashCode() + fun1.GetHashCode() + fun2.GetHashCode();
    }

    public class FunctionsMul : Function
    {
        public FunctionsMul(Functions fun1, Functions fun2) : base(fun1,fun2) {}

        public override Functions differentiate()
            => new FunctionsSum(
                    new FunctionsMul( base.fun1.differentiate(), base.fun2 ),
                    new FunctionsMul( base.fun1, base.fun2.differentiate() ) 
                );

        public override string ToString() => "("+ base.fun1 +")*("+ base.fun2 +")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            FunctionsMul funM = (FunctionsMul) obj;
            return fun1.Equals(funM.fun1) && fun2.Equals(funM.fun2);
        }
            
        public override int GetHashCode() => "mul".GetHashCode() + fun1.GetHashCode() + fun2.GetHashCode();
    }

    public class FunctionsDiv : Function
    {
        public FunctionsDiv(Functions fun1, Functions fun2) : base(fun1,fun2)
        {
            if (fun2 is Constant c && c.Equals(new Constant(0)))
                throw new ArgumentException("You can NOT divide by zero");
        }

        public override Functions differentiate()
            => new FunctionsDiv(
                    new FunctionsSub(
                        new FunctionsMul( base.fun1.differentiate(), base.fun2 ),
                        new FunctionsMul( base.fun1, base.fun2.differentiate() )
                    ),
                    new FunctionsPow( base.fun2, new Constant(2) )
                );


        public override string ToString() => "("+ base.fun1 +")/("+ base.fun2 +")" ;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            FunctionsDiv funD = (FunctionsDiv) obj;
            return fun1.Equals(funD.fun1) && fun2.Equals(funD.fun2);
        }
            
        public override int GetHashCode() => "div".GetHashCode() + fun1.GetHashCode() + fun2.GetHashCode();
    }


    public class FunctionsPow : Function
    {
        public FunctionsPow(Functions fun1, Functions fun2)  : base(fun1,fun2) {}
        public FunctionsPow(Functions fun1, Constant valueToRaise) : base(fun1,valueToRaise) {}

        public override Functions differentiate()
            //  a * x^(a-1) * f'
            => new FunctionsMul(
                    new FunctionsMul(
                        base.fun2,  // base.fun2 = valueToRaise
                        new FunctionsPow(
                            fun1,
                            // TODO: check if is a const
                            new FunctionsSub( base.fun2, new Constant(1) ) // base.fun2 = valueToRaise
                        )
                    ),
                    fun1.differentiate()
                );

        public override string ToString() => "("+ base.fun1 +")^("+ base.fun2 +")" ; // base.fun2 = valueToRaise

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            FunctionsPow funP = (FunctionsPow) obj;
            return fun1.Equals(funP.fun1) && fun2.Equals(funP.fun2);
        }
            
        public override int GetHashCode() => "pow".GetHashCode() + fun1.GetHashCode() + fun2.GetHashCode();
    }


}