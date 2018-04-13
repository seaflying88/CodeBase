using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Editor.Svn
{
    public class SvnUtil : UnityEditor.Editor
    {
        private static string _svnPath = "";

        public static void DelAndUpdateSvnFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            ExecuteProcess(GetSvnProcPath(), string.Format("/command:update /path:{0} /closeonend:2", path));
        }

        public static void RevertAndUpdateSvnDirectory(string path)
        {
            ExecuteProcess(GetSvnProcPath(), string.Format("/command:revert -r /path:{0} /closeonend:2", path));
            ExecuteProcess(GetSvnProcPath(), string.Format("/command:update /path:{0} /closeonend:2", path));
        }

        public static void UpdateSvnDirectory(string path)
        {
            ExecuteProcess(GetSvnProcPath(), string.Format("/command:update /path:{0} /closeonend:2", path));
        }

        public static void CommitSvnDirectory(string path)
        {
            ExecuteProcess(GetSvnProcPath(), string.Format("/command:commit /path:{0} /closeonend:0", path));
        }

        public static void AddSvnDirectory(string path)
        {
            ExecuteProcess(GetSvnProcPath(), string.Format("/command:add /path:{0} /closeonend:0", path));
        }

        public static void ProcessSvnCommand(string command)
        {
            ExecuteProcess(GetSvnProcPath(), command);
        }

        private static List<string> drives = new List<string>() { "c:", "d:", "e:", "f:" };

        private static string svnPath = @"\Program Files\TortoiseSVN\bin\";
        private static string svnProc = @"TortoiseProc.exe";
        private static string svnProcPath = "";

        private static string GetSvnProcPath()
        {
            if (_svnPath != string.Empty)
            {
                return _svnPath;
            }
            foreach (string item in drives)
            {
                string path = string.Concat(item, svnPath, svnProc);
                if (File.Exists(path))
                {
                    _svnPath = path;
                    break;
                }
            }
            if (_svnPath == string.Empty)
            {
                _svnPath = EditorUtility.OpenFilePanel("Select TortoiseProc.exe", "c:\\", "exe");
            }
            //可将路径存到本地注册表
            return _svnPath;
        }

        private static void ExecuteProcess(string filePath, string command, int seconds = 0)
        {
            EditorUtil.ExecuteProcess(filePath, command, "", seconds);//参见文末另一篇：C#调用可执行文件
        }
    }
}