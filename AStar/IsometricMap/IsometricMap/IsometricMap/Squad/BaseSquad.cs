using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Formations;
namespace IsometricMap.Squad
{
    /// <summary>
    /// Base class for the squad definition.
    /// </summary>
    public abstract class BaseSquad
    {
        Formation m_fCurrFormation;

        public void SetCurrentFormation(Formation curr)
        {
            m_fCurrFormation = curr;
        }

        public abstract void AddSquad(BaseSquad squad);
        public abstract void RemoveSquad(BaseSquad squad);
    }
}
