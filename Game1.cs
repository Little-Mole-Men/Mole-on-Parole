using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Mole_on_Parole
{
    public class Game1 : Game
    {
        Mole mole;
        Map map;
        Man man;
        
        List<IFood> earthworms = new List<IFood>();
        List<IValuable> collectibles = new List<IValuable>();
        Grid grid;
        Texture2D moleTexture;
        Texture2D manTexture;
        Texture2D wormTexture;
        Texture2D valuableTexture;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Vector2 center;

        SpriteFont spriteFont;

        private int numWorms = 1000;
        private int numValuables = 20;

        private bool qDown = false;
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

            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            center = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            mole = new Mole(moleTexture);
            mole.SetPosition(center);
            Texture2D grass = Content.Load<Texture2D>("grass");
            map = new Map(1000, 1000, 1, Content.Load<Texture2D>("grass"), Content.Load<Texture2D>("grass"), Content.Load<Texture2D>("grass"), Content.Load<Texture2D>("grass"));
            map.setViewRadius(20);
            man = new Man(manTexture, center);

            Random rd = new Random();

   

            for (var i=0; i<numWorms; i++)
            {
                Earthworm worm;
                worm = new Earthworm(wormTexture, new Vector2(rd.Next(0,32000), rd.Next(0, 32000)));
                earthworms.Add(worm);

            }

            for (var i = 0; i < numWorms; i++)
            {
                GenericValuable valuable;
                valuable = new GenericValuable(valuableTexture, new Vector2(rd.Next(0, 32000), rd.Next(0, 32000)), 2, 10);
                collectibles.Add(valuable);

            }
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
            if (kState.IsKeyDown(Keys.W) == kState.IsKeyDown(Keys.S))
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
            if(kState.IsKeyDown(Keys.A) == kState.IsKeyDown(Keys.D))
            {
                mole.Slow(Directions.RIGHT, (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            if(kState.IsKeyDown(Keys.Q))
            {
                qDown = true;
            }
            if(kState.IsKeyUp(Keys.Q) && qDown)
            {
                qDown = false;

                if(!mole.HasAttachedValuable() && map.IsClosestGrass(mole.GetPosition()))
                {
                    if (!map.IsClosestDug(mole.GetPosition()))
                    {
                        map.DigHole(mole.GetPosition());
                    }
                    mole.Underground = !mole.Underground;
                }
            }

            mole.Update(gameTime.ElapsedGameTime.TotalSeconds, mole.GetPosition());
            map.Update(gameTime.ElapsedGameTime.TotalSeconds, mole.GetPosition());
            if (!mole.Underground)
            {
                if(map.IsClosestDug(mole.GetPosition()) && mole.HasAttachedValuable())
                {
                    collectibles.Remove(mole.GetAttachedValuable());
                    mole.SetAttachedValuable(null);
                }
                man.DetectAndKillMole(gameTime.ElapsedGameTime.TotalSeconds, mole);
                man.Update(gameTime.ElapsedGameTime.TotalSeconds, mole.GetPosition());
                earthworms.RemoveAll(elem => elem.DetectMoleClose(mole) == true);
                foreach (var valuable in collectibles)
                {
                    valuable.DetectMoleClose(mole);
                }
                if(mole.HasAttachedValuable())
                {
                    mole.GetAttachedValuable().SetPosition(mole.GetPosition());
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(blendState: BlendState.AlphaBlend);

            map.Draw(_spriteBatch, mole.GetPosition(), mole.Underground, center, mole);
            foreach (var worm in earthworms)
            {
                worm.Draw(_spriteBatch, mole.GetPosition(), mole.Underground, center);
            }
            foreach (var valuable in collectibles)
            {
                valuable.Draw(_spriteBatch, mole.GetPosition(), mole.Underground, center);
            }
            man.Draw(_spriteBatch, mole.GetPosition(), mole.Underground, center);

            spriteFont = Content.Load<SpriteFont>("File");

            _spriteBatch.DrawString(spriteFont, "Score: " + mole.GetScore(), new Vector2(_graphics.PreferredBackBufferWidth / 2 - 50, 20), Color.Black);

            _spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}