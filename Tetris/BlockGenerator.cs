using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BlockGenerator
    {
        private List<int[,]> patterns;
        private string file;
        private Random randGen;

        public BlockGenerator(string file)
        {
            patterns = new List<int[,]>();
            randGen = new Random();
            this.file = file;
        }
        public void LoadBlocks()
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string line;
                int[,] pattern = new int[4, 4];
                int j = 0;
                int lineNumber = 1;

                while ((line = sr.ReadLine()) != null)
                {
                    if((lineNumber % 5) != 0)
                    {
                        for (int i = 0; i < 4; i++)
                            pattern[i, j] = int.Parse(line[i].ToString());
                        j++;

                            if(j > 3)
                            {
                                patterns.Add(pattern);
                                j = 0;
                                pattern = new int[4, 4];
                            }
                        }
                    lineNumber++;

                }
            }
        }
        public Block Generate(int count)
        {
            int index = randGen.Next(0, count);
            return new Block(patterns[index]);
        }
    }
}
