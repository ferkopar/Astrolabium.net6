using Astrolabium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrolabium.Services
{
    public interface INativeService
    {
        void WriteNative(Native native);
        Native ReadNative(Guid nativeGuid);
        List<(Native, string)> GetNativeTupleList(string nativeDirectory = null);
    }
}
