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

    public class Rational : FieldElement
    {
        public int Numerator {get;}
        public int Denominator {get;}

        public Rational(int n, int d)
        {
            if (d == 0)
                throw new Exception("Denominator must be non-zero!");
            if ((n < 0) ^ (d < 0))
                n = -1*Math.Abs(n);
            else
                n = Math.Abs(n);
            d = Math.Abs(d);
            int gcd = Math.Abs(Utils.GCD(n,d));
            Numerator = n/gcd;
            Denominator = d/gcd;
        }

        public Rational(int n) : this(n, 1) {}

        public override RingElement Add(RingElement el)
        {
            Rational q = el as Rational;
            if (q == null)
                throw new Exception("RingElement cannot be cast as Rational!");
            return new Rational(Numerator * q.Denominator + Denominator * q.Numerator,
                    Denominator * q.Denominator);
        } 

        public override RingElement Multiply(RingElement el)
        {
            Rational q = el as Rational;
            if (q == null)
                throw new Exception("RingElement cannot be cast as Rational!");
            return new Rational(Numerator * q.Numerator, Denominator * q.Denominator);
        }

        public override RingElement GetAdditiveInverse()
        {
            return new Rational(-1*Numerator, Denominator);
        }

        public override FieldElement GetMultiplicativeInverse()
        {
            if (Numerator == 0)
                throw new Exception("Zero has no multiplicative inverse!");
            return new Rational(Denominator, Numerator);
        }

        public override string ToString()
        {
            if (Denominator == 1)
                return Numerator.ToString();
            return string.Format("({0}/{1})", Numerator, Denominator);
        }
    }
}
