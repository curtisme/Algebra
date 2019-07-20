using System;

namespace Algebra
{
    public abstract class RingElement
    {
        public static RingElement operator +(RingElement a, RingElement b)
        {
            return a.Add(b);
        }

        public static RingElement operator *(RingElement a, RingElement b)
        {
            return a.Multiply(b);
        }

        public static RingElement operator -(RingElement a, RingElement b)
        {
            return a + (-b);
        }

        public static RingElement operator -(RingElement el)
        {
            return el.GetAdditiveInverse();
        }

        public abstract RingElement Add(RingElement el);
        public abstract RingElement Multiply(RingElement el);
        public abstract RingElement GetAdditiveInverse();
    }

    public abstract class FieldElement : RingElement
    {
        public static RingElement operator /(FieldElement a, FieldElement b)
        {
            return a * b.GetMultiplicativeInverse();
        }

        public abstract FieldElement GetMultiplicativeInverse();
    }
}
