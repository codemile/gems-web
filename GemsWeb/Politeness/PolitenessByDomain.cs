using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GemsLogger;

namespace GemsWeb.Politeness
{
    /// <summary>
    /// Handles politeness when hitting a web server repeatedly with
    /// requests. This is done by causing a delay between requests
    /// to the same domain.
    /// </summary>
    public class PolitenessByDomain : iPoliteness
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (PolitenessByDomain));

        /// <summary>
        /// Used to control thread access
        /// </summary>
        private readonly object _sync;

        /// <summary>
        /// Min milliseconds between requests of the same domain.
        /// </summary>
        private readonly int _waitMilliseconds;

        /// <summary>
        /// The last time a domain was hit
        /// </summary>
        private Dictionary<string, DateTime> _domainActivity;

        /// <summary>
        /// Size of history
        /// </summary>
        private int _count
        {
            get
            {
                lock (_sync)
                {
                    return _domainActivity.Count;
                }
            }
        }

        /// <summary>
        /// The time difference between a DateTime and Now in milliseconds.
        /// </summary>
        private static int TimeDifference(DateTime pWhen)
        {
            TimeSpan delta = DateTime.Now.Subtract(pWhen);
            return (int)Math.Abs(delta.TotalMilliseconds);
        }

        /// <summary>
        /// The current thread must wait at least X milliseconds.
        /// </summary>
        /// <param name="pMilliseconds">How long it has already waited.</param>
        /// <param name="pAtLeast">How long it has to wait.</param>
        private static void WaitAtLeast(int pMilliseconds, int pAtLeast)
        {
            if (pMilliseconds < pAtLeast)
            {
                Thread.Sleep(pAtLeast - pMilliseconds);
            }
        }

        /// <summary>
        /// Returns the time milliseconds since this host
        /// was last hit.
        /// </summary>
        private int Milliseconds(string pHost)
        {
            lock (_sync)
            {
                if (!_domainActivity.ContainsKey(pHost))
                {
                    _domainActivity.Add(pHost, DateTime.Now);
                }
                return TimeDifference(_domainActivity[pHost]);
            }
        }

        /// <summary>
        /// Purges domain entries that have expired.
        /// </summary>
        private void Purge()
        {
            lock (_sync)
            {
                _domainActivity = (from pair in _domainActivity
                                   where TimeDifference(pair.Value) <= _waitMilliseconds
                                   select pair).ToDictionary(pPair=>pPair.Key, pPair=>pPair.Value);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pWaitMilliseconds">Min milliseconds between requests of the same domain.</param>
        public PolitenessByDomain(int pWaitMilliseconds)
        {
            _waitMilliseconds = pWaitMilliseconds;
            _sync = new object();
            _domainActivity = new Dictionary<string, DateTime>();
        }

        /// <summary>
        /// Will block the thread if the URL refers to a domain
        /// that was used to recently.
        /// </summary>
        /// <param name="pURL">The URL to check.</param>
        public void Wait(string pURL)
        {
            _logger.Finer("{0}: {1}", _count, pURL);

            Uri uri;
            if (!Uri.TryCreate(pURL, UriKind.Absolute, out uri))
            {
                return;
            }

            string host = uri.Host.ToLower();

            int milliseconds = Milliseconds(host);
            WaitAtLeast(milliseconds, 2000);

            lock (_sync)
            {
                _domainActivity[host] = DateTime.Now;
            }

            Purge();
        }
    }
}