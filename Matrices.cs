using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

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
                Matrix<T> B = el as Matrix<T>;
                if (B == null)
                    throw new Exception("Ring element cannot be cast as Matrix!");
                if (B.M != M || B.N != N)
                    throw new Exception("Invalid dimensions for matrix addition!");
                T[][] newElements = new T[M][];
                for (int i=0;i<M;i++)
                {
                    newElements[i] = new T[N];
                    for(int j=0;j<N;j++)
                        newElements[i][j] = (T)(elements[i][j] + B.GetElement(i,j));
                }
                return new Matrix<T>(newElements);
            }

            public override RingElement Multiply(RingElement el)
            {
                Matrix<T> B = el as Matrix<T>;
                if (B == null)
                    throw new Exception("Ring element cannot be cast as Matrix!");
                if (N != B.M)
                    throw new Exception("Invalid dimensions for matrix multiplication!");
                T[][] newElements = new T[M][];
                for (int i=0;i<M;i++)
                {
                    newElements[i] = new T[B.N];
                    for (int j=0;j<B.N;j++)
                    {
                        RingElement sum = elements[0][0].Zero();
                        for (int k=0;k<N;k++)
                            sum += elements[i][k] * B.GetElement(k,j);
                        newElements[i][j] = (T)sum;
                    }
                }
                return new Matrix<T>(newElements);
            }

            public override RingElement GetAdditiveInverse()
            {
                return new Matrix<T>(elements.Select(x =>
                            x.Select(y =>
                                (T)y.GetAdditiveInverse()
                                ).ToArray()
                            ).ToArray());
            }

            public override RingElement Zero()
            {
                T[][] zeros = new T[M][];
                for (int i=0;i<M;i++)
                {
                    zeros[i] = new T[N];
                    for(int j=0;j<N;j++)
                        zeros[i][j] = (T)elements[0][0].Zero();
                }
                return new Matrix<T>(zeros);
            }

            public override bool Equals(RingElement el)
            {
                return true;
            }

            public override RingElement Copy()
            {
                return new Matrix<T>(CopyElementsArray());
            }

            public Matrix<T> ScalarMultiply(T el)
            {
                return new Matrix<T>(elements.Select(x =>
                            x.Select(y =>
                                (T)(el*y)
                                ).ToArray()
                            ).ToArray());
            }

            public T GetElement(int i, int j)
            {
                if (!(i < M && j < N))
                    throw new Exception("Matrix coordinates out of bounds!");
                return elements[i][j];
            }

            protected T[][] CopyElementsArray()
            {
                T[][] copy = new T[M][];
                for (int i=0;i<M;i++)
                {
                    copy[i] = new T[N];
                    for (int j=0;j<N;j++)
                        copy[i][j] = (T)elements[i][j].Copy();
                }
                return copy;
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
            public MatrixOverField(T[][] elements) : base(elements) {}

            public MatrixOverField<T> GetRREForm()
            {
                T[][] newElements = CopyElementsArray();
		Array.Sort<T[]>(newElements, new NumLeadingZerosComparer());
                return new MatrixOverField<T>(newElements);
            }

            private void AddRowMultiple(T[][] matrix, T scalar, int row1, int row2)
            {
                for (int i=0;i<N;i++)
                    matrix[row2][i] = (T)(matrix[row2][i] + scalar*matrix[row1][i]);
            }

            private void MultiplyRowByScalar(T[][] matrix, T scalar, int row)
            {
                for(int i=0;i<N;i++)
                    matrix[row][i] = (T)(scalar*matrix[row][i]);
            }

            private class NumLeadingZerosComparer : IComparer<T[]>
            {
                private int getNumLeadingZeros(T[] array)
                {
                    int numZeros = 0;
                    if (array.Length > 0)
                    {
                        T zero = (T)array[0].Zero();
                        foreach (T thing in array)
                            if (thing.Equals(zero))
                                numZeros += 1;
                            else
                                break;
                    }
                    return numZeros;
                }

                public int Compare(T[] a, T[] b)
                {
                    Console.WriteLine(getNumLeadingZeros(a));
                    Console.WriteLine(getNumLeadingZeros(b));
                    return getNumLeadingZeros(a).CompareTo(getNumLeadingZeros(b));
                }
            }
        }

    public class Test
    {
        public static void Main()
        {
            Rational[][] Ts = new Rational[3][]
            {
                new Rational[3] {new Rational(0), new Rational(1),(Rational)2},
                    new Rational[3] {new Rational(1), new Rational(1),(Rational)1},
                    new Rational[3] {new Rational(0), new Rational(0),new Rational(45,7)}
            };

            MatrixOverField<Rational> M = new MatrixOverField<Rational>(Ts);
            Console.WriteLine(M.GetRREForm());
        }
    }
}
