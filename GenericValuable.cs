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
    public class GenericValuable : IValuable
    {
        public string Name => "";

        private int eatRange = 20;

        private Texture2D _texture;
        private Vector2 _position;
        private int _weight;
        private int _value;
        private bool _attached;

        public GenericValuable(Texture2D texture, Vector2 position, int weight, int value)
        {
            _texture = texture;
            _position = position;
            _weight = weight;
            _value = value;
            _attached = false;
        }

        public bool getAttached()
        {
            return _attached;
        }

        public void SetPosition(Vector2 molePosition)
        {
            _position = molePosition + new Vector2(-35, 0);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground, Vector2 center)
        {
            Vector2 scale = new Vector2(0.2f, 0.2f); //80% smaller
            Vector2 pos = Vector2.Add(Vector2.Negate(position), _position);
            pos += center;

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

        public float GetWeight()
        {
            return _weight;
        }

        public void Update(double totalSeconds, Vector2 position)
        {
            throw new NotImplementedException();
        }

        public void DetectMoleClose(Mole mole)
        {
            if ((Vector2.Distance(_position, mole.GetPosition()) <= eatRange) && !mole._HasAttachedValuable())
            {
                mole.SetAttachedValuable(this);
                Console.WriteLine("Mole picked up valuable");
                _attached = true;
                mole.IncreaseScore(_value);
                
            }
            
        }
    }
}
