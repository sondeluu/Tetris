using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tetris
{
    public class Block
    {
        public int[,] Tiles { get; set; }
        public Vector2 pos;
        private Random rand;
        private int colourSelect;
        private int[,] CopyTiles(int[,] tiles)
        {
            int[,] newTiles = new int[4, 4];
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    newTiles[j, i] = tiles[j, i];
            return newTiles;
        }

        public Block(int[,] tiles)
        {
            Tiles = CopyTiles(tiles);
            rand = new Random();
            Generate();
            pos = new Vector2(0, 0);
        }
        private void Generate()
        {
            colourSelect = rand.Next(1, 9);
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    if (Tiles[i, j] > 0)
                        Tiles[i, j] = colourSelect;
        }
        public void Fall()
        {
            pos.Y++;
        }
        public void Rotate()
        {
            int[,] a = CopyTiles(Tiles);

            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    Tiles[x, y] = a[y, 3 - x];
                }
            }
        }
        public void Draw(Vector2 border, TextEffects spriteBatch, Texture2D[] sprite)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (Tiles[i, j] > 0)
                        spriteBatch.Draw(sprite[Tiles[i,j] - 1],
                            new Vector2(border.X + (i + pos.X) * sprite[0].Width, 
                                        border.Y + (j + pos.Y) * sprite[0].Height),
                                        Color.White);

                    
                }
            }
        }
    }
}
