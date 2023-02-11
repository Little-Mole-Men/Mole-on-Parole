using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public abstract class GridItem : IDrawable
    {
        protected Texture2D _texture;
        protected Vector2 _position;
        protected Color _color;

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            Vector2 pos = Vector2.Add(Vector2.Negate(position), _position);
            pos += new Vector2(255, 255);
            spriteBatch.Draw(_texture, _position*16, _color);
        }
    }
}
