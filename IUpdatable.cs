﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mole_on_Parole
{
    public interface IUpdatable
    {
        public void Update(double totalSeconds, Vector2 position);
    }
}
