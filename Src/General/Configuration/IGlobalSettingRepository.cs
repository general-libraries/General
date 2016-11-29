using System.Collections.Generic;
using System.Threading.Tasks;

namespace General.Configuration
{
    public interface IGlobalSettingRepository
    {
        IDictionary<string, object> Get();
    }
}
