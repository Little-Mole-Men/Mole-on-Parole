using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;

namespace Mole_on_Parole
{
    public class Game1 : Game
    {
        Mole mole;
        Man man;
        Earthworm worm;
        List<Earthworm> earthworms = new List<Earthworm>();
        GenericValuable valuable;
        List<GenericValuable> collectibles = new List<GenericValuable>();
        Grid grid;
        Texture2D moleTexture;
        Texture2D manTexture;
        Texture2D wormTexture;
        Texture2D valuableTexture;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            moleTexture = Content.Load<Texture2D>("ball");
            manTexture = Content.Load<Texture2D>("ball");
            wormTexture = Content.Load<Texture2D>("ball");
            valuableTexture = Content.Load<Texture2D>("ball");


            mole = new Mole(moleTexture);
            man = new Man(manTexture, new Vector2(_graphics.PreferredBackBufferWidth / 2,
_graphics.PreferredBackBufferHeight / 2));
            
            worm = new Earthworm(wormTexture, new Vector2(100, 100));
            earthworms.Add(worm);

            valuable = new GenericValuable(valuableTexture, new Vector2(300, 300), 2, 10);
            collectibles.Add(valuable);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.W))
            {
                mole.Accelerate(Directions.UP, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (kState.IsKeyDown(Keys.S))
            {
                mole.Accelerate(Directions.DOWN, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (!kState.IsKeyDown(Keys.W) && !kState.IsKeyDown(Keys.S))
            {
                mole.Slow(Directions.UP, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (kState.IsKeyDown(Keys.A))
            {
                mole.Accelerate(Directions.LEFT, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (kState.IsKeyDown(Keys.D))
            {
                mole.Accelerate(Directions.RIGHT, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(!kState.IsKeyDown(Keys.A) && !kState.IsKeyDown(Keys.D))
            {
                mole.Slow(Directions.RIGHT, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            mole.Update(gameTime.ElapsedGameTime.TotalSeconds);
            man.UpdatePos(gameTime.ElapsedGameTime.TotalSeconds, mole.getPosition());
            man.DetectAndKillMole(gameTime.ElapsedGameTime.TotalSeconds, mole);
            earthworms.RemoveAll(elem => elem.DetectMoleClose(mole) == true);
            foreach (var valuable in collectibles)
            {
                valuable.DetectMoleClose(mole);
                if (valuable.getAttached())
                {
                    valuable.SetPosition(mole.getPosition());
                }
            }
                
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Olive);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            mole.Draw(_spriteBatch);
            man.Draw(_spriteBatch);
            foreach (var warm in earthworms)
            {
                worm.Draw(_spriteBatch);
            }
            foreach (var valuable in collectibles)
            {
                valuable.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}