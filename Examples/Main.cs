
using System;
using System.Diagnostics;

using AngouriMath;

using Symdiff;


namespace MainNameSpace
{
    class App
    {         
        static void Main(string[] args)
        {

            string expr = "sin(-5x +1) * cos(ln(x))";

            Functions fun = DifferentiateFunction(expr,'x', 0);

            Console.WriteLine("Input:\t  "+ expr +"\nDifferentiate: "+ fun);
        }


        static Functions DifferentiateFunction(string expr, char var, int debug)
        {
            if (debug == 1)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                Console.WriteLine("Input Expression:  \t  "+ expr);

                expr = SimplifyExpression(expr);
                Console.WriteLine("Simplified Expression: \t  "+ expr);

                Functions fun = Parser.Parse(expr,var);
                Console.WriteLine("Parsed Function:   \t  "+ fun);

                fun = fun.differentiate();
                Console.WriteLine("Differentiated Function:\t  "+ fun);

                expr = SimplifyFunction(fun);
                Console.WriteLine("Simple Differentiate:   \t  "+ expr);

                fun = Parser.Parse(expr,var);
                Console.WriteLine("Differentiated Final Function: "+ fun);
            
                stopWatch.Stop(); 
                Console.WriteLine("\nTime to differentiate function: " + stopWatch.ElapsedMilliseconds.ToString() + " milliseconds");
                return fun;
            }
            else if (debug == 0)
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                Functions fun = Parser.Parse(
                                    SimplifyFunction( 
                                        Parser.Parse( SimplifyExpression(expr) ,var).differentiate() )
                                    ,var);
                
                stopWatch.Stop();
                Console.WriteLine("Time to differentiate function: " + stopWatch.ElapsedMilliseconds.ToString() + " milliseconds");

                return fun;
            }

            return Parser.Parse(
                SimplifyFunction( 
                    Parser.Parse( SimplifyExpression(expr) ,var).differentiate() )
                ,var);
        }

        static string SimplifyFunction(Functions fun)
        {
            #pragma warning disable 8604
            Entity expr = fun.ToString();
            #pragma warning restore 8604
            return expr.Simplify().ToString();
        }

        static string SimplifyExpression(string expr)
        {
            expr = ((Entity)expr).Simplify().ToString();
            return (expr == "NaN")? throw new ArgumentException("The expression is unrepresentable (NaN): Check your input!") : expr;
        }

    }

}