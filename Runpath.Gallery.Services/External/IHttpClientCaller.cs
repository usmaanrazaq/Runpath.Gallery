using System.Collections.Generic;
using System.Threading.Tasks;

namespace Runpath.Gallery.Service.External
{
    public interface IHttpClientCaller<T>
    {

        Task<ICollection<T>> GetApiCollection(string url);
        Task<T> GetApi(string url);

    }
}
