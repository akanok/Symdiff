
namespace Symdiff
{

    public interface Functions
    {
        public Functions differentiate(); 
    }

    public abstract class SimpleFunction : Functions
    {
        protected Functions arg;

        public SimpleFunction(Functions arg) => this.arg = arg;

        public abstract Functions differentiate();
    }


    public abstract class Function : Functions
    {
        protected Functions fun1;
        protected Functions fun2;

        public Function(Functions fun1, Functions fun2)
        {
            this.fun1 = fun1;
            this.fun2 = fun2;
        }

        public abstract Functions differentiate();
    }



}

