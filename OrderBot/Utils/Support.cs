namespace OrderBot.Utils
{
    public static class Support
    {
        public static DateTime Timestamp2DateTime(long unixTimestampMilliseconds)
        {
            // Convert the Unix timestamp to DateTime using DateTimeOffset
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestampMilliseconds).DateTime;

            // Return the DateTime object
            return dateTime.ToLocalTime();
        }
    }
}