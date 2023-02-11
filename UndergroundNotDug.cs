using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = Microsoft.Xna.Framework.Color;

namespace Mole_on_Parole
{
    public class UndergroundNotDug : GridItem
    {
        public UndergroundNotDug(Vector2 position, Color color, Texture2D texture)
        {
            _position = position;
            _color = color;
            _texture = texture;
        }
    }
}
