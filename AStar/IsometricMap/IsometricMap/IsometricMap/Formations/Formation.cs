using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace IsometricMap.Formations
{
    public class Formation
    {
        List<FormationPosition> m_lstFormation;

        public Formation()
        {
            m_lstFormation = new List<FormationPosition>();
        }

        public void AddFormationPosition(Vector2 posIndex)
        {
            FormationPosition pos = new FormationPosition(posIndex);
            m_lstFormation.Add(pos);
        }
    }
}
