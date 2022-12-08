using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameBoard
    {
        private int[,] tiles;
        private int height;
        private int width;
        private Vector2 pos;

        public GameBoard(int width, int height, Vector2 pos)
        {
            this.width = width;
            this.height = height;
            this.pos = pos;
            Clear();
        }

        public Vector2 InsertBlock(Block block)
        {
            return new Vector2(((int)width / 2) - 2, 0);
        }
        public void Merge(Block block)
        {
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                {
                    int x = Convert.ToInt32(i + block.pos.X);
                    int y = Convert.ToInt32(j + block.pos.Y);

                    if ((block.Tiles[i, j] > 0) && (y >= 0))
                        tiles[x, y] = block.Tiles[i, j];
                }
        }
        public bool Collision(Block block, Vector2 pos)
        {
            bool collision = false;
            for (int j = 0; j < 4; j++)
                for (int i = 0; i < 4; i++)
                    if ((block.Tiles[i,j] > 0) && 
                        ((j + pos.Y >= height) ||
                        (i + pos.X < 0)        || 
                        (i + pos.X >= width)   ||
                        (tiles[Convert.ToInt32(i + pos.X), 
                        Convert.ToInt32(j + pos.Y)] > 0)))
                        collision = true;
            return collision;

        }
        public int RemoveRows()
        {
            int count = 0;
            for (int row = 0; row < height; row++)
            {
                bool complete = true;
                for (int i = 0; i <= (width - 1); i++)
                {
                    if (tiles[i, row] == 0)
                        complete = false;
                }
                if (complete)
                {
                    for(int j = (row - 1); j > 0; j--)
                        for (int i = 0; i < width; i++)
                        {
                            tiles[i, j + 1] = tiles[i, j];
                        }
                    count++;
                }
            }
            return count;
        }

        public void Clear()
        {
            tiles = new int[width, height];
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D[] sprites)
        {
            for (int j = 0; j <= (height - 1); j++)
                for (int i = 0; i <= (width - 1); i++)
                    if (tiles[i, j] != 0)
                        spriteBatch.Draw(sprites[tiles[i, j] - 1], 
                            new Vector2(pos.X + i * sprites[0].Width,
                                        pos.Y + j * sprites[0].Height),
                                        Color.White);

        }
    }
}
