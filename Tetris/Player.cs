using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Player
    {
        public long score;
        public int rows;
        public int level;
        public string nickname;
        public Player()
        {
            score = 0;
            rows = 0;
            level = 1;
        }
    }
}
