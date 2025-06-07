using System;

namespace RestAPI_ProcessValidated_PartnerInfo.Helper
{
    public static class CalculationHelper
    {
        public static bool CheckPrimeNumber(long number)
        {
            if (number < 2) {  return false; }
            if (number == 2) { return true; }
            if (number % 2 == 0) { return false; }

            var squareRoot = Math.Sqrt(number);

            for (long i = 3; i <= squareRoot; i+=2)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
