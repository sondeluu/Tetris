using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tetris
{
    public class Level : Microsoft.Xna.Framework.DrawableGameComponent
    {

        public enum eGameState
        {
            Playing,
            Paused,
        }

        public eGameState gameState;

        public Tetris game;
        private Texture2D background, sPaused;

        private SoundEffect rowSound;

        private SpriteFont fontSm;

        private Block block;
        private Block nextBlock;
        private Vector2 gameBoardPos;
        private Texture2D[] tileSprite;
        private BlockGenerator blockGenerator;
        public GameBoard board;

        public float speed;
        private float timeSinceLastFall;

        private float timeSinceLastPress;
        private float delay;

        private float powerWait;

        public Level(Tetris game) : base(game)
        {
            this.game = game;
        }
  
        public override void Initialize()
        {
            powerWait = 0;
            gameState = eGameState.Playing;
            speed = 0.6f;
            timeSinceLastFall = 0;
            timeSinceLastPress = 0;
            delay = 0.06f;

            blockGenerator = new BlockGenerator("netfx.dll");
            blockGenerator.LoadBlocks();
            gameBoardPos = new Vector2(362, 37);
            board = new GameBoard(11, 20, gameBoardPos);


            nextBlock = blockGenerator.Generate(7);
            NextBlock();

            if (board.Collision(block, block.pos))
                game.Exit();

            game.player = new Player();

            base.Initialize();
        }
        protected override void LoadContent()
        {
            background = game.Content.Load<Texture2D>(@"Sprites\background");
            tileSprite = new Texture2D[8];
            sPaused = game.Content.Load<Texture2D>(@"Sprites\spr_pause");
            fontSm = game.Content.Load<SpriteFont>("Fonts/font_courier_new_small");
            for (int i = 0; i < 8; i++)
            {
                tileSprite[i] = game.Content.Load<Texture2D>(@"Sprites\Tiles\" + (i + 1).ToString());
            }
            rowSound = game.Content.Load<SoundEffect>(@"Sounds\sound_row");
            base.LoadContent(); 
        }
        public override void Update(GameTime gameTime)
        {
            if(gameState == eGameState.Playing)
            {
                if (game.NewKey(Keys.Enter) || game.NewKey(Keys.Up))
                {
                    block.Rotate();

                    if (board.Collision(block, block.pos))
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            block.Rotate();
                        }
                    }
                }
            }

            


            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float timeBetweenFalls = speed;
            timeSinceLastFall += seconds;

            if(timeSinceLastFall >= timeBetweenFalls)
            {

                block.Fall();

                if (board.Collision(block, block.pos))
                {
                    block.pos.Y--;
                    board.Merge(block);
                    int rows = board.RemoveRows();
                    if (rows > 0)
                        rowSound.Play();
                    game.player.rows += rows;

                    switch (rows)
                    {
                        case 1:
                            game.player.score += game.player.level * 40 + 40;
                            break;
                        case 2:
                            game.player.score += game.player.level * 100 + 100;
                            break;
                        case 3:
                            game.player.score += game.player.level * 300 + 300;
                            break;
                        case 4:
                            game.player.score += game.player.level * 1200 + 1200;
                            break;
                    }
                    int level = game.player.rows / 10 + 1;
                    
                    if(game.player.level != level)
                    {
                        game.player.level = level;
                        speed = speed * 5 / 6;
                    }
                    NextBlock();
                }
                timeSinceLastFall = 0;
                if (game.keyboardState.IsKeyDown(Keys.P))
                {
                    MediaPlayer.Pause();
                    gameState = eGameState.Paused;
                }
                if(gameState == eGameState.Paused)
                {
                    if (game.keyboardState.IsKeyDown(Keys.Y))
                    {
                        Thread.Sleep(3000);
                        game.Exit();
                    }
                    if (game.keyboardState.IsKeyDown(Keys.N))
                    {
                        gameState = eGameState.Playing;
                        MediaPlayer.Resume();
                    }
                }
            }

            timeSinceLastPress += seconds;

            if(timeSinceLastPress > delay)
            {
                if ((game.keyboardState.IsKeyDown(Keys.Left)) && (!board.Collision(block, new Vector2(block.pos.X - 1, block.pos.Y))))
                    block.pos.X--;
                if ((game.keyboardState.IsKeyDown(Keys.Right)) && (!board.Collision(block, new Vector2(block.pos.X + 1, block.pos.Y))))
                    block.pos.X++;
                timeSinceLastPress = 0;
            }
            if (game.NewKey(Keys.Down))
            {
                while (!board.Collision(block, new Vector2(block.pos.X, block.pos.Y + 1)))
                    block.Fall();
                timeSinceLastFall += timeBetweenFalls;
            }


            base.Update(gameTime);
        }
        public void NextBlock()
        {
            block = nextBlock;
            nextBlock = blockGenerator.Generate(7);
            block.pos = board.InsertBlock(block);

        }
        public override void Draw(GameTime gameTime)
        {
            game.spriteBatch.Begin();



            game.spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            nextBlock.Draw(new Vector2(900, 45), game.spriteBatch, tileSprite);
            game.spriteBatch.TextWithShadow(fontSm, game.player.score.ToString(), new Vector2(940, 530), Color.Yellow);
            game.spriteBatch.TextWithShadow(fontSm, game.player.level.ToString(), new Vector2(940, 225), Color.Yellow);



            block.Draw(gameBoardPos, game.spriteBatch, tileSprite);
            board.Draw(game.spriteBatch, tileSprite);
            if (gameState == eGameState.Paused)
            {
                game.spriteBatch.Draw(sPaused, new Vector2(0, 0), Color.White);
            }

            game.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
