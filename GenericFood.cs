using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private const int value = 10;
        private int _frameTimer = 0;
        private int _frameDirection = 0;
        private int _animationFrame = 0;

        public Earthworm(Texture2D wormTexture, Vector2 position)
        {
            _texture = wormTexture;
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground, Vector2 center)
        {
            _frameTimer++;

            if (_frameTimer == 5)
            {
                _frameTimer = 0;
                if (_frameDirection == 0)
                {
                    if (_animationFrame == 3)
                    {
                        _animationFrame--;
                        _frameDirection = 1;
                    }
                    else
                    {
                        _animationFrame++;
                    }


                }
                else if (_frameDirection == 1)
                {
                    if (_animationFrame == 0)
                    {
                        _animationFrame++;
                        _frameDirection = 0;
                    }
                    else
                    {
                        _animationFrame--;
                    }
                }
            }
            Vector2 pos = Vector2.Add(Vector2.Negate(position), _position);
            pos += center;

            spriteBatch.Draw(
                    _texture,
                    pos,
                    new Rectangle(32 * _animationFrame, 0, 32, 32),
                    Color.White,
                    0f,
                    new Vector2(_texture.Width / 8, _texture.Height / 2),
                    Vector2.One,
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
