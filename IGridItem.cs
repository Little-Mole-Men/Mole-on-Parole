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
        public bool Dug = false;

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground, Vector2 center)
        {
            Vector2 pos = Vector2.Add(Vector2.Negate(position), _position);
            pos += center;
            spriteBatch.Draw(_texture, pos, (underground ? 0.3f : 1) * _color);
        }

        public void Dig()
        {
            Dug = true;
        }
    }
}
