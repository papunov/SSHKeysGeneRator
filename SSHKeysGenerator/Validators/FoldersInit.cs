using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSHKeysGenerator.Validators
{
    public static class FoldersInit
    {
        public static string ExportFolder = Directory.GetCurrentDirectory() + "\\Exports\\";

        public static void CheckingExportsFolder()
        {
            if (!Directory.Exists(ExportFolder))
            {
                CreateFolder(ExportFolder);
            }
        }

        private static void CreateFolder(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        public static bool CheckIfFileExist(string file)
        {
            if(File.Exists(file))
            {
                return true;
            }

            return false;
        }
    }
}
