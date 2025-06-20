using RestAPI_ProcessValidated_PartnerInfo.Utils;

namespace RestAPI_ProcessValidated_PartnerInfo.Helper
{
    public static class GenericHelper
    {
        public static bool CheckLastDigitMatch<T>(this T value, string amountDigit)
        {
            if (value == null) { 
                return false;
            }

            var convertedValue = value.ToString();
            var digitIndex = convertedValue.IndexOf($"{amountDigit}");

            if (digitIndex < 0)
            {
                return false;
            }

            convertedValue = convertedValue.Substring(digitIndex).Replace("0","");

            return convertedValue.Equals(amountDigit);
        }

        public static DateTime ConvertStringToDateTime(this string date)
        {
            string format = $"yyyy-MM-ddTHH:mm:ss.fffffffZ";

            var culture = System.Globalization.CultureInfo.InvariantCulture;
            var style = System.Globalization.DateTimeStyles.None;
            var convertSuccess = DateTime.TryParseExact(date, format, culture, style, out DateTime converted);

            if (!convertSuccess)
            {
                throw new HttpStatusCodeException(500, $"Date conversion failed - {date}");
            }

            return converted;
        }
    }
}
