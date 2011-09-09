using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IsometricMap.Map
{
    public class MapTile
    {
        // Actual coordinate in the map.
        Vector2 m_PositionReal;
        // Draw coordinate 
        Vector2 m_PositionDraw;
        // Size of the tile
        Vector2 m_TileSize;

        Texture2D m_tileSetRef;
        int m_TileID;

        Vector2 m_tileOffset;

        // Is this tile solid or can I move through it.
        public bool m_bIsSolid = false;
        
        // Tile color, used for debugging purposes mainly.
        public Color m_cTileColor = Color.White;

        public Rectangle m_SourceRect;

        /// <summary>
        /// Constructor for the MapTile.
        /// </summary>
        /// <param name="positionReal">position of the tile in space.</param>
        /// <param name="tileID">Id for the tile, this is usually the id on the spritesheet.</param>
        /// <param name="tileSize">Size in pixels of the tile, X = W and Y = H</param>
        public MapTile(Vector2 positionReal, int tileID, Vector2 tileSize)
        {
            SetRealPosition(positionReal);
            SetDrawPosition(positionReal);

            SetTileSize(tileSize);
            SetTileID(tileID);
            SetTileOffset(Vector2.Zero);

        }
        
        public void SetDrawPosition(Vector2 newPos)
        {
            if (m_PositionDraw == null)
                m_PositionDraw = new Vector2();

            m_PositionDraw.X = newPos.X;
            m_PositionDraw.Y = newPos.Y;
        }
        public Vector2 GetDrawPosition()
        {
            return Vector2.Add(m_tileOffset,  m_PositionDraw);
        }

        public void SetRealPosition(Vector2 newPos)
        {
            if (m_PositionReal == null)
                m_PositionReal = new Vector2();

            m_PositionReal.X = newPos.X;
            m_PositionReal.Y = newPos.Y;
        }
        public Vector2 GetRealPosition()
        {
            return m_PositionReal;
        }

        public void SetSourceRectangle(Rectangle rect)
        {
            if (m_SourceRect == null)
                m_SourceRect = new Rectangle();

            m_SourceRect.X = rect.X;
            m_SourceRect.Y = rect.Y;
            m_SourceRect.Width = rect.Width;
            m_SourceRect.Height = rect.Height;
        }
        public Rectangle GetSourceRectangle()
        {
            return m_SourceRect;
        }
        /// <summary>
        /// Set a reference to the image tileset.
        /// </summary>
        /// <param name="tileSet">Source Image</param>
        public void SetTileSet(Texture2D tileSet)
        {
            m_tileSetRef = tileSet;
            SetTileID(m_TileID);
        }

        public Texture2D GetTileSet()
        {
            return m_tileSetRef;
        }

        public int GetTileID()
        {
            return m_TileID;
        }
        /// <summary>
        /// Sets the size of the tile in pixels.
        /// </summary>
        /// <param name="size">X = W, Y = H </param>
        public void SetTileSize(Vector2 size)
        {
            if (m_TileSize == null)
                m_TileSize = new Vector2();

            if (size == null)
                return;

            m_TileSize.X = size.X;
            m_TileSize.Y = size.Y;
        }
        /// <summary>
        /// Sets the offset for a given tile.
        /// </summary>
        /// <param name="offSet"></param>
        public void SetTileOffset(Vector2 offSet)
        {
            if (m_tileOffset == null)
                m_tileOffset = new Vector2();

            if (offSet == null)
                return;


            m_tileOffset.X = offSet.X;
            m_tileOffset.Y = offSet.Y;

        }
        public Vector2 GetTileSize()
        {
            return m_TileSize;
        }
        public void SetTileID(int tileID)
        {
            m_TileID = tileID;
            if (m_tileSetRef != null)
            {
               SetSourceRectangle(MapHandler.GetInstance().GetSourceRectByID(GetTileID(), m_tileSetRef.Width,
                                          m_tileSetRef.Height, (int)GetTileSize().X, (int)GetTileSize().Y));
            }
        }
        public void SetTileColor(Color color)
        {
            m_cTileColor = color;
        }
        /// <summary>
        /// Used to update the logic for a tile, if any.
        /// </summary>
        /// <param name="time"></param>
        public void UpdateTile(GameTime time)
        {

            if (m_bIsSolid)
                m_cTileColor = Color.Red;
            //else
            //    m_cTileColor = Color.White;
            //else
            //    m_cTileColor = Color.White;
        }

        /// <summary>
        /// Sets the solid property of this tile
        /// either to true or false.
        /// </summary>
        /// <param name="isSolid"></param>
        public void SetTileSolid(bool isSolid)
        {
            m_bIsSolid = isSolid;
            if (m_bIsSolid)
                m_cTileColor = Color.Red;
            else if(m_cTileColor.Equals(Color.Red))
                m_cTileColor = Color.White;
        }

        public bool IsTileSolid()
        {
            return m_bIsSolid;
        }

        /// <summary>
        /// Draws a tile on the screen.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="tileTexture"></param>
        public void DrawTile(SpriteBatch spriteBatch, Texture2D tileTexture)
        {
            if(!Map.MapHandler.GetInstance().GetDebug())
                spriteBatch.Draw(tileTexture, GetDrawPosition(), GetSourceRectangle(), Color.White);    
            else
            spriteBatch.Draw(tileTexture, GetDrawPosition(), GetSourceRectangle(), m_cTileColor);    
        }

        /// <summary>
        /// Draws a tile on the screen with a given 
        /// offset.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="tileTexture"></param>
        /// <param name="offSet"></param>
        public void DrawTile(SpriteBatch spriteBatch, Texture2D tileTexture, Vector2 offSet)
        {
            if (!Map.MapHandler.GetInstance().GetDebug())
                spriteBatch.Draw(tileTexture, GetDrawPosition(), GetSourceRectangle(), Color.White);
            else
                spriteBatch.Draw(tileTexture, Vector2.Add(GetDrawPosition(), offSet), GetSourceRectangle(), m_cTileColor);
        }


        
        
           
    }
}
