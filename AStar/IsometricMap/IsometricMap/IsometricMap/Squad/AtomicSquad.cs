using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Entities;
namespace IsometricMap.Squad
{
    /// <summary>
    /// An atomic squad is that which holds 
    /// a single game entity.
    /// </summary>
    public class AtomicSquad:BaseSquad
    {

        GameEntity m_geSquadEntity;

        public AtomicSquad(GameEntity ent)
        {
            m_geSquadEntity = ent;
        }

        /// <summary>
        /// Atomic squads are a single memeber squad
        /// thus there is no need to implement the add/remove
        /// methods.
        /// </summary>
        /// <param name="squad"></param>
        public override void AddSquad(BaseSquad squad)
        {}
        public override void RemoveSquad(BaseSquad squad)
        {}

    }
}
