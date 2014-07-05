using System.Net;
using GemsWeb.Annotations;

namespace GemsWeb.Server
{
    public interface iResponseHandler
    {
        void Handle([NotNull] HttpListenerResponse pResponse);
    }
}