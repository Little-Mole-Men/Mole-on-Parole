using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public class GenericValuable : IValuable
    {
        public string Name => throw new NotImplementedException();

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public float GetWeight()
        {
            throw new NotImplementedException();
        }

        public void Update(double totalSeconds)
        {
            throw new NotImplementedException();
        }
    }
}
