using System.Diagnostics;

namespace dotNet_BackEnd
{
    public class FileCompiler
    {
        public FileCompiler(string cppSourceName)
        {
            cppSourceName = cppSourceName.Remove(cppSourceName.Length - 4); // Se scoate .cpp din nume
            runCpp(cppSourceName);
        }

        public void runCpp(string cppSourceName)
        {
            Process process = new Process();
            var processInfo = new ProcessStartInfo();
            processInfo.WorkingDirectory = @"C:\Windows\System32";
            processInfo.FileName = @"C:\Windows\System32\cmd.exe";
            processInfo.Verb = "runas";
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardInput = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;
            processInfo.WindowStyle = ProcessWindowStyle.Maximized;
            process.StartInfo = processInfo;
            process.Start();
            process.OutputDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (sender, e) => { Console.WriteLine(e.Data); };
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            using (StreamWriter sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("C:\\Windows\\SysWOW64\\WindowsPowerShell\\v1.0\\powershell.exe -noe");    // Mutare de la cmd la powershell
                    sw.WriteLine("&{Import-Module 'D:\\Visual Studio 2022\\Common7\\Tools\\Microsoft.VisualStudio.DevShell.dll'; Enter-VsDevShell 3fe84f48}");

                    sw.WriteLine("D:");
                    sw.WriteLine("cd 'D:\\Proiecte C++\\dotNet_FrontEnd\\dotNet_BackEnd\\Resources\\Files'");

                    sw.WriteLine("cl /EHsc " + cppSourceName + ".cpp");         // Compilare
                    sw.WriteLine(".\\" + cppSourceName + ".exe > output.txt");  // Creare output file
                }
                process.WaitForExit();
            }
        }

        public static bool compareOutput(string file1, string file2)   //compar outputFileName cu outputTemplate
        {
            string directoryPath = "D:\\Proiecte C++\\dotNet_FrontEnd\\dotNet_BackEnd\\Resources\\Files";
            string file1Path = Path.Combine(directoryPath, file1);
            string file2Path = Path.Combine(directoryPath, file2);

            int file1byte;
            int file2byte;
            if (file1 == file2)
                return true;

            FileStream fs1 = new FileStream(file1Path, FileMode.Open);
            FileStream fs2 = new FileStream(file2Path, FileMode.Open);

            if (fs1.Length != fs2.Length)
            {
                fs1.Close();
                fs2.Close();
                return false;
            }

            do
            {
                file1byte = fs1.ReadByte();
                file2byte = fs2.ReadByte();
            }
            while ((file1byte == file2byte) && (file1byte != -1) && (file2byte != -1));

            fs1.Close();
            fs2.Close();

            return ((file1byte - file2byte) == 0);
        }
    }
}
