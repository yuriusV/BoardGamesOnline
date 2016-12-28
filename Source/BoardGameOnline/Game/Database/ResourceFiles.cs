using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Database
{
    public class ResourceFiles
    {
        private static string _base = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

        public static string SaveFile( string filePath ) {
            Directory.CreateDirectory("Resources");
            var path = Path.Combine(_base, Path.GetFileName(filePath));
            File.Copy(filePath, path, overwrite: true);
            return path;
        }

        public static string GetFile( string name ) {
            var realName = Path.GetFileName(name);
            return Path.Combine(_base, name);
        }
    }
}
