using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Entities;
namespace IsometricMap.Squad
{
    /// <summary>
    /// Composite squads are composed of a single leader 
    /// and a list of sub-squads. This subsquads can be 
    /// single atomic squads or can be composite squads.
    /// </summary>
    public class CompositeSquad:BaseSquad
    {
        GameEntity m_geSquadLeader;
        List<BaseSquad> m_lstSquad;

        public CompositeSquad(GameEntity squadLeader)
        {
            m_geSquadLeader = squadLeader;
            m_lstSquad = new List<BaseSquad>();
        }
        /// <summary>
        /// Adds the desired squad.
        /// </summary>
        /// <param name="squad"></param>
        public override void AddSquad(BaseSquad squad)
        {
            m_lstSquad.Add(squad);
        }

        /// <summary>
        /// Removes the desired squad.
        /// </summary>
        /// <param name="squad"></param>
        public override void RemoveSquad(BaseSquad squad)
        {
            m_lstSquad.Remove(squad);
        }
    }
}
