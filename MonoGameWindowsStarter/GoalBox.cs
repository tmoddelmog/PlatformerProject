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
    public class GoalBox
    {
        const int BOX_SIZE = 50;

        public int X, Y;
        public BoundingRectangle Bounds;
        Random random;
        Texture2D texture;

        public GoalBox(int screenWidth, int screenHeight)
        {
            random = new Random();
            this.X = random.Next(0, screenWidth + 100);
            this.Y = random.Next(0, screenHeight + 100);
            this.Bounds = new BoundingRectangle(this.X, this.Y, BOX_SIZE, BOX_SIZE);
        }

        public void LoadContent(ContentManager Content)
        {
            this.texture = Content.Load<Texture2D>("pixel");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle(X, Y, BOX_SIZE, BOX_SIZE), Color.Gold);
        }
    }
}
