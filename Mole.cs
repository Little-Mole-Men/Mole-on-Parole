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
        private double _health;
        private const double _maxHealth = 100;
        private const double _depletionRate = 100;
        private IValuable _attachedValuable = null;
        private int _digSpaces = 50;
        private Vector2 _velocity;
        private Vector2 _position;
        private const float _baseMaxSpeed = 200;
        private float _baseAcceleration;
        private float _acceleration;
        private float _deceleration;
        private float _maxSpeed;
        private Texture2D _texture;
        private int _score;
        private Vector2 _originalPosition;
        public bool Underground { get; set; }
        public int _frameTimer = 0;
        public int _animationFrame = 0;
        private int _lastDirection = 0;
        private Directions _digDirection = Directions.UP;
        private bool _digging = false;
        private GridItem[,] _surroundings;
        private double _digTime;

        public Mole(Texture2D moleTexture)
        {
            _lives = 3;
            _texture = moleTexture;
            _velocity = new Vector2(0, 0);
            _baseAcceleration = 2 * _baseMaxSpeed;
            Underground = true;
            _score = 0;
            _health = _maxHealth;
            _surroundings = new GridItem[3, 3];
            _digTime = 0;
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
            if (_originalPosition.X == 0) _originalPosition = position;
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
                _animationFrame = (_animationFrame + 1) % 2;
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
                        spr = 2;
                    }
                    else
                    {
                        spr = 3;
                    }

                }
                _lastDirection = spr;
            }
            Rectangle sourceRectangle = new Rectangle(32 * _animationFrame, (32 * spr), 32, 32);
            spriteBatch.Draw(_texture, pos, sourceRectangle, Color.White, 0f, new Vector2(_texture.Width / 4, _texture.Height / 6), Vector2.One, SpriteEffects.None, 0f);

        }

        public void setSurroundings(GridItem[,] surroundings)
        {
            _surroundings = surroundings;
        }


        public void Update(double totalSeconds, Vector2 position)
        {
            if (!Underground)
            {
                _digging = false;
                _digTime = 0;
            }
            else if (_digging) _digTime += totalSeconds;
            _acceleration = _baseAcceleration / ((_attachedValuable != null) ? _attachedValuable.GetWeight() : 1);
            _deceleration = _baseAcceleration * ((_attachedValuable != null) ? _attachedValuable.GetWeight() : 1);
            _maxSpeed = _baseMaxSpeed / ((_attachedValuable != null) ? _attachedValuable.GetWeight() : 1);
            if (_digging && _digTime > 0.5)
            {
                switch (_digDirection)
                {
                    case Directions.UP:
                        _surroundings[1, 0].Dig();
                        break;
                    case Directions.DOWN:
                        _surroundings[1, 2].Dig();
                        break;
                    case Directions.LEFT:
                        (_surroundings[0, 1]).Dig();
                        break;
                    case Directions.RIGHT:
                        (_surroundings[2, 1]).Dig();
                        break;
                }
                _digging = false;
                _digTime = 0;
            }
            float xPosInBlock = _position.X % 32;
            float yPosInBlock = _position.Y % 32;
            if (Underground)
            {
                if (_velocity.X > 0 && _surroundings[2, 1] is UndergroundNotDug && !((_surroundings[2, 1] as UndergroundNotDug).Dug)
                    && xPosInBlock + _velocity.X * (float)totalSeconds > 16)
                {
                    Console.WriteLine("blocked");
                    _position.X += (16 - xPosInBlock);
                }
                else if (_velocity.X < 0 && _surroundings[0, 1] is UndergroundNotDug && !(_surroundings[0, 1] as UndergroundNotDug).Dug
                        && xPosInBlock + _velocity.X * (float)totalSeconds < 16)
                {
                    _position.X -= (xPosInBlock - 16);
                }
                else
                {
                    _position.X += _velocity.X * (float)totalSeconds;
                }
                if (_velocity.Y > 0 && _surroundings[1, 2] is UndergroundNotDug && !(_surroundings[1, 2] as UndergroundNotDug).Dug
                    && yPosInBlock + _velocity.Y * (float)totalSeconds > 16)
                {
                    _position.Y += (16 - yPosInBlock);
                }
                else if (_velocity.Y < 0 && _surroundings[1, 0] is UndergroundNotDug && !(_surroundings[1, 0] as UndergroundNotDug).Dug
                        && yPosInBlock + _velocity.Y * (float)totalSeconds < 16)
                {
                    _position.Y -= (yPosInBlock - 16);
                }
                else
                {
                    _position.Y += _velocity.Y * (float)totalSeconds;
                }
            }
            else
            {
                _position += _velocity * (float)totalSeconds;
            }
        }

        public void Accelerate(Directions direction, float totalSeconds)
        {
            switch (direction)
            {
                case Directions.UP:
                    _velocity.Y = Math.Max((_velocity.Y - _acceleration * totalSeconds), -_maxSpeed);
                    if (Underground && !_digging && _surroundings[1, 0] is UndergroundNotDug)
                    {
                        _digging = true;
                        if (_digDirection != Directions.UP) _digTime = 0;
                        _digTime += totalSeconds;
                        _digDirection = direction;
                    }
                    break;
                case Directions.DOWN:
                    _velocity.Y = Math.Min((_velocity.Y + _acceleration * totalSeconds), _maxSpeed);
                    if (Underground && !_digging && _surroundings[1, 2] is UndergroundNotDug)
                    {
                        _digging = true;
                        if (_digDirection != direction) _digTime = 0;
                        _digTime += totalSeconds;
                        _digDirection = direction;
                    }
                    break;
                case Directions.LEFT:
                    _velocity.X = Math.Max((_velocity.X - _acceleration * totalSeconds), -_maxSpeed);
                    if (Underground && !_digging && _surroundings[0, 1] is UndergroundNotDug)
                    {
                        _digging = true;
                        if (_digDirection != direction) _digTime = 0;
                        _digTime += totalSeconds;
                        _digDirection = direction;
                    }
                    break;
                case Directions.RIGHT:
                    _velocity.X = Math.Min((_velocity.X + _acceleration * totalSeconds), _maxSpeed);
                    if (Underground && !_digging && _surroundings[2, 1] is UndergroundNotDug)
                    {
                        _digging = true;
                        if (_digDirection != direction) _digTime = 0;
                        _digTime += totalSeconds;
                        _digDirection = direction;
                    }
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
                    if (_digDirection == Directions.UP || _digDirection == Directions.DOWN)
                    {
                        _digging = false;
                        _digTime = 0;
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
                    if (_digDirection == Directions.LEFT || _digDirection == Directions.RIGHT)
                    {
                        _digging = false;
                        _digTime = 0;
                    }
                    break;
            }
        }

        public bool GetKilled(double totalSeconds)
        {
            if (_health != 0)
            {
                _health -= _depletionRate * totalSeconds;
            }
            if (_health <= 0)
            {
                _lives--;
                _reset();
            }
            if (_lives == 0)
            {
                Console.WriteLine("MOLE HAS BEEN KILLED ");
                Console.WriteLine(_lives);
                return true;
            };
            return false;
        }

        private void _reset()
        {
            _position = _originalPosition;
            _attachedValuable = null;
            _velocity = new Vector2(0, 0);
            Underground = true;
            _digging = false;
            _digTime = 0;
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

        public int GetLives()
        {
            return _lives;
        }
    }
}
