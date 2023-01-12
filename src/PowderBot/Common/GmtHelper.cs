namespace Common
{
    public static class GmtHelper
    {
        public static int GetGmt(long userLocalTime, long timestamp)
        {
            return (int)Math.Round((double)(userLocalTime - timestamp) / 60 / 60);
        }
    }
}
