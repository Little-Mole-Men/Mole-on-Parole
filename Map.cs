using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2;
using System;
using System.Reflection.Metadata;

namespace Mole_on_Parole
{
    public class Map : IDrawable
    {
        private GridItem[,] _Overworld;
        private GridItem[][] _Underworld;
        private int _sizeX;
        private int _sizeY;
        private int _viewRadius;
        private double _concretePerlinScale = 0.08;
        private double _grassPerlinScale = 0.2;
        private Perlin _grassPerlin;
        private Perlin _concretePerlin;
        private Texture2D _grassTexture;
        private Texture2D _concreteTexture;


        public Map(int sizeX, int sizeY, int seed, Texture2D grass, Texture2D concrete)
        {
            _Overworld = new GridItem[sizeX,sizeY];
            Random r = new Random(seed);
            _grassPerlin = new Perlin(r.Next());
            _concretePerlin = new Perlin(r.Next());
            _grassTexture = grass;
            _concreteTexture = concrete;

            double _concreteChance = 0.5;

            for (int x = 0; x<sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (_concretePerlin.perlin(x * _concretePerlinScale, y * _concretePerlinScale, 0) < _concreteChance)
                    {
                        _Overworld[x,y] = new Concrete(new Vector2(x*32, y*32), new Color(150, 150, 150), _concreteTexture);
                    }
                    else
                    {
                        _Overworld[x,y] = new Grass(new Vector2(x*32, y*32), Color.Green, _grassTexture);
                    }
                }
            }
        }

        public void setViewRadius(int r)
        {
            _viewRadius = r;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            for (int i = 0; i < (_viewRadius) * 2; i++)
            {
                for (int j = 0; j < (_viewRadius) * 2; j++)
                {
                    float a = (((int)(position.X) / 32) - _viewRadius) + i;
                    float b = (((int)(position.Y) / 32) - _viewRadius) + j;

                    float c = (position.X / 32) - a;
                    float d = (position.Y / 32) - b;

                    double sq = Math.Sqrt((c * c) + (d * d));

                    if (sq < _viewRadius)
                    {
                        if ((int)a >= 0 && (int)b >= 0)
                        {
                            _Overworld[(int)(a), (int)(b)].Draw(spriteBatch, position);
                        }

                    }
                }
            }
        }

    }
}
