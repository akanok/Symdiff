
namespace Symdiff
{

    public class Ln : SimpleFunction
    {
        public Ln(Functions arg) : base(arg) {}

        public override Functions differentiate() => new FunctionsDiv( arg.differentiate(), arg );

        public override string ToString() => "ln(" + base.arg + ")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Ln fun = (Ln) obj;
            return arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "ln".GetHashCode() + arg.GetHashCode();
    }


    public class Exp : SimpleFunction
    {
        public Exp(Functions arg) : base(arg) {}

        public override Functions differentiate() => new FunctionsMul( this, arg.differentiate() );

        public override string ToString() => "e^(" + base.arg + ")";

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Exp fun = (Exp) obj;
            return arg.Equals(fun.arg);
        }
            
        public override int GetHashCode() => "exp".GetHashCode() + arg.GetHashCode();
    }
    
}