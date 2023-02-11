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
    public class Man : IUpdatable, IDrawable
    {
        private IValuable _attachedCollectible = null;
        private Vector2 _velocity;
        private Vector2 _position;
        private float _baseMaxSpeed = 50;
        private float _killRange = 20;
        private float _baseAcceleration;
        private float _acceleration;
        private float _maxSpeed;
        private Vector2 _direction;
        private Texture2D _texture;

        public Man(Texture2D manTexture, Vector2 position)
        {
            _texture = manTexture;
            _velocity = new Vector2(0, 0);
            _baseAcceleration = 2 * _baseMaxSpeed;
            _position = position;
        }


        public void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Draw(
                _texture,
                _position,
                null,
                Color.White,
                0f,
                new Vector2(_texture.Width / 2, _texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );

        }

        public void UpdatePos(double totalSeconds, Vector2 molePosition)
        {
            _direction = (molePosition - _position);
            _direction.Normalize();
            _velocity = _direction * _baseMaxSpeed;
            _position += _velocity * (float) totalSeconds;
        }

        public void Update(double totalSeconds)
        {
            throw new NotImplementedException();
        }

        public void DetectAndKillMole(double totalSeconds, Mole mole)
        {
            if (Vector2.Distance(_position, mole.getPosition()) <= _killRange)
            {
                mole.GetKilled();
            }
        }
    }
}
