using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
namespace IsometricMap.Pathfinder.Base
{
    public abstract class PathFinder
    {
        /// <summary>
        /// This method creates a path to the destination
        /// if any exists.
        /// </summary>
        /// <param name="startPos">Start Position</param>
        /// <param name="destPos">Destination Position</param>
        /// <returns>A path to destination, null if no path exists.
        /// The return node is the initial node in the path. 
        /// Paths are represented as a linked list.</returns>
        public abstract PathNode FindPath(Vector2 startPos, Vector2 destPos);
    }
}
