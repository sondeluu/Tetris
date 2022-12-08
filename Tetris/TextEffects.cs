using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class TextEffects : SpriteBatch
    {
        public TextEffects(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {

        }
        public void TextWithShadow(SpriteFont spriteFont, string text, Vector2 pos, Color colour)
        {
            DrawString(spriteFont, text, new Vector2(pos.X + 2, pos.Y + 2), Color.Black * 0.8f);
            DrawString(spriteFont, text, pos, colour);
        }
    }
}
