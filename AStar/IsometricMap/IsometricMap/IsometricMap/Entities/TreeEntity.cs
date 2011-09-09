using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using IsometricMap.Sprites;
namespace IsometricMap.Entities
{
    public class TreeEntity:GameEntity
    {
        #region Nothing to see here, move along.

        //123
        //133

        //Tree 1 IDs
        // 130
        
        //Tree 2 IDs
        // 131

        //Tree 3 IDs
        // 132
        //Tree 4 IDs
        // 133

        //Tree 5 IDs

        //Tree 6 IDs


        private int m_TreeWidth = 64;
        private int m_TreeHeight = 128;

        private int m_WoodQuantity = 20;
        
        public TreeEntity(Vector2 position)
        {
            SetDrawPosition(position);
            SetWorldPosition(position);
            SetEntitySpeed(Vector2.Zero);
            LoadAnimations("");
            
        }
        /// <summary>
        /// This method returns the location for the 
        /// tree.
        /// </summary>
        /// <returns></returns>
        public string GetResourceFile()
        {
            return "Map_TileSet/iso-64x64-outside";
        }
       
        /// <summary>
        /// We do no update for the tree entity, yet...
        /// </summary>
        /// <param name="time"></param>
        protected override void UpdateEntity(GameTime time)
        {
        
        }

        /// <summary>
        /// We do no extra drawing for the tree entity.
        /// </summary>
        /// <param name="spriteBatch"></param>
        protected override void DrawEntity(SpriteBatch spriteBatch)
        {
     
        }

        /// <summary>
        /// This loads all the animations for the tree.
        /// Currently a tree only has one animation of 1 frame.
        /// </summary>
        /// <param name="animationFile">Not used for this implementation.</param>
        protected override void LoadAnimations(string animationFile)
        {
            Animation anim = new Animation(3 * 64, 12 * 64, 1, 64, 128, 100, true);
            AddAnimation(DIRECTION.UP, "IDLE", anim);
            SetCurrentAnimation(DIRECTION.UP, "IDLE");

            m_EntityCenter = new Vector2(32, 64);
        }
        /// <summary>
        /// This method checks if the mouse position falls
        /// inside the tree's clickable area.
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        public bool IsTreeClicked(Vector2 mousePosition)
        {
            if (mousePosition.X < m_DrawPosition.X)
                return false;

            if (mousePosition.X > m_DrawPosition.X + m_TreeWidth)
                return false;

            if (mousePosition.Y < (m_DrawPosition.Y + 48))
                return false;

            if (mousePosition.Y > m_DrawPosition.Y + m_TreeHeight)
                return false;

            return true;
        }

        /// <summary>
        /// This method removes a bit of the tree's wood
        /// and returns it to the caller. 
        /// </summary>
        /// <returns></returns>
        public int GetWood()
        {
            int giveAmount = 0;
            if (m_WoodQuantity > 0)
            {
                m_WoodQuantity--;
                giveAmount = 1;
            }
            if (m_WoodQuantity <= 0)
            {
                m_bEnabled = false;

                Map.MapHandler.GetInstance().SetTileSolid(Map.MapHandler.GetInstance().GetTileIndex(
                                                          Vector2.Add(m_WorldPosition, m_EntityCenter)), 
                                                          false);
            }
            return giveAmount;
        }
        /// <summary>
        /// This method returns the current amount 
        /// of wood left on the tree.
        /// </summary>
        /// <returns></returns>
        public int GetWoodCount()
        {
            return m_WoodQuantity;
        }
        #endregion
    }
}
