using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public interface ICollectible : IDrawable, IUpdatable
    { 
        string Name { get; }
        public bool DetectMoleClose(Mole mole);
    }
}
