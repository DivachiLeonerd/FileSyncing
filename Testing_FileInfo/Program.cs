namespace Testing_FileInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestFileRecursion("C:\\Users\\Afonso Fernandes\\OneDrive\\Documentos\\Programação\\C#\\exercicios\\Test_Task");
        }

        static void TestFileRecursion(string directory)
        {
            DirectoryInfo teste = new DirectoryInfo(directory);
            try
            {
                foreach (var files in teste.EnumerateFiles())
                {
                    Console.WriteLine(files.FullName);
                }
                foreach (var currentDirectory in teste.EnumerateDirectories())
                {
                    Console.WriteLine(currentDirectory.FullName);
                    TestFileRecursion(currentDirectory.FullName);
                }
            }
            catch (Exception ex) { Console.WriteLine("error:" + ex.Message); }
        }
    }
}
