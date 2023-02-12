using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Mole_on_Parole
{
    public class Mole : IDrawable, IUpdatable
    {
        private int _lives;
        private IValuable _attachedValuable = null;
        private int _digSpaces = 50;
        private Vector2 _velocity;
        private Vector2 _position;
        private float _baseMaxSpeed = 200;
        private float _baseAcceleration;
        private float _acceleration;
        private float _deceleration;
        private float _maxSpeed;
        private Texture2D _texture;
        private int _score;
        public bool Underground { get; set; }

        public Mole(Texture2D moleTexture)
        {
            _lives = 3;
            _texture = moleTexture;
            _velocity = new Vector2(0, 0);
            _baseAcceleration = 2 * _baseMaxSpeed;
            Underground = true;
            _score = 0;
        }

        public bool HasAttachedValuable()
        {
            return _attachedValuable != null;
        }

        public Vector2 GetPosition()
        {
            return _position;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground, Vector2 center)
        {
            Vector2 pos = Vector2.Add(Vector2.Negate(position), _position);
            pos += center;
            spriteBatch.Draw(_texture, pos, null, Color.Brown, 0f, new Vector2(_texture.Width / 2, _texture.Height / 2), Vector2.One, SpriteEffects.None, 0f); 
        }


        public void Update(double totalSeconds, Vector2 position)
        {
            _acceleration = _baseAcceleration / ((_attachedValuable != null) ? _attachedValuable.GetWeight() : 1);
            _deceleration = _baseAcceleration * ((_attachedValuable != null) ? _attachedValuable.GetWeight() : 1);
            _maxSpeed = _baseMaxSpeed / ((_attachedValuable != null) ? _attachedValuable.GetWeight() : 1);
            _position += _velocity * (float)totalSeconds;
        }

        public void Accelerate(Directions direction, float totalSeconds)
        {
            switch (direction)
            {
                case Directions.UP:
                    _velocity.Y = Math.Max((_velocity.Y - _acceleration * totalSeconds), -_maxSpeed);
                    break;
                case Directions.DOWN:
                    _velocity.Y = Math.Min((_velocity.Y + _acceleration * totalSeconds), _maxSpeed);
                    break;
                case Directions.LEFT:
                    _velocity.X = Math.Max((_velocity.X - _acceleration * totalSeconds), -_maxSpeed);
                    break;
                case Directions.RIGHT:
                    _velocity.X = Math.Min((_velocity.X + _acceleration * totalSeconds), _maxSpeed);
                    break;
            }
            if (_velocity.LengthSquared() > Math.Pow(_maxSpeed, 2))
            {
                _velocity.Normalize();
                _velocity *= _maxSpeed;
            }
        }

        public void Slow(Directions direction, float totalSeconds)
        {
            switch (direction)
            {
                case Directions.UP:
                case Directions.DOWN:
                    if (Math.Abs(_velocity.Y) >= Math.Abs(_deceleration * totalSeconds))
                    {
                        _velocity.Y = (_velocity.Y - (Math.Sign(_velocity.Y) * _deceleration * totalSeconds));
                    }
                    else
                    {
                        _velocity.Y = 0;
                    }
                    break;
                case Directions.LEFT:
                case Directions.RIGHT:
                    if (Math.Abs(_velocity.X) >= Math.Abs(_deceleration * totalSeconds))
                    {
                        _velocity.X = (_velocity.X - (Math.Sign(_velocity.X) * _deceleration * totalSeconds));
                    }
                    else
                    {
                        _velocity.X = 0;
                    }
                    break;
            }
        }

        public void GetKilled()
        {
            if (_lives != 0)
            {
                _lives--;
                Console.WriteLine("MOLE HAS BEEN KILLED ");
                Console.WriteLine(_lives);
            };

        }

        public void EatWorm(int value)
        {
            _digSpaces += value;
            Console.WriteLine("Dig spaces up by 2");
        }

        public void SetAttachedValuable(GenericValuable valuable)
        {
            _attachedValuable = valuable;
        }

        public IValuable GetAttachedValuable()
        {
            return this._attachedValuable;
        }

        public int GetScore()
        {
            return _score;
        }

        public void IncreaseScore(int scoreToAdd)
        {
            _score += scoreToAdd;   
        }
    }


}
