using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Configuration
{
    public interface IGlobalSettingRepository
    {
        IDictionary<string, object> Get();
    }
}
