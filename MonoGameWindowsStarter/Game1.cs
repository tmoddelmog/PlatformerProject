#define VISUAL_DEBUG

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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;
        Player player;
        Platform[] platforms;
        Vector2[] platformPositions;
        AxisList axisList;
        SpriteFont font;
        bool playerMadeIt;
        const String madeIt = "You Made It";

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

            var viewportWidth = GraphicsDevice.Viewport.Width;
            var viewportHeight = GraphicsDevice.Viewport.Height;
            var tileSize = 32;

            platformPositions = new Vector2[]
            {
                // bottom to top on screen
                new Vector2(0, 400), // ground
                new Vector2(viewportWidth/2 - tileSize, 290), // first
                new Vector2(0, 215), // second
                new Vector2(viewportWidth - 15*tileSize, 120) // top
            };

            platforms = new Platform[]
            {
                new Platform(platformPositions[0], 15),
                new Platform(platformPositions[1], 10),
                new Platform(platformPositions[2], 8),
                new Platform(platformPositions[3], 15)
            };

            axisList = new AxisList();
            foreach (Platform platform in platforms)
            {
                platform.LoadContent(Content);
                axisList.AddGameObject(platform);
            }

            playerMadeIt = false;
            font = Content.Load<SpriteFont>("font");
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
            var platformsInRange = axisList.QueryRange(0, 500);
            player.CheckForPlatformCollision(platformsInRange);

            if (player.Position.X >= 814) playerMadeIt = true;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // TODO: Add your drawing code here
            player.Draw(spriteBatch);
            foreach (Platform platform in platforms) platform.Draw(spriteBatch);

            if (playerMadeIt)
            {
                spriteBatch.DrawString(font, madeIt, new Vector2(300, 200), Color.Gold);
                player.Position = new Vector2(0, 0);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
