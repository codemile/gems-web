using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GemsWeb.Annotations;

namespace GemsWeb.Politeness
{
    /// <summary>
    /// Counts how many times URL requests are made.
    /// </summary>
    public class PolitenessCounter : iPoliteness
    {
        /// <summary>
        /// How many times wait was called.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// THe child will handle politeness
        /// </summary>
        private readonly iPoliteness _child;

        /// <summary>
        /// Constructor
        /// </summary>
        public PolitenessCounter([NotNull] iPoliteness pChild)
        {
            if (pChild == null)
            {
                throw new ArgumentNullException("pChild");
            }

            _child = pChild;
            Count = 0;
        }

        /// <summary>
        /// Will pass the wait task to the child.
        /// </summary>
        public void Wait(string pURL)
        {
            lock (_child)
            {
                Count++;
                _child.Wait(pURL);                
            }
        }

        /// <summary>
        /// Clears the counter.
        /// </summary>
        public void Reset()
        {
            lock (_child)
            {
                Count = 0;
            }
        }
    }
}
