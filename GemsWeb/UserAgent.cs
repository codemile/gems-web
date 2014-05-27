namespace GemsWeb
{
    public static class UserAgent
    {
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