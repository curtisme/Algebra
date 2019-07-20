using System;
using System.Linq;
using System.Text;

namespace Algebra
{
    public class Matrix<T> : RingElement
        where T : RingElement
        {
            private T[][] elements;

            public int M {get;}
            public int N {get;}

            public Matrix()
            {
                elements = null;
                M = 0;
                N = 0;
            }
            public Matrix(T[][] elements)
            {
                this.elements = elements;
                M = elements.Length;
                if (M != 0)
                {
                    N = elements[0].Length;
                    foreach (T[] row in elements)
                        if (row.Length != N)
                            throw new Exception("Failed to create matrix! " +
                                    "Supplied rows are not all of the same length.");
                }
                else
                    N = 0;
            }

            public override RingElement Add(RingElement el)
            {
                return this;
            }

            public override RingElement Multiply(RingElement el)
            {
                return this;
            }

            public override RingElement GetAdditiveInverse()
            {
                return this;
            }

            public Matrix<T> ScalarMultiply(T el)
            {
                return new Matrix<T>(elements.Select(x =>
                            x.Select(y =>
                                el*y as T
                                ).ToArray()
                            ).ToArray());
            }

            public override string ToString()
            {
                if (M < 1 || N < 1)
                    return "Empty Matrix";
                StringBuilder sb = new StringBuilder();
                for (int i=0;i<M;i++)
                    for (int j=0;j<N;j++)
                        sb.Append(elements[i][j].ToString() + (j==N-1?'\n':' '));
                return sb.ToString();
            }
        }

    public class MatrixOverField<T> : Matrix<T>
        where T : FieldElement
        {
        }

    public class Test
    {
        public static void Main()
        {
            Rational[][] Ts = new Rational[2][]
            {
                new Rational[2] {new Rational(0), new Rational(1)},
                    new Rational[2] {new Rational(1,2), new Rational(0)}
            };
            Matrix<Rational> M = new Matrix<Rational>(Ts);
            Console.WriteLine(M.ScalarMultiply(new Rational(2)));
        }
    }
}
