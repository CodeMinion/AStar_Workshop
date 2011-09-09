using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using IsometricMap.Pathfinder.Base;
namespace IsometricMap.Pathfinder.A_Star
{
    public class AStarNode:PathNode, IEquatable<AStarNode>, IComparable<AStarNode>
    {
        /// <summary>
        /// Cost of this node.
        /// </summary>
        private double m_dGCost;

        /// <summary>
        /// Cheapest way to get 
        /// from this node to the 
        /// destination. This is a 
        /// heruistic value and in most
        /// maps is the Euclidian distance.
        /// </summary>
        private double m_dHCost;

        
        /// <summary>
        /// This method calculates the f(n) cost for this node.
        /// f(n) = g(n) + h(n)
        /// </summary>
        /// <returns>f(n) cost for the node.</returns>
        public double GetFCost()
        {
            return m_dGCost + m_dHCost;
        }

        
        public bool Equals(AStarNode other)
        {
            if (other == null)
                return false;

            return m_vNodePosition.Equals(other.m_vNodePosition);
        }

        public int CompareTo(AStarNode other)
        {
            if (other == null)
                return 1;

            return GetFCost().CompareTo(other.GetFCost());
        }

        public void SetGCost(double g)
        {
            m_dGCost = g;
        }
        public double GetGCost()
        {
            return m_dGCost;
        }

        public void SetHCost(double h)
        {
            m_dHCost = h;
        }

        public double GetHCost()
        {
            return m_dHCost;
        }

        
    }
}
