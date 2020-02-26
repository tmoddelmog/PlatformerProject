//#define VISUAL_DEBUG

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        const int SCREEN_WIDTH = 1000, SCREEN_HEIGHT = 750;
        const int NUMBER_OF_PLATFORMS = 200;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        Player player;
        Platform[] platforms;
        AxisList axisList;
        SpriteFont font;
        bool playerMadeIt;
        const String madeIt = "You Made It";
        GoalBox box;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if VISUAL_DEBUG
            VisualDebugging.LoadContent(Content);
#endif
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            var texture = Content.Load<Texture2D>("CavemanSpriteSheet");
            sheet = new SpriteSheet(texture, 25, 32);
            var playerFrames = from index in Enumerable.Range(0, 16) select sheet[index];
            player = new Player(this, playerFrames);

            platforms = generatePlatforms(NUMBER_OF_PLATFORMS);

            axisList = new AxisList();
            foreach (Platform platform in platforms)
            {
                platform.LoadContent(Content);
                axisList.AddGameObject(platform);
            }

            playerMadeIt = false;
            font = Content.Load<SpriteFont>("font");

            box = new GoalBox(SCREEN_WIDTH, SCREEN_HEIGHT);
            box.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            player.Update(gameTime);


            //var platformsInRange = axisList.QueryRange(
            //    player.Bounds.X, player.Bounds.X + player.Bounds.Width
            //    );

            // include all platforms or else only the first works
            //var platformsInRange = axisList.QueryRange(0, 500);

            player.CheckForPlatformCollision(platforms);

            if (player.Bounds.CollidesWith(box.Bounds))
            {
                playerMadeIt = true;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkGreen);

            // TODO: Add your drawing code here
            var offset = new Vector2(500, 375) - player.Position;
            var translation = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, translation);

            player.Draw(spriteBatch);
            foreach (Platform platform in platforms) platform.Draw(spriteBatch);

            if (playerMadeIt)
            {
                spriteBatch.DrawString(font, madeIt, new Vector2(player.Position.X, 
                    player.Position.Y), 
                    Color.Gold);
            }
            else
            {
                box.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        static Platform[] generatePlatforms(int numPlatforms)
        {
            Platform[] platforms = new Platform[numPlatforms];
            Random r = new Random();

            for (var i = 0; i < numPlatforms; i++)
            {
                platforms[i] = new Platform(new Vector2(r.Next(0, SCREEN_WIDTH * 4), // X
                    r.Next(0, SCREEN_HEIGHT * 4)), // Y
                    r.Next(2, 20)); // tile count
            }

            return platforms;
        }
    }
}
