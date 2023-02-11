using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Mole_on_Parole
{
    public class Earthworm : IFood
    {
        public string Name => "Earthworm";

        private Vector2 _position;
        private Texture2D _texture;

        private int eatRange = 30;

        private const int value = 2;


        public Earthworm(Texture2D wormTexture, Vector2 position)
        {
            _texture = wormTexture;
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground)
        {
            Vector2 scale = new Vector2(0.5f, 0.5f); //50% smaller
            Vector2 pos = Vector2.Add(Vector2.Negate(position), _position);
            pos += new Vector2(255, 255);

            spriteBatch.Draw(
                _texture,
                pos,
                null,
                Color.White,
                0f,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public float GetValue()
        {
            return value;
        }

        public void Update(double totalSeconds, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public bool DetectMoleClose(Mole mole)
        {
            if (Vector2.Distance(_position, mole.GetPosition()) <= eatRange)
            {
                mole.EatWorm(value);
                return true;
            }
            return false;
        }
    }
}
