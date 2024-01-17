using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_task
{
    internal class TextFileClass
    {
        public TextFileClass(string _filePath, int numberOfBlocks)
        {
            hashCodes = new List<HashCode>(numberOfBlocks);
            filePath = _filePath;
        }
        private List <HashCode> hashCodes;
        private readonly string filePath;

        public string FilePath
        {
            get { return filePath; }
            init { }
        }

        //It should compare if two files have the same content
        public void IsEqualTo(TextFileClass file)
        {
            throw new NotImplementedException();
        }
    }
}
