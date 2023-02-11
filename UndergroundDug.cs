using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public class UndergroundDug : GridItem
    {
        public UndergroundDug(Vector2 position, Color color, Texture2D texture)
        {
            _position = position;
            _color = color;
            _texture = texture;
        }
    }
}
