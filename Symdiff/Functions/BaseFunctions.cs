
using System;

namespace Symdiff
{

    public class Constant : Functions
    {
        private double value;

        public Constant (double value) => this.value = value;

        public Functions differentiate() {
            return new Constant(0);
        }

        public static implicit operator Constant(int value) => new Constant(value);
        public static implicit operator Constant(double value) => new Constant(value);
        public static explicit operator int(Constant c) => (int)c.value;
        public static explicit operator double(Constant c) => c.value;

        public override string ToString() => value.ToString();

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Constant c = (Constant) obj;
            return value.Equals(c.value);
        }
            
        public override int GetHashCode() => value.GetHashCode();
    }


    public class Litteral : Functions
    {
        private char name;

        public Litteral(char name) => this.name = name;

        public Functions differentiate() {
            return new Constant(0);
        }

        public static implicit operator Litteral(char name) => new Litteral(name);
        public static explicit operator char(Litteral l) => l.name;

        public override string ToString() => name.ToString();

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Litteral var = (Litteral) obj;
            return name.Equals(var.name);
        }
            
        public override int GetHashCode() => name.GetHashCode();
    }
   

    public class Variable : Functions
    {
        private string name;

        public Variable(string name) => this.name = name;
        public Variable(char name)   => this.name = Char.ToString(name);

        public Functions differentiate() {
            return new Constant(1);
        }

        public static implicit operator Variable(char name) => new Variable(name.ToString());
        public static implicit operator Variable(string name) => new Variable(name);

        public override string ToString() => name;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Variable var = (Variable) obj;
            return name.Equals(var.name);
        }
            
        public override int GetHashCode() => name.GetHashCode();
    }


    public class Pi : Functions
    {
        public Functions differentiate() => new Constant(0);

        public override string ToString() => "pi";

        public override bool Equals(object? obj) => !(obj == null || GetType() != obj.GetType());
        public override int GetHashCode() => "PI".GetHashCode();
    }

}

