using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using IsometricMap.Map;
using IsometricMap.Pathfinder.Base;
namespace IsometricMap.Pathfinder.A_Star
{
    public class AStarPathFinder:PathFinder
    {
        List<AStarNode> m_lstClosed;
        List<AStarNode> m_lstOpen;

        public override PathNode FindPath(Vector2 startPos, Vector2 destPos)
        {
            MapHandler.GetInstance().ResetClosedList();
            MapHandler.GetInstance().ResetOpenList();
            
            m_lstClosed = new List<AStarNode>();
            m_lstOpen = new List<AStarNode>();

            // Insert start node in the open list.
            AStarNode node = new AStarNode();
            node.SetGCost(0);
            node.SetHCost(HeuristicCostStimation(startPos, destPos));
            node.SetNodePosition(startPos);

            if (MapHandler.GetInstance().IsTileSolid(startPos) || MapHandler.GetInstance().IsTileSolid(destPos))
                return null;

            // Place node in the open list.
            m_lstOpen.Add(node);

            while (m_lstOpen.Count > 0)
            {
                m_lstOpen.Sort();
                node = m_lstOpen[0];
                if (node.GetNodePosition().Equals(destPos))
                {
                    MapHandler.GetInstance().SaveClosedList(m_lstClosed);
                    MapHandler.GetInstance().SaveOpenList(m_lstOpen);


                    return ReconstructPath(node);
                }
                // Remove from open list.
                m_lstOpen.Remove(node);
                // Add to closed list
                m_lstClosed.Add(node);

                
                bool tentativeIsBeter = false;
                double tentativeGScore = 0;
                // Get Adjacents tiles
                List<Vector2> adjacents = MapHandler.GetInstance().GetAdjacentsToTile(node.GetNodePosition());
                // For each neighbour node N in "node"
                foreach (Vector2 pos in adjacents)
                {
                    AStarNode neighbour = new AStarNode();
                    neighbour.SetNodePosition(pos);
                    MapHandler.GetInstance().SetTileColor(neighbour.GetNodePosition(), Color.Gray);
                
                    //If it is in the close set then we skip it
                    if (m_lstClosed.Contains(neighbour))
                        continue;

                    // We no longer get solid tiles as valid adjecent.
                    //if (MapHandler.GetInstance().IsTileSolid(pos))
                    //    continue;
                    
                    tentativeGScore = node.GetGCost() + DistanceBetweenNodes(node, neighbour);
                    if (!m_lstOpen.Contains(neighbour))
                    {
                        m_lstOpen.Add(neighbour);
                        tentativeIsBeter = true;
                    }
                    else if (tentativeGScore < neighbour.GetGCost())
                    {
                        tentativeIsBeter = true;
                    }
                    else
                    {
                        tentativeIsBeter = false;
                    }

                    if (tentativeIsBeter)
                    {
                        neighbour.SetPrevNode(node);
                        neighbour.SetGCost(tentativeGScore);
                        neighbour.SetHCost(HeuristicCostStimation(neighbour.GetNodePosition(), destPos));
                    }
                }
  
            }
            MapHandler.GetInstance().SaveOpenList(m_lstOpen);
            MapHandler.GetInstance().SaveClosedList(m_lstClosed);

            return ReconstructPath((AStarNode) m_lstClosed[m_lstClosed.Count-1]);
        }
        private double DistanceBetweenNodes(AStarNode n1, AStarNode n2)
        {
            double dist = Vector2.Distance(n1.GetNodePosition(), n2.GetNodePosition());
            return dist;
        }
        private PathNode ReconstructPath(AStarNode path)
        {
            AStarNode temp = path;
            if (temp == null)
                return null;

            // Skip the start position as a node in the path
            // because we are already there.
            while (temp.GetPrevNode()!= null)
            {
                // Get the reference to previous node.
                AStarNode prev = (AStarNode)temp.GetPrevNode();

                //MapHandler.GetInstance().SetTileColor(temp.GetNodePosition(), Color.Blue);
                // Make the previous node' next reference this one.
                prev.SetNextNode(temp);
                // Make the previous node, our current node.
                temp = prev;

            }
            //MapHandler.GetInstance().SetTileColor(temp.GetNodePosition(), Color.Blue);

            MapHandler.GetInstance().BuildPathMarker(temp);
            //List<PathNode> m_path = new List<PathNode>();
            //m_path.Add(temp);
            return temp;//m_path;
        }

        private double HeuristicCostStimation(Vector2 startPos, Vector2 endPos)
        {
            return Vector2.Distance(startPos, endPos);
        }
    }
}
