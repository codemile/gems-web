using System.Net;
using GemsWeb.Annotations;

namespace GemsWeb.Server
{
    public interface iResponseProvider
    {
        void Handle([NotNull] HttpListenerResponse pResponse);
    }
}