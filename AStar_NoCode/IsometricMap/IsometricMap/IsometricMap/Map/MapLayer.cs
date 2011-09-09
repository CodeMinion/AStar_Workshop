using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace IsometricMap.Map
{
    public class MapLayer
    {
        Texture2D m_tileSetRef;
        Vector2 m_vMapSize;
        Vector2 m_vTileSize;

        List<List<MapTile>> m_tile2DList;
        
        Vector2 m_DrawViewStart;
        Vector2 m_DrawViewEnd;
        Vector2 m_viewOffset;

        /// <summary>
        /// Constructor for the MapLayer
        /// </summary>
        /// <param name="mapSize">The number of tiles that the map can have.
        /// X = Number of tiles let to right.
        /// Y = Number of tiles top to bottom.</param>
        /// <param name="tileSize">Size of the tile in pixes.
        /// X = Width in pixels.
        /// Y = Height in pixels.</param>
        public MapLayer(Vector2 mapSize, Vector2 tileSize)
        {
            SetMapSize(mapSize);
            SetTileSize(tileSize);
            
            SetDrawViewEnd(mapSize);
            m_tile2DList = new List<List<MapTile>>();
            for (int i = 0; i < (int)GetMapSize().Y; i++)
            {
                m_tile2DList.Add(new List<MapTile>((int)GetMapSize().X));
                for (int j = 0; j < (int)GetMapSize().X; j++)
                {
                    m_tile2DList[i].Add(null);
                }
            }
        }
        /// <summary>
        /// Constructor for the MapLayer
        /// </summary>
        /// <param name="mapSize">The number of tiles that the layer can have.
        /// X = Number of tiles let to right.
        /// Y = Number of tiles top to bottom.</param>
        /// <param name="tileSize">Size of the tile in pixes.
        /// X = Width in pixels.
        /// Y = Height in pixels.</param>
        /// <param name="vieportSize"> The number of tiles to be displayed out
        /// of the entire layer.
        /// X = Number of tiles left to right.
        /// Y = Number of tiles top to bottom.</param>
        public MapLayer(Vector2 mapSize, Vector2 tileSize, Vector2 vieportSize)
            : this(mapSize, tileSize)
        {
            SetDrawViewEnd(vieportSize);
        }
        public void SetTileSetRef(Texture2D tileSet)
        {
            m_tileSetRef = tileSet;

            for (int i = 0; i < m_vMapSize.Y; i++)
            {
                for (int j = 0; j < m_vMapSize.X; j++)
                {
                    if(m_tile2DList[i][j] != null)
                        m_tile2DList[i][j].SetTileSet(m_tileSetRef);
                }
            }

        }
        public Texture2D GetTileSetRef()
        {
            return m_tileSetRef;
        }
        /// <summary>
        /// Sets the size of the map in number of tiles.
        /// </summary>
        /// <param name="size">Number of tiles in the map.
        /// X = Number of tiles from left to right.
        /// Y = Number of tiles from top to bottom.</param>
        public void SetMapSize(Vector2 size)
        {
            if (size == null)
                return;

            if (m_vMapSize == null)
                m_vMapSize = new Vector2();

            m_vMapSize.X = size.X;
            m_vMapSize.Y = size.Y;
        }

        public Vector2 GetMapSize()
        {
            return m_vMapSize;
        }
        /// <summary>
        /// Sets the tile size.
        /// </summary>
        /// <param name="size">Size of the tile in pixels.
        /// X = Width in pixels.
        /// Y = Height in pixels.</param>
        public void SetTileSize(Vector2 size)
        {
            if (size == null)
                return;

            if (m_vTileSize == null)
                m_vTileSize = new Vector2();

            m_vTileSize.X = size.X;
            m_vTileSize.Y = size.Y;
        }

        public Vector2 GetTileSize()
        {
            return m_vTileSize;
        }

        /// <summary>
        /// Helper function to handle the scrolling of the map.
        /// </summary>
        /// <param name="direction"></param>
        private void ScrollLayer(Vector2 direction)
        {
            if (direction == null)
                return;

            if (direction.X + GetDrawViewStart().X >= 0
                && direction.X + GetDrawViewStart().X +  GetDrawViewEnd().X < GetMapSize().X
                && direction.Y + GetDrawViewStart().Y >= 0
                && direction.Y + GetDrawViewStart().Y + GetDrawViewEnd().Y < GetMapSize().Y)
            {
                Vector2 start = Vector2.Add(GetDrawViewStart(), direction);
                SetDrawViewStart(start);
                m_viewOffset.X = start.X * -1 * GetTileSize().X;
                m_viewOffset.Y = start.Y * -1 * (GetTileSize().Y * 0.25f);

                UpdateVeiwPortTilesOffSet();
                
            }

           
        }

        /// <summary>
        /// This method sets the specified tile's solid
        /// property. This method only sets the properties 
        /// for tiles that are inside the map. If the 
        /// specified tile is no in the map then nothing 
        /// occurs.
        /// </summary>
        /// <param name="x">Column index of the tile.</param>
        /// <param name="y">Row index of the tile.</param>
        /// <param name="isSolid">Should the tile be solid?</param>
        public void SetTileToSolid(int x, int y, bool isSolid)
        {
            if (x < 0)
                return;
            if (y < 0)
                return;

            if (x > m_vMapSize.X)
                return;
            if (y > m_vMapSize.Y)
                return;

            if(m_tile2DList[y][x] != null)
                m_tile2DList[y][x].SetTileSolid(isSolid);
        }
        public bool IsTileSolid(int x, int y)
        {
            if (x < 0)
                return false;
            if (y < 0)
                return false;

            if (x >= m_vMapSize.X)
                return false;

            if (y >= m_vMapSize.Y)
                return false;

            if (m_tile2DList == null)
                return false;

            return m_tile2DList[y][x].IsTileSolid();
        }

        public void SetTileColor(int x, int y, Color color)
        {
            if (x < 0)
                return;
            if (y < 0)
                return;

            if (x >= m_vMapSize.X)
                return;

            if (y >= m_vMapSize.Y)
                return;

            if (m_tile2DList == null)
                return;

            m_tile2DList[y][x].SetTileColor(color);
        
        }
        /// <summary>
        /// Updates the layer. Only Updates the
        /// tiles that are being displayed.
        /// </summary>
        /// <param name="time"></param>
        public void Update(GameTime time)
        {
            for (int i = (int)GetDrawViewStart().Y; i < (int)GetMapSize().Y && i < (int)GetDrawViewStart().Y + (int)GetDrawViewEnd().Y; i++)
            {
                for (int j = (int)GetDrawViewStart().X; j < (int)GetMapSize().X && j < (int)GetDrawViewStart().X + (int)GetDrawViewEnd().X; j++)
                {
                    m_tile2DList[i][j].UpdateTile(time);
                }
            }
        }
        /// <summary>
        /// Scrolls the layer left.
        /// </summary>
        public void ScrollLeft()
        {
            Vector2 left = new Vector2(-1, 0);
            ScrollLayer(left);
        }
        /// <summary>
        /// Scrolls the layer right.
        /// </summary>
        public void ScrollRight()
        {
            Vector2 right = new Vector2(1, 0);
            ScrollLayer(right);
        }
        /// <summary>
        /// Scrolls the layer up.
        /// </summary>
        public void ScrollUp()
        {
            Vector2 up = new Vector2(0, -1);
            ScrollLayer(up);
        }
        /// <summary>
        /// Scroll the layer down.
        /// </summary>
        public void ScrollDown()
        {
            Vector2 down = new Vector2(0, 1);
            ScrollLayer(down);
        }

        /// <summary>
        /// Sets the starts of view of the layer.
        /// This represents the indexs in the map 
        /// 2D array from which to start drawing. 
        /// </summary>
        /// <param name="drawStart"></param>
        public void SetDrawViewStart(Vector2 drawStart)
        {
            if (m_DrawViewStart == null)
                m_DrawViewStart = new Vector2();

            if (drawStart == null)
                return;

            m_DrawViewStart.X = drawStart.X;
            m_DrawViewStart.Y = drawStart.Y;
        }

        public Vector2 GetDrawViewStart()
        {
            return m_DrawViewStart;
        }
        /// <summary>
        /// Sets the offset for every tile in the viewport.
        /// </summary>
        public void UpdateVeiwPortTilesOffSet()
        {
            for (int i = (int)GetDrawViewStart().Y; i < (int)GetMapSize().Y && i < (int)GetDrawViewStart().Y + (int)GetDrawViewEnd().Y; i++)
            {
                for (int j = (int)GetDrawViewStart().X; j < (int)GetMapSize().X && j < (int)GetDrawViewStart().X + (int)GetDrawViewEnd().X; j++)
                {
                    m_tile2DList[i][j].SetTileOffset(m_viewOffset);
                }
            }
        }
        /// <summary>
        /// Sets the number of tiles to draw at any point in time.
        /// </summary>
        /// <param name="drawEnd">
        /// X = Tiles to draw from left to right.
        /// Y = Tiles to draw from top to bottom.</param>
        public void SetDrawViewEnd(Vector2 drawEnd)
        {
            if (drawEnd == null)
                return;

            if (m_DrawViewEnd == null)
                m_DrawViewEnd = new Vector2();

            m_DrawViewEnd.X = drawEnd.X;
            m_DrawViewEnd.Y = drawEnd.Y;
        }

        public Vector2 GetDrawViewEnd()
        {
            return m_DrawViewEnd;
        }
        /// <summary>
        /// Adds a tile a to the layer.
        /// The position on the layer is caluclated from
        /// the X,Y position specified.
        /// </summary>
        /// <param name="tileID">ID from the tileset.</param>
        /// <param name="position">Position in pixels where to place this tile.</param>
        public void AddTile(int tileID, Vector2 position)
        {
            if (position == null)
                return;

            int x = (int)(position.X);
            int y = (int)(position.Y);

            Vector2 newPosition = GetFinalPosition(position);
            MapTile tile = new MapTile(newPosition, tileID, m_vTileSize);
            
            tile.SetTileSet(m_tileSetRef);

            m_tile2DList[y][x] = tile;

        }
        /// <summary>
        /// Draws the layer in the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            
            for (int i = (int)GetDrawViewStart().Y; i < (int)GetMapSize().Y&& i < (int)GetDrawViewStart().Y + (int)GetDrawViewEnd().Y; i++)
            {
                for (int j = (int)GetDrawViewStart().X; j < (int)GetMapSize().X && j < (int)GetDrawViewStart().X + (int)GetDrawViewEnd().X; j++)
                {
                    m_tile2DList[i][j].DrawTile(spriteBatch, m_tileSetRef);
                }
            }
        }
        /// <summary>
        /// Draws the layer with a given offset.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="offset"></param>
        public void Draw(SpriteBatch spriteBatch, Vector2 offset)
        {
            for (int i = (int)GetDrawViewStart().Y; i < (int)GetMapSize().Y && i < (int)GetDrawViewStart().Y + (int)GetDrawViewEnd().Y; i++)
            {
                for (int j = (int)GetDrawViewStart().X; j < (int)GetMapSize().X && j < (int)GetDrawViewStart().X + (int)GetDrawViewEnd().X; j++)
                {
                    //m_tile2DList[i][j].SetTileOffset(finalOffset);
                    if(m_tile2DList[i][j] != null)
                        m_tile2DList[i][j].DrawTile(spriteBatch, m_tileSetRef, offset);
                }
            }
        }

        public Vector2 GetFinalPosition(Vector2 position)
        {
            int x = (int)(position.X);
            int y = (int)(position.Y);

            int finalX = 0, finalY = 0;

            finalX = (int)(x * GetTileSize().X);
            finalY = (int)(y * GetTileSize().Y);
            if (y % 2 == 1)
            {
                finalX += (int)(GetTileSize().X / 2);
            }
            if (y > 0)
                finalY -= (int)(GetTileSize().Y * 0.75) * y;

            Vector2 newPosition = new Vector2(finalX, finalY);

            return newPosition;
        }

        public Vector2 GetTilePosition(Vector2 tileIndex)
        {
            return m_tile2DList[(int)tileIndex.Y][(int)tileIndex.X].GetRealPosition();
        }
        
    }
}
