using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Mole_on_Parole
{
    public class Man : IUpdatable, IDrawable
    {
        private IValuable _attachedCollectible = null;
        private Vector2 _velocity;
        private Vector2 _position;
        private float _baseMaxSpeed = 200;
        private float _killRange = 20;
        private float _baseAcceleration;
        private float _acceleration;
        private float _maxSpeed;
        private Vector2 _direction;
        private Texture2D _texture;
        public int _frameTimer = 0;
        public int _animationFrame = 0;
        private int _lastDirection = 0;

        public Man(Texture2D manTexture, Vector2 position)
        {
            _texture = manTexture;
            _velocity = new Vector2(0, 0);
            _baseAcceleration = 2 * _baseMaxSpeed;
            _position = position;
        }


        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground, Vector2 center)
        {
            _frameTimer++;

            Vector2 pos = Vector2.Add(Vector2.Negate(position), _position);
            pos += center;

            if (_frameTimer == 5)
            {
                _frameTimer = 0;
                _animationFrame = (_animationFrame + 1) % 4;
            }

            int spr = 2;
            if (_velocity.X == 0 && _velocity.Y == 0)
            {
                spr = _lastDirection;
            }
            else
            {
                if ((_velocity.X * _velocity.X) > (_velocity.Y * _velocity.Y))
                {
                    if (_velocity.X > 0)
                    {
                        spr = 0;
                    }
                    else
                    {
                        spr = 1;

                    }
                }
                else
                {
                    if (_velocity.Y > 0)
                    {
                        spr = 0;
                    }
                    else
                    {
                        spr = 1;
                    }

                }
                _lastDirection = spr;
            }
            Rectangle sourceRectangle = new Rectangle(64 * _animationFrame, (128 * spr), 64, 128);
            spriteBatch.Draw(_texture, pos, sourceRectangle, Color.White, 0f, new Vector2(_texture.Width / 4, _texture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }

        public void Update(double totalSeconds, Vector2 molePosition)
        {
            _direction = (molePosition - _position);
            if(_direction.LengthSquared() != 0) _direction.Normalize();
            _velocity = _direction * _baseMaxSpeed;
            _position += _velocity * (float) totalSeconds;
        }

        public bool DetectAndKillMole(double totalSeconds, Mole mole)
        {
            if (Vector2.Distance(_position, mole.GetPosition()) <= _killRange)
            {
                return mole.GetKilled(totalSeconds);
            }
            else return false;
        }
    }
}
