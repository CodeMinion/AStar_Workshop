using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace IsometricMap.Pathfinder.Base
{
    public abstract class PathNode
    {
        protected Vector2 m_vNodePosition;
        /// <summary>
        /// Reference to the next node
        /// in the path.
        /// </summary>
        private PathNode m_asnNext = null;
        /// <summary>
        /// Reference to the previous node 
        /// in the path.
        /// </summary>
        private PathNode m_asnPrev = null;

        /// <summary>
        /// This method returns a given node's physical position. 
        /// </summary>
        /// <returns></returns>
        public Vector2 GetNodePosition()
        {
            return m_vNodePosition;
        }
        public void SetNodePosition(Vector2 pos)
        {
            m_vNodePosition = pos;
        }

        public void SetNextNode(PathNode next)
        {
            m_asnNext = next;
        }
        public PathNode GetNextNode()
        {
            return m_asnNext;
        }

        public void SetPrevNode(PathNode prev)
        {
            m_asnPrev = prev;
        }
        public PathNode GetPrevNode()
        {
            return m_asnPrev;
        }

        
    }
}
