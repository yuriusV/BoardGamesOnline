using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Game.Modules
{
    public static class Loader
    {
        public static List<Assembly> LoadedAssemblies { get; set; }

        public static IEnumerable<IExtension> LoadExtensions( ) {
            yield break;
        }
    }
}
