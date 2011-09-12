using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace IsometricMap.Formations
{
    public class FormationPosition
    {
        Vector2 m_vFormationIndex;

        public FormationPosition(Vector2 posIndex)
        {
            m_vFormationIndex = posIndex;
        }
    }
}
