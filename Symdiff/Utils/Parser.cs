
using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Symdiff
{
    public static class Parser
    {
        public static Functions Parse(string expr, char var)
        {
            Queue<string> postfixExpr = new Queue<string>();
            Stack<string> operatorsStack = new Stack<string>();

            expr = preParse(expr);

            for(int i=0; i<expr.Length; i++)
            {
                string c = expr[i].ToString();
                if (c == "(") operatorsStack.Push(c);
                else if ( c == ")" ) 
                {
                    while (operatorsStack.Count > 0 && operatorsStack.Peek() != "(")
                        postfixExpr.Enqueue( operatorsStack.Pop());
                    
                    if (operatorsStack.Count > 0 && operatorsStack.Peek() != "(") throw new ArgumentException("Invalid Expression");
                    else operatorsStack.Pop();
                }
                else if ( c == var.ToString() ) postfixExpr.Enqueue(c);
                else if ( c.All(Char.IsDigit) )
                {
                    int constLen = constantLength(expr.Substring(i));
                    postfixExpr.Enqueue( expr.Substring(i,constLen) );
                    i += constLen -1;
                }
                else if (c == "+" || c == "-" || c == "*" || c == "/" || c == "^")
                { 
                    if (c == "-" && Char.IsDigit(expr[i+1]) && (i==0 || expr[i-1]=='('))
                    {
                        int constLen = constantLength(expr.Substring(i+1));
                        postfixExpr.Enqueue( expr.Substring(i,constLen+1) );
                        i += constLen;
                        continue;
                    }

                    while ( operatorsStack.Count > 0 && operatorsStack.Peek() != "(" && 
                            operatorPrecedence(c) <= operatorPrecedence(operatorsStack.Peek()) )
                        postfixExpr.Enqueue( operatorsStack.Pop().ToString() );

                    operatorsStack.Push(c);
                }
                else
                {
                    int substringLength = expr.Substring(i).Length;
                    string s;
                    if ( substringLength >= 9) // arcsin(.)
                    {
                        s = expr.Substring(i,7);
                        if ( s == "arcsin(" || s == "arccos(" || s == "arctan(") { operatorsStack.Push( s = expr.Substring(i,6) ); i+=5; continue;}
                    }
                    if ( substringLength >= 7 && expr.Substring(i,5) == "sqrt(" ) { operatorsStack.Push( expr.Substring(i,4) ); i+=3; continue;} // sqrt(.)
                    if ( substringLength >= 6 ) // sin(.)
                    {
                        s = expr.Substring(i,4);
                        if ( s == "sin(" || s == "cos(" || s == "tan(") { operatorsStack.Push( expr.Substring(i,3) ); i+=2; continue;}
                    }                    
                    if ( substringLength >= 5) // ln(.) | e^(.)
                    {
                        s = expr.Substring(i,3);
                        if ( s == "ln(" || s == "e^(" ) { operatorsStack.Push( expr.Substring(i,2) ); i+=1; continue;}
                    }
                    if ( substringLength >= 3) // e^.
                    {
                        s = expr.Substring(i,2);
                        if ( s == "e^" ) { operatorsStack.Push(s); i+=1; continue;}
                    }
                    if ( substringLength >=2 ) // pi | ERROR
                    {
                        if ( expr.Substring(i,2) == "pi" ) { operatorsStack.Push("pi"); i+=1; continue;}

                        s = expr.Substring(i);
                        int l = 0;
                        while (l<s.Length && ('a' <= s[l] && s[l] <= 'z') )  l++;

                        if ( l > 1 )
                            throw new ArgumentException("Not a valid substring '"+ expr.Substring(i,l) +"' starting at position "+ i );
                        
                        i += l-1;
                    }
                    postfixExpr.Enqueue(c);
                }
            }

            while (operatorsStack.Count > 0)
                postfixExpr.Enqueue( operatorsStack.Pop().ToString() );
         
            return buildFunctionsTree(postfixExpr, var.ToString());
        }

        private static int constantLength(string expr)
        {
            int l = 0;
            while (l<expr.Length && (Char.IsDigit(expr[l]) || expr[l] == '.' || expr[l] == ',')) l++;
            return l;
        }

        private static string preParse(string expr)
        {
            expr = expr.Replace(" ", "")
                        .Replace("asin", "arcsin")
                        .Replace("acos", "arccos")
                        .Replace("atan", "arctan")
                        .Replace("log", "ln")
                        .Replace("exp", "e^")
                        .ToLower();
            expr = Regex.Replace(expr, @"(\(|^)(-{0,1})([0-9]+)([a-z])", "$1$2$3*$4");
            expr = Regex.Replace(expr, @"(\(|^)(-)([^0-9])", "$1(-1)*$3");
            return (expr[0]=='+')? expr.Substring(1) : expr;
        }

        private static int operatorPrecedence(string op) => op switch
        {
            "+" => 1,
            "-" => 1,
            "*" => 2,
            "/" => 2,
            "^" => 3,
            var val when new Regex(@"^[a-z]+$"/*, RegexOptions.Compiled */).IsMatch(val) => 10,
             _  => throw new ArgumentException(op + " Is not a valid operator"),
        };

        private static Functions buildFunctionsTree(Queue<string> postfixExpr, string var)
        {
            Stack<Functions> outStack = new Stack<Functions>();

            foreach (var s in postfixExpr)
            {
                if (s == "+" || s == "-" || s == "*" || s == "/" || s == "^")
                {
                    Functions f1 = outStack.Pop();
                    Functions f2 = outStack.Pop();
                    switch (s)
                    {
                        case "+":
                            outStack.Push( new FunctionsSum(f2,f1) );
                            break;
                        case "-":
                            outStack.Push( new FunctionsSub(f2,f1) );
                            break;
                        case "*":
                            outStack.Push( new FunctionsMul(f2,f1) );
                            break;
                        case "/":
                            outStack.Push( new FunctionsDiv(f2,f1) );
                            break;
                        case "^":
                            outStack.Push( new FunctionsPow(f2,f1) );
                            break;
                        default: throw new ArgumentException("Invalid operator: " + s);
                    }
                }
                else
                {
                    if ( s == var ) outStack.Push( new Variable(var) );
                    else if ( Regex.IsMatch(s, @"^[-]{0,1}[0-9\.,]*$") ) outStack.Push( new Constant(Double.Parse(s)) );
                    //else if ( s == "pi" ) outStack.Push( new Pi() );
                    else if ( s.Length == 1 && ('a' <= Char.Parse(s) && Char.Parse(s) <= 'z') ) outStack.Push( new Litteral(Char.Parse(s)) );
                    else
                    {
                        switch (s)
                        {
                            case "arcsin":
                                outStack.Push( new ASin( outStack.Pop() ) );
                                break;
                            case "arccos":
                                outStack.Push( new ACos( outStack.Pop() ));
                                break;
                            case "arctan":
                                outStack.Push( new ATan( outStack.Pop() ) );
                                break;
                            case "sin":
                                outStack.Push( new Sin( outStack.Pop() ) );
                                break;
                            case "cos":
                                outStack.Push( new Cos( outStack.Pop() ));
                                break;
                            case "tan":
                                outStack.Push( new Tan( outStack.Pop() ) );
                                break;
                            case "ln":
                                outStack.Push( new Ln( outStack.Pop() ) );
                                break;
                            case "e^":
                                outStack.Push( new Exp( outStack.Pop() ) );
                                break;
                            case "pi":
                                outStack.Push( new Pi() );
                                break;
                            case "sqrt":
                                outStack.Push( new FunctionsPow(outStack.Pop(), new Constant(1.0/2.0)) );
                                break;
                            default:
                                throw new ArgumentException(s + " is NOT a valid input");
                        }
                    }
                }
                
            }
            return outStack.Pop();
        }


    }
}