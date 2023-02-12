using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public class Grass : GridItem, IDrawable
    {

        private bool _dug;

        public Grass(Vector2 position, Color color, Texture2D texture) {
            _position = position;
            _color = color;
            _texture = texture;
        }

        public void Dig()
        {
            _dug = true;
            _color = Color.SaddleBrown;
        }
        public bool isDug()
        {
            return _dug;
        }
    }
}
