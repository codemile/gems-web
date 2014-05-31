using System.Net.Mime;

namespace GemsWeb.Readers
{
    public interface iStreamReaderFactory
    {
        iStreamReader Create(ContentType pContentType);
    }
}