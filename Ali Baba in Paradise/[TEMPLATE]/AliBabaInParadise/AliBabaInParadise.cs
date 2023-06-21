using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class AliBabaInParadise
    {
        #region YOUR CODE IS HERE
        #region FUNCTION#1: Calculate the Value
        //Your Code is Here:
        //==================
        /// <summary>
        /// Given the Camels maximum load and N items, each with its weight and profit 
        /// Calculate the max total profit that can be carried within the given camels' load
        /// </summary>
        /// <param name="camelsLoad">max load that can be carried by camels</param>
        /// <param name="itemsCount">number of items</param>
        /// <param name="weights">weight of each item</param>
        /// <param name="profits">profit of each item</param>
        /// <returns>Max total profit</returns>
        static public int SolveValue(int camelsLoad, int itemsCount, int[] weights, int[] profits)
        {
            //attributes
            int W = camelsLoad;
            int N = itemsCount;
            int[] w = weights;
            int[] s = profits;
            // Initialize 2D array
            int[,] result = new int[N + 1, W + 1];
            // Bottom-up Sol
            for (int i = 0; i <= N; i++) // LOOP at N
            {
                for (int j = 0; j <= W; j++) // LOOP at W
                {
                    if (i == 0 || j == 0) // Base case
                    {
                        result[i, j] = 0;
                    }
                    else if (w[i - 1] > j) // If w > W
                    {
                        result[i, j] = result[i - 1, j];
                    }
                    else
                    {
                        // Choose max (including item || excluding item)
                        result[i, j] = Math.Max(result[i, j - w[i - 1]] + s[i - 1], result[i - 1, j]);
                    }
                }
            }
            return result[N, W];
        }
        #endregion

        #region FUNCTION#2: Construct the Solution
        //Your Code is Here:
        //==================
        /// <returns>Tuple array of the selected items to get MAX profit (stored in Tuple.Item1) together with the number of instances taken from each item (stored in Tuple.Item2)
        /// OR NULL if no items can be selected</returns>
        static public Tuple<int, int>[] ConstructSolution(int camelsLoad, int itemsCount, int[] weights, int[] profits)
        {
            //attributes
            int W = camelsLoad;
            int N = itemsCount;
            int[] w = weights;
            int[] s = profits;
            // Initialize 2D array
            int[,] result = new int[N + 1, W + 1];
            // Bottom-up Sol
            for (int i = 0; i <= N; i++) // LOOP at N
            {
                for (int j = 0; j <= W; j++) // LOOP at W
                {
                    if (i == 0 || j == 0) // Base case
                    {
                        result[i, j] = 0;
                    }
                    else if (w[i - 1] > j) // If w > W
                    {
                        result[i, j] = result[i - 1, j];
                    }
                    else
                    {
                        // Choose max (including item || excluding item)
                        result[i, j] = Math.Max(result[i, j - w[i - 1]] + s[i - 1], result[i - 1, j]);
                    }
                }
            }
            // Backtrack

            // If no items selected
            if (result[N, W] == 0)
            {
                return null;
            }

            // If items selected
            List<Tuple<int, int>> LoadItems = new List<Tuple<int, int>>();
            for (int i = N; i > 0 && W > 0; i--)
            {
                // If profit of item(N) = profit of item(N-1)
                if (result[i, W] != result[i - 1, W])
                {
                    // Calculate the instances of item
                    int ins = (W / w[i - 1]);
                    LoadItems.Add(new Tuple<int, int>(i, ins));

                    // Update load
                    W -= (ins * w[i - 1]);
                }
            }
            return LoadItems.ToArray();
        }
        #endregion
        #endregion
    }
}
