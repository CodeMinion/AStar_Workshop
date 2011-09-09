using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using IsometricMap.Pathfinder.Base;
using IsometricMap.Pathfinder.A_Star;
namespace IsometricMap.Map
{
    public class MapHandler
    {
        //int m_tileHeight, m_tileWidth;
        //Texture2D m_mapTexture;

        MapLayer m_mlActiveLayer = null;

        PathNode m_currentPath = null;
        List<AStarNode> m_lstClosed = null;
        List<AStarNode> m_lstOpen = null;

        private bool m_bDisplayDebug = true;
        private static MapHandler m_Instance = null;

        public static MapHandler GetInstance()
        {
            if(m_Instance == null)
                m_Instance = new MapHandler();

            return m_Instance;
        }

        private MapHandler()
        {

        }

        public void SetDebug(bool on)
        {
            m_bDisplayDebug = on;
        }
        public bool GetDebug()
        {
            return m_bDisplayDebug;
        }
        public void SetActiveLayer(MapLayer activeLayer)
        {
            m_mlActiveLayer = activeLayer;
        }

        public List<Vector2> GetAdjacentsToTile(Vector2 tilePosition)
        {
            List<Vector2> adjacents = new List<Vector2>();

            int x = (int)(tilePosition.X);
            int y = (int)(tilePosition.Y);

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (j == 0 && i == 0)
                        continue;


                    if ((i == 1 || i == -1) && j == 0)
                        continue;

                    if (tilePosition.Y % 2 == 1)
                    {
                        if (i < 0)
                            continue;
                    }
                    else
                    {
                        if (i > 0)
                            continue;
                    }
                    /*
                    if (((int)tilePosition.Y) % 2 == 0)
                    {
                        if ((j == 1 || j == -1) && i == 0)
                            continue;
                    }
                    */
                    Vector2 temp = Vector2.Add(new Vector2(i, j), tilePosition);
                    if (temp.X < 0)
                        continue;

                    if (temp.Y < 0)
                        continue;

                    if (temp.X >= m_mlActiveLayer.GetMapSize().X)
                        continue;

                    if (temp.X >= m_mlActiveLayer.GetMapSize().Y)
                        continue;

                    if (MapHandler.GetInstance().IsTileSolid(temp))
                        continue;

                    adjacents.Add(temp);
                }
            }
            return adjacents;
        }
        public bool IsTileSolid(Vector2 tileIndex)
        {
            return m_mlActiveLayer.IsTileSolid((int)tileIndex.X, (int)tileIndex.Y);
        }

        public void SetTileColor(Vector2 tileIndex, Color tileColor)
        {
            m_mlActiveLayer.SetTileColor((int)tileIndex.X, (int)tileIndex.Y, tileColor);
        }

        public void ResetPathMarkers()
        {
            if (m_currentPath == null)
                return;

            while (m_currentPath != null)
            {
                SetTileColor(m_currentPath.GetNodePosition(), Color.White);
                m_currentPath = m_currentPath.GetNextNode();
            }
        }

        public void BuildPathMarker(PathNode newPath)
        {
            ResetPathMarkers();
            m_currentPath = newPath;
            while (newPath != null)
            {
                SetTileColor(newPath.GetNodePosition(), Color.Blue);
                newPath = newPath.GetNextNode();
            }
            
        }
        public void SaveClosedList(List<AStarNode> closed)
        {
            m_lstClosed = closed;
        }
        public void SaveOpenList(List<AStarNode> open)
        {
            m_lstOpen = open;
        }

        public void ResetOpenList()
        {
            if (m_lstOpen == null)
                return;

            else
            {
                foreach (PathNode node in m_lstOpen)
                {
                    SetTileColor(node.GetNodePosition(), Color.White);
                }
            }
        }
        public void ResetClosedList()
        {
            if (m_lstClosed == null)
                return;

            else
            {
                foreach (PathNode node in m_lstClosed)
                {
                    SetTileColor(node.GetNodePosition(), Color.White);
                }
            }
        }
        public Vector2 GetTilePosition(Vector2 tileIndex)
        {
            if (m_mlActiveLayer == null)
                return new Vector2(-1,-1);


            return m_mlActiveLayer.GetTilePosition(tileIndex);
        }
        public Rectangle GetSourceRectByID(int tileID, int imageWidth, int imageHeight, int tileWidth, int tileHeight)
        {
            Rectangle sourceRect = new Rectangle();

            int tilesAccross = imageWidth / tileWidth;
            int tilesTopDown = imageHeight / tileHeight;

            int yPos = tileID / tilesAccross;
            int xPos = tileID % tilesAccross;

            sourceRect.X = xPos * tileWidth;
            sourceRect.Y = yPos * tileHeight;
            sourceRect.Width = tileWidth;
            sourceRect.Height = tileHeight;

            return sourceRect;
        }

        public Vector2 GetTileSetSizeByTiles(Texture2D tileSet, Vector2 tileSize)
        {
            if (tileSet == null || tileSize == null)
                return Vector2.Zero;

            return new Vector2(tileSet.Width / tileSize.X, tileSet.Height / tileSize.Y);

        }

        public Vector2 GetTileIndex(Vector2 tilePos)
        {
            Vector2 finalPos = new Vector2();
            finalPos.X = tilePos.X;

            int x;
            int y = (int)(tilePos.Y / (int)(m_mlActiveLayer.GetTileSize().Y * 0.25));
            if (y % 2 == 1)
            {
                x = (int)((tilePos.X - m_mlActiveLayer.GetTileSize().X/2) / m_mlActiveLayer.GetTileSize().X);
            }
            else
            {
                x = (int)(tilePos.X / m_mlActiveLayer.GetTileSize().X);
            }

            finalPos.X = x;
            finalPos.Y = y;
            return finalPos;
        }
        public void SetTileSolid(Vector2 tileIndex, bool isSolid)
        {
            if (m_mlActiveLayer == null)
                return;

            m_mlActiveLayer.SetTileToSolid((int)tileIndex.X, (int)tileIndex.Y, isSolid);
        }
        
    }
}
