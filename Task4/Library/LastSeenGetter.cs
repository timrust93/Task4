namespace Task4.Library
{
    public static class LastSeenGetter
    {
        public static string GetLastSeenTime(DateTime dateTime)
        {
            TimeSpan timespan = DateTime.Now - dateTime;
            if (timespan.TotalDays > 1)
            {
                return (int)timespan.TotalDays + " days ago";
            }
            if (timespan.TotalMinutes < 1)
            {
                return "just now";
            }
            if (timespan.TotalHours < 1)
            {
                return "~" + (int)MathF.Max(2, (float)timespan.TotalMinutes) + " minutes ago";
            }
            if (timespan.TotalHours > 1)
            {
                return "~" + timespan.TotalHours + "hours " + timespan.TotalMinutes + " minutes ago";
            }
            return "";
        }
    }
}
