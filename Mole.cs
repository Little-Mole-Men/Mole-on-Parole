using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public class Mole : IDrawable, IUpdatable
    {
        private int _lives;
        private ICollectible _attachedCollectible;

        public Mole()
        {
            _lives = 3;
        }
        public void Draw()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
