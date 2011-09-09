using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using IsometricMap;
using IsometricMap.Sprites;

namespace IsometricMap.Entities
{
    public class LumberMill:GameEntity
    {
        #region Nothing to see here, move along.

        private int m_GatheredWood;

        public LumberMill(Vector2 position)
        {
            SetWorldPosition(position);
            SetDrawPosition(position);
            LoadAnimations("");
            m_SpriteSize.X = 64;
            m_SpriteSize.Y = 64;
            m_bEnabled = true;
            m_GatheredWood = 0;
        }
        /// <summary>
        /// This method returns the path to the
        /// image file for this mill.
        /// </summary>
        /// <returns></returns>
        public string GetResourceFile()
        {
            return "Map_TileSet/human-city";
        }
       
        protected override void UpdateEntity(GameTime time)
        {
        
        }

        protected override void DrawEntity(SpriteBatch spriteBatch)
        {
        
        }

        protected override void LoadAnimations(string animationFile)
        {
            Animation anim = new Animation(0 * 64, 0 * 64, 1, 64, 64, 100, true);
            AddAnimation(DIRECTION.UP, "IDLE", anim);
            SetCurrentAnimation(DIRECTION.UP, "IDLE");

            m_EntityCenter = new Vector2(0, 0);//(32, 16);
   
        }

        public bool IsClicked(Vector2 mousePosition)
        {
            if (mousePosition.X < m_DrawPosition.X)
                return false;

            if (mousePosition.X > m_DrawPosition.X + m_SpriteSize.X)
                return false;

            if (mousePosition.Y < (m_DrawPosition.Y))
                return false;

            if (mousePosition.Y > m_DrawPosition.Y + m_SpriteSize.Y)
                return false;

            return true;
        
        }
        public void DepositWood(int wood)
        {
            m_GatheredWood += wood;
        }
        #endregion 
    }
}
