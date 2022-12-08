using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace Tetris
{
    public class Tetris : Game
    {
        public Player player;
        
        private GraphicsDeviceManager graphics;
        public TextEffects spriteBatch;
        public Texture2D background;
        public SpriteFont fontCourierNew, fontCourierNewSmall, fontCourierNewTiny;

        private SoundEffect soundRow;
        private Song backgroundMusic;

        public KeyboardState keyboardState, prevKeyboardState;

        public int windowHeight = 720, windowWidth = 1280;
        public Tetris()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            
            
            Level level = new Level(this);
            Components.Add(level);

            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new TextEffects(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            background = Content.Load<Texture2D>(@"Sprites\background");
            fontCourierNew = Content.Load<SpriteFont>(@"Fonts\font_courier_new");
            fontCourierNewSmall = Content.Load<SpriteFont>(@"Fonts\font_courier_new_small");
            fontCourierNewTiny = Content.Load<SpriteFont>(@"Fonts\font_courier_new_tiny");

            soundRow = Content.Load<SoundEffect>(@"Sounds\sound_row");
            backgroundMusic = Content.Load<Song>(@"Sounds\tetris-theme");

            MediaPlayer.IsRepeating = true;
            ChangeMusic(backgroundMusic);

        }

        public void ChangeMusic(Song song)
        {
            if (MediaPlayer.Queue.ActiveSong != song)
                MediaPlayer.Play(song);
        }
        public bool NewKey(Keys key)
        {
            return keyboardState.IsKeyDown(key) && prevKeyboardState.IsKeyUp(key);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            prevKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter) & prevKeyboardState.IsKeyUp(Keys.Enter)) soundRow.Play();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}