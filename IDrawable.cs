using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public interface IDrawable
    {
        public void Draw(SpriteBatch spriteBatch, Vector2 position);
    }
}
