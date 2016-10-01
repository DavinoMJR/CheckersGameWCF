using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DamasGame.Util
{
    public class DirectoryHelper
    {

        public static string GetRootPath()
        {
            var debugPath = string.Empty;

            #if (DEBUG)
            debugPath = "..\\..\\";
            #endif

            return Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), debugPath);
        }
    }
}
