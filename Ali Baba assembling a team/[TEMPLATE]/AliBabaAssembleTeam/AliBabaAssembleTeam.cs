using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem
{
    // *****************************************
    // DON'T CHANGE CLASS OR FUNCTION NAME
    // YOU CAN ADD FUNCTIONS IF YOU NEED TO
    // *****************************************
    public static class AliBabaAssembleTeam
    {
        #region YOUR CODE IS HERE
        //Your Code is Here:
        //==================
        /// <summary>
        /// Find the minimum cost for any team that can be assembled 
        /// </summary>
        /// <param name="N">size of the array</param>
        /// <param name="array">contains the cost of each theif (+ve, -ve or 0) </param>
        /// <returns>min total cost of a team</returns>
        static public long AssembleTeam(int N, short[] array)
        {
            int minSum = 0;
            int currentSum = 0;
            int positivenumcount = 0;

            foreach (int num in array)
            {
                currentSum += num;
                if (currentSum > 0)
                {
                    currentSum = 0;
                }
                if (num > 0) 
                {
                    positivenumcount++;
                }
                minSum = Math.Min(minSum, currentSum);
            }
            if (positivenumcount == N)
            {
                minSum = array.Min();
            }
            return minSum;
        }
        #endregion
    }
}
