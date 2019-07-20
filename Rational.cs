using System;

namespace Algebra
{
    public class Utils
    {
        public static int GCD(int a, int b)
        {
            if (b == 0)
                return a;
            return GCD(b, a%b);
        }
    }
    public class Rational
    {
        public int Numerator {get;}
        public int Denominator {get;}
        
        public Rational(int n, int d)
        {
            if (d == 0)
                throw new Exception("Denominator must be non-zero!");
            int gcd = Utils.GCD(n,d);
            Numerator = n/gcd;
            Denominator = d/gcd;
        }

        public override string ToString()
        {
            if (Denominator == 1)
                return Numerator.ToString();
            return string.Format("({0}/{1})", Numerator, Denominator);
        }
    }

    public class Test
    {
        public static void Main(string[] args)
        {
            if (args.Length < 2)
                return;
            int a,b;
            if (!(int.TryParse(args[0], out a) && int.TryParse(args[1], out b)))
            {
                Console.Error.WriteLine("can't parse args as ints");
                return;
            }
            try
            {
                Console.WriteLine(new Rational(a,b));
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }
    }
}
