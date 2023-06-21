using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class IntegerMultiplication
    {
        #region YOUR CODE IS HERE

        //Your Code is Here:
        //==================
        /// <summary>
        /// Multiply 2 large integers of N digits in an efficient way [Karatsuba's Method]
        /// </summary>
        /// <param name="X">First large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="Y">Second large integer of N digits [0: least significant digit, N-1: most signif. dig.]</param>
        /// <param name="N">Number of digits (power of 2)</param>
        /// <returns>Resulting large integer of 2xN digits (left padded with 0's if necessarily) [0: least signif., 2xN-1: most signif.]</returns>
        static public byte[] IntegerMultiply(byte[] X, byte[] Y, int N)
        {
            Array.Reverse(X);
            Array.Reverse(Y);
            byte[] EndResult = IntegerMultiplicationcall(X, Y, N);
            Array.Reverse(EndResult);
            return EndResult;

        }
        public static byte[] IntegerMultiplicationcall(byte[] X, byte[] Y, int N)
        {
            if (N <= 128)
            {
                int Xlen = X.Length;
                int Ylen = Y.Length;
                byte[] R = new byte[Xlen + Ylen];

                for (int i = Xlen - 1; i >= 0; i--)
                {
                    int carry = 0;
                    for (int j = Ylen - 1; j >= 0; j--)
                    {
                        int mul = X[i] * Y[j] + carry + R[i + j + 1];
                        carry = mul / 10;
                        R[i + j + 1] = (byte)(mul % 10);
                    }
                    R[i] += (byte)carry;
                }
                return R;
            }
            else
            {
                //Declare A,B,C and D and split
                byte[] A = null;
                byte[] B = null;
                byte[] C = null;
                byte[] D = null;
                byte[] M1 = null;
                byte[] M2 = null;
                byte[] Z1 = null;
                int h = N / 2;

                if (N % 2 == 0)
                {
                    B = X.Take(h).ToArray();
                    A = X.Skip(h).ToArray();
                    D = Y.Take(h).ToArray();
                    C = Y.Skip(h).ToArray();
                }
                else
                {
                    B = X.Take(h + 1).ToArray();
                    A = X.Skip(h + 1).ToArray();
                    D = Y.Take(h + 1).ToArray();
                    C = Y.Skip(h + 1).ToArray();
                }
                Parallel.Invoke(
                () =>
                {
                    //M1 = B * D
                    M1 = IntegerMultiplicationcall(B, D, B.Length);
                },

                () =>
                {
                    //M2 = A * C
                    M2 = IntegerMultiplicationcall(A, C, A.Length);
                },

                () =>
                {
                    //Z = (A + B) * (C + D)
                    byte[] q = Add(A, B);
                    byte[] w = Add(C, D);
                    int MaxLen = Math.Max(q.Length, w.Length);
                    if (q.Length < w.Length)
                    {
                        q = llzeros(q, (w.Length - q.Length));
                    }
                    else if (w.Length < q.Length)
                    {
                        w = llzeros(w, (q.Length - w.Length));
                    }
                    Z1 = IntegerMultiplicationcall(q, w, (MaxLen));
                }
                );

                //Z2 = Z - M1 - M2
                byte[] Z2 = Sub(Sub(Z1, M1), M2);
                M1 = rlzeros(M1, h * 2);
                Z2 = rlzeros(Z2, h);
                int MaxD = Math.Max(M1.Length, Z2.Length);
                M1 = llzeros(M1, MaxD - M1.Length);
                Z2 = llzeros(Z2, MaxD - Z2.Length);
                M2 = llzeros(M2, MaxD - M2.Length);

                ////R = (10 ^ (N)) * M1 + (10 ^ (N / 2)) * (Z - M1 - M2) + M2;
                byte[] R = Add(Add(M1, Z2), M2);
                if (R.Length < N * 2)
                {
                    R = llzeros(R, N - R.Length);
                }
                else if (R.Length > N * 2)
                {
                    R = R.Skip(R.Length - N * 2).ToArray();
                }
                return R;

            }
        }

        //Function Add
        public static byte[] Add(byte[] R, byte[] L)
        {
            //Declare attributes
            int len = 0;
            int sum = 0;
            byte carry = 0;
            len = Math.Max(R.Length, L.Length);
            byte[] r = new byte[len];
            byte[] rwc = new byte[len + 1];
            //check length
            if (R.Length < L.Length)
            {
                R = llzeros(R, L.Length - R.Length);
            }
            else if (L.Length < R.Length)
            {
                L = llzeros(L, R.Length - L.Length);
            }
            //function body
            for (int i = len - 1; i >= 0; i--)
            {
                sum = carry + R[i] + L[i];
                r[i] = (byte)(sum % 10);
                carry = (byte)(sum / 10);
            }
            if (carry == 0)
            {
                return r;
            }
            else
            {
                r.CopyTo(rwc, 1);
                rwc[0] = (byte)carry;
                r = rwc;
            }
            return r;
        }

        //Function Sub
        static byte[] Sub(byte[] R, byte[] L)
        {
            //Declare attributes
            int len = 0;
            int sub = 0;
            int pseudonum = 0;
            int minuendDigit = 0;
            int subtrahendDigit = 0;
            len = Math.Max(R.Length, L.Length);
            byte[] r = new byte[len];
            //Fun Body
            for (int i = 0; i < len; i++)
            {
                minuendDigit = i < R.Length ? R[R.Length - 1 - i] : 0;
                subtrahendDigit = i < L.Length ? L[L.Length - 1 - i] : 0;

                sub = minuendDigit - subtrahendDigit - pseudonum;
                pseudonum = sub < 0 ? 1 : 0;

                r[len - 1 - i] = (byte)((sub + 10 * pseudonum) % 10);
            }
            return r;
        }

        //Function Shift Right
        static byte[] rlzeros(byte[] R, int n)
        {
            byte[] r = new byte[R.Length + n];
            Array.Copy(R, r, R.Length);
            for (int i = R.Length; i < r.Length; i++)
            {
                r[i] = 0;
            }
            return r;
        }

        //Function Shift Left
        static byte[] llzeros(byte[] R, int n)
        {
            byte[] r = new byte[R.Length + n];
            R.CopyTo(r, n);
            return r;
        }
        #endregion
    }
}
