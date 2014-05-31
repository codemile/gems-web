using System.IO;
using System.Net.Mime;
using GemsWeb.Responses;

namespace GemsWeb.Readers
{
    public interface iStreamReader
    {
        iResponse Read(ContentType pContentType, Stream pStream);
    }
}