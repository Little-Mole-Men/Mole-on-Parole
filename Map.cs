using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project2;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;

namespace Mole_on_Parole
{
    public class Map : IDrawable, IUpdatable
    {
        private GridItem[,] _Overworld;
        private GridItem[,] _Underworld;
        private int _sizeX;
        private int _sizeY;
        private int _viewRadius;
        private double _concretePerlinScale = 0.08;
        private double _grassPerlinScale = 0.2;
        private double _undergroundDugPerlinScale = 0.08;
        private double _undergroundNotDugPerlinScale = 0.02;
        private Perlin _grassPerlin;
        private Perlin _concretePerlin;
        private Perlin _undergroundDugPerlin;
        private Perlin _undergroundNotDugPerlin;
        private Texture2D _grassTexture;
        private Texture2D _concreteTexture;
        private Texture2D _undergroundDugTexture;
        private Texture2D _undergroundNotDugTexture;
        private List<Tuple<int, int>> coordsToDraw;


        public Map(int sizeX, int sizeY, int seed, Texture2D grass, Texture2D concrete, Texture2D undergroundDug, Texture2D undergroundNotDug)
        {
            _Overworld = new GridItem[sizeX, sizeY];
            _Underworld = new GridItem[sizeX, sizeY];
            Random r = new Random(seed);
            _grassPerlin = new Perlin(r.Next());
            _concretePerlin = new Perlin(r.Next());
            _undergroundDugPerlin = new Perlin(r.Next());
            _undergroundNotDugPerlin = new Perlin(r.Next());
            _grassTexture = grass;
            _concreteTexture = concrete;
            _undergroundNotDugTexture = undergroundNotDug;
            _undergroundDugTexture = undergroundDug;
            coordsToDraw = new List<Tuple<int, int>>();

            double _concreteChance = 0.5;
            double _dugChance = 0.3;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (_concretePerlin.perlin(x * _concretePerlinScale, y * _concretePerlinScale, 0) < _concreteChance)
                    {
                        _Overworld[x, y] = new Concrete(new Vector2(x * 32, y * 32), new Color(150, 150, 150), _concreteTexture);
                    }
                    else
                    {
                        _Overworld[x, y] = new Grass(new Vector2(x * 32, y * 32), Color.Green, _grassTexture);
                    }
                    if (_undergroundDugPerlin.perlin(x * _undergroundDugPerlinScale, y * _undergroundDugPerlinScale, 0) < _dugChance)
                    {
                        _Underworld[x, y] = new UndergroundDug(new Vector2(x * 32, y * 32), Color.Gray, _undergroundDugTexture);
                    }
                    else
                    {
                        _Underworld[x, y] = new UndergroundNotDug(new Vector2(x * 32, y * 32), Color.Black, _undergroundNotDugTexture);
                    }
                }
            }
        }

        public void setViewRadius(int r)
        {
            _viewRadius = r;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground, Vector2 center)
        {
            foreach (Tuple<int, int> coord in coordsToDraw)
            {
                _Overworld[coord.Item1, coord.Item2].Draw(spriteBatch, position, underground, center);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, bool underground, Vector2 center, Mole mole)
        {
            foreach (Tuple<int, int> coord in coordsToDraw)
            {
                _Underworld[coord.Item1, coord.Item2].Draw(spriteBatch, position, false, center);
            }
            if (underground) mole.Draw(spriteBatch, position, underground, center);
            foreach (Tuple<int, int> coord in coordsToDraw)
            {
                _Overworld[coord.Item1, coord.Item2].Draw(spriteBatch, position, underground, center);
            }
            if (!underground) mole.Draw(spriteBatch, position, underground, center);
        }

        public void Update(double totalSeconds, Vector2 position)
        {
            coordsToDraw.Clear();
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
                            coordsToDraw.Add(new Tuple<int, int>((int)a, (int)b));
                            if (_Underworld[(int)a,(int)b] is UndergroundNotDug && (_Underworld[(int)a,(int)b] as UndergroundNotDug).Dug)
                            {
                                _Underworld[(int)a,(int)b] = new UndergroundDug(new Vector2(a * 32, b * 32), Color.Gray, _undergroundDugTexture);
                            }
                        }
                    }
                }
            }
        }

        public bool IsClosestGrass(Vector2 position)
        {
            return ((position.X > 0 && position.Y > 0) &&
                (_Overworld[((int)(position.X) / 32), ((int)(position.Y) / 32)] is Grass));
        }

        public bool IsClosestDug(Vector2 position)
        {
            return ((position.X > 0 && position.Y > 0) &&
                (_Overworld[((int)(position.X) / 32), ((int)(position.Y) / 32)] is Grass) &&
                ((_Overworld[((int)(position.X) / 32), ((int)(position.Y) / 32)]) as Grass).isDug());
        }

        public GridItem[,] GetSurroundings(Vector2 position)
        {
            GridItem[,] surroundings = new GridItem[3, 3];
            int midX = (int)(position.X / 32) - 1;
            int midY = (int)(position.Y / 32) - 1;
            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    if (midX + i < 0 || midY + j < 0)
                    {
                        surroundings[i, j] = null;
                    }
                    else surroundings[i, j] = _Underworld[midX + i, midY + j];
                }
            }
            return surroundings;
        }

        internal void DigHole(Vector2 position)
        {
            int x = ((int)(position.X) / 32);
            int y = ((int)(position.Y) / 32);
            if (x > 0 && y > 0)
            {
                GridItem closestPoint = _Overworld[x, y];
                if (closestPoint is Grass)
                {
                    (closestPoint as Grass).Dig();
                    _Underworld[x, y] = new UndergroundDug(new Vector2(x * 32, y * 32), Color.Gray, _undergroundDugTexture);
                }
            }
        }
    }
}
