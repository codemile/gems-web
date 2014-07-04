using System.IO;
using System.Net.Mime;
using GemsWeb.Annotations;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    public interface iStreamReader
    {
        [NotNull]
        iResponse Read([NotNull] ContentType pContentType, [NotNull] Stream pStream);
    }
}