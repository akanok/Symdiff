# Symdiff
A simple C# library for symbolic differentiation of single-variable functions.


### Table of contents
- [Symdiff](#symdiff)
  - [About The Project](#about-the-project)
    - [Known Limitations](#known-limitations)
    - [Example](#example)
  - [Get Started](#get-started)
    - [Prerequisites](#prerequisites)
    - [Try it your self!](#try-it-your-self)
  - [License](#license)

## About The Project
**Symdiff** allows you to differentiate single-variable functions very easily:
you just give it a string with your expression, it will parse your input and build objects and then you can differentiate!

The library has a built in parser that will build an objects tree from your input string.  
You can differentiate *addition*, *subtraction*, *multiplication*, *division*, and *exponentiation* of *Numbers*, *Parameters*, *Pi* and all this functions:

| Supported functions |
|---------------------|
| Sine                |
| Cosine              |
| Tangent             |
| Arcsine             |
| Arccosine           |
| Arctangent          |
| Exponential         |
| Natural Logarithm   |


What if I need a function that isn't supported?  
Just add it! You can use this template:
```C#
namespace Symdiff
{
    public class CustomFunction : SimpleFunction
    {
        public CustomFunction(Functions arg) : base(arg) {}

        public override Functions differentiate() {/*FIXME: Write your implementation*/}

        public override string ToString() {/*FIXME: Write your implementation*/}
        public override bool Equals(object? obj) {/*FIXME: Write your implementation*/}
        public override int GetHashCode() {/*FIXME: Write your implementation*/}
    }
}
```
Feel free to contribute with your functions.


### Known Limitations
- The exponentiation operation is supported only if we raise a function to a constant power, a function raised to an other function is not supported!
- A Function divided by a Constant must be written as the Constant multiplied to the Function, the parser is not able to do this.
- Because *Symdiff* does not offer simplification of expressions/functions, after just one or two differentiations your objects tree of functions can grow exponentially. A simple work around is to use an external library (like *AngouriMath*) to simplify your differentiate function, as shown in the [example](./Examples/Main.cs)


### Example
The `Examples` directory contains a proof of concept:
```C#
  string expr = "sin(-5x +1) * cos(ln(x))";
  Functions fun = DifferentiateFunction(expr,'x', 0);
  Console.WriteLine("Input:\t  "+ expr +"\nDifferentiate: "+ fun);
```
which will give you this output:
```shell
  Time to differentiate function: 604 milliseconds
  Input:    sin(-5x +1) * cos(ln(x))
  Differentiate: ((cos(ln(x)))*(cos(1+(-5)*(x))))*(-5)+(((sin(ln(x)))*(sin(1+(-5)*(x))))*(-1))/(x)
```

## Get Started
### Prerequisites
The library requires:
+ [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

Some example also requires:
+ [AngouriMath](https://github.com/asc-community/AngouriMath)
  
which can be installed with: `dotnet add package AngouriMath --prerelease`, in the `Examples` directory, as shown in the [official docs](https://am.angouri.org/quickstart/)



### Try it your self!
Choose your favourite:

**using [Microsoft official Docker image](https://hub.docker.com/_/microsoft-dotnet-sdk/)**:
1. Clone this repo
2. Create the container with:  
  `docker run -it --rm -e DOTNET_CLI_TELEMETRY_OPTOUT=1 -v /Path/to/symdiff/folder:/symdiff mcr.microsoft.com/dotnet/sdk:6.0`  
    - `-it` interactive mode
    - `--rm` remove after exited
    - `-e DOTNET_CLI_TELEMETRY_OPTOUT=1` disable Microsoft telemetry
    - `-v /Path/to/symdiff/folder:/symdiff` mount project folder inside the container
1. Once in the container move in the `Examples` folder: `cd /symdiff/Examples`
2. Run the example: `dotnet run`

**on Linux**:
1. Clone this repo: `git clone  https://github.com/akanok/Symdiff.git`
2. Move in the `Examples` folder: `cd Examples`
3. Run the example: `dotnet run`

**on Windows**:
1. Clone this repo
2. Open poweshell an go in the project directory: `chdir \Path\to\symdiff-folder`
3. Run the example: `dotnet run --project Examples\Examples.csproj`


## License
This project is licensed under MIT.  
Please see the [LICENSE](/LICENSE) file for details.
