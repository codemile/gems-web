using System;
using System.Net;
using System.Threading;
using Logging;

namespace GemsWeb.Server
{
    /// <summary>
    /// A very simple HTTP server that can be used for testing.
    /// </summary>
    public class WebServer : IDisposable
    {
        /// <summary>
        /// Logging
        /// </summary>
        private static readonly Logger _logger = Logger.Create(typeof (WebServer));

        /// <summary>
        /// The default response for unhandled requests.
        /// </summary>
        private readonly iResponseHandler _default;

        /// <summary>
        /// The http listener
        /// </summary>
        private readonly HttpListener _server;

        /// <summary>
        /// Listens and sends responses.
        /// </summary>
        private void Listen(Object pState)
        {
            _logger.Finest("Started");

            try
            {
                while (_server.IsListening)
                {
                    HttpListenerContext context = _server.GetContext();

                    _logger.Finer("{0}: {1}", context.Request.UserHostAddress, context.Request.Url);

                    try
                    {
                        _default.Handle(context.Response);
                    }
                    catch (Exception ex)
                    {
                        _logger.Exception(ex);
                    }
                    finally
                    {
                        context.Response.OutputStream.Close();
                    }
                }
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode != 995)
                {
                    _logger.Exception(ex);
                }
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
            }

            _logger.Finest("Finished");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pPort">The port number to listen for connections.</param>
        /// <param name="pDefault">The default response for unhandled request, when Null a 404 response will be used.</param>
        public WebServer(int pPort, iResponseHandler pDefault = null)
        {
            if (!HttpListener.IsSupported)
            {
                throw new NotSupportedException();
            }

            _default = pDefault ?? new FileNotFoundHandler();

            _server = new HttpListener();
            _server.Prefixes.Add(string.Format("http://*:{0}/", pPort));
            _server.Start();

            ThreadPool.QueueUserWorkItem(Listen);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _logger.Finest("Stopping");

            _server.Stop();
            _server.Close();
        }
    }
}