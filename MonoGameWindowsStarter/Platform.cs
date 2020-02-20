//#define VISUAL_DEBUG

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter
{
    class Platform : IBoundable
    {
        Vector2 position;
        Texture2D texture;
        public int TileSize = 32;
        int tileCount;
        BoundingRectangle bounds;
        public BoundingRectangle Bounds => bounds;

        public Platform(Vector2 position, int tileCount)
        {
            this.position = position;
            this.bounds = new BoundingRectangle(position.X,
                position.Y, 
                TileSize * tileCount, 
                TileSize);
            this.tileCount = tileCount;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            this.texture = content.Load<Texture2D>("GrassTile");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
#if VISUAL_DEBUG
            VisualDebugging.DrawRectangle(spriteBatch, Bounds, Color.Orange);
#endif
            for (var i = 0; i < tileCount; i++)
            {
                var pos = new Vector2(this.position.X + (i * TileSize), this.position.Y);
                spriteBatch.Draw(texture, pos, Color.White);
            }
        }
    }
}
