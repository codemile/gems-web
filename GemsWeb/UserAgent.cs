namespace GemsWeb
{
    public static class UserAgent
    {
        public const string MOZILLA = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";

        /// <summary>
        /// Formulate a user agent to identify the robot.
        /// </summary>
        /// <param name="pAppName">The name of the application.</param>
        /// <param name="pDomain">The website domain</param>
        /// <param name="pEmail">The support email address</param>
        /// <returns>The user agent string.</returns>
        public static string Format(string pAppName, string pDomain, string pEmail)
        {
            return string.Format("{0}/1.0 (+http://{1}; email:{2}; Mozilla/5.0; please report problems)",
                pAppName,
                pDomain,
                pEmail);
        }
    }
}