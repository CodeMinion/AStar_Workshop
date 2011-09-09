using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


using IsometricMap.Sprites;

namespace IsometricMap
{
    public enum DIRECTION
    { 
        UP,
        UP_RIGHT,
        RIGHT,
        DOWN_RIGHT,
        DOWN,
        DOWN_LEFT,
        LEFT, 
        UP_LEFT,
        NO_DIR
        
    };
}
namespace IsometricMap.Entities
{
    public abstract class GameEntity
    {

        #region Nothing to see here, move along.

        protected Dictionary<DIRECTION, Dictionary<String, Animation>> m_Animations;
        protected Animation m_currentAnimation;

        protected Texture2D m_spriteTexture;
        protected Vector2 m_WorldPosition;
        protected Vector2 m_DrawPosition;
        protected Vector2 m_DestinationPosition;
        protected Vector2 m_EntitySpeed;
        protected Vector2 m_SpriteSize;
        protected Vector2 m_MovementDelta = new Vector2();
        protected DIRECTION m_EntityDircetion;
        protected string m_EntityAnimName;
        protected Vector2 m_EntityCenter;
        protected bool m_bEnabled = true;

        protected GameEntity m_TargetEntity;

        protected abstract void UpdateEntity(GameTime time);
        protected abstract void DrawEntity(SpriteBatch spriteBatch);
        protected abstract void LoadAnimations(String animationFile);

        
        /// <summary>
        /// This methods adds an animation to the animation list.
        /// Animations are stored in a dual value map where the first
        /// key represents the direction the animation is going and 
        /// the second is the name of ID of the animation.
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="animID"></param>
        /// <param name="anim"></param>
        public void AddAnimation(DIRECTION dir, String animID, Animation anim)
        {
            if (anim == null)
                return;

            if (m_Animations == null)
                m_Animations = new Dictionary<DIRECTION, Dictionary<string, Animation>>();

            //if (m_currentAnimation == null)
            //    return;

            // Get the animation list for this direction.
            Dictionary<String, Animation> animsDict = null;

            if (m_Animations.ContainsKey(dir))
                animsDict = m_Animations[dir];
            
            if (animsDict == null)
                animsDict = new Dictionary<string, Animation>();

            if (!animsDict.ContainsKey(animID))
            {
                // If there is no animation for that key, then add it.
                animsDict.Add(animID, anim);
            }
            else
            {
                // Replace the animation at that id with the new animation.
                animsDict[animID] = anim;
            }
            if (m_Animations.ContainsKey(dir))
                m_Animations[dir] = animsDict;
            else
                m_Animations.Add(dir, animsDict);
 
        }
        /// <summary>
        /// This method sets the currently playing animation to the
        /// animation passed as a parameter. 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="animationID"></param>
        public void SetCurrentAnimation(DIRECTION dir, string animationID)
        {
            m_EntityAnimName = animationID;
            m_EntityDircetion = dir;
            m_currentAnimation = m_Animations[dir][animationID];
        }

        /// <summary>
        /// This method searches the animation map for the given
        /// animation ID. If the animation is found the current animation
        /// is replaced.
        /// </summary>
        /// <param name="animationID"></param>
        public void SetCurrentAnimation(string animationID)
        {
            if (!m_Animations[m_EntityDircetion].ContainsKey(animationID))
                return;

            m_EntityAnimName = animationID;
            m_currentAnimation = m_Animations[m_EntityDircetion][animationID];
        }

        /// <summary>
        /// Update logic for each game entity is handled here.
        /// </summary>
        /// <param name="time"></param>
        public virtual void Update(GameTime time)
        {

            if (!m_bEnabled)
                return;

            // If we are moving then continue to modify the 
            // world and draw positions of the entity.
            if (!IsAtDestination())
            {
                // Everytime adjust where we are facing just in case
                // we have changed our movement direction.
                AdjustHeading();

                Vector2 tempWorld = m_WorldPosition;
               
                m_WorldPosition.X += m_MovementDelta.X * m_EntitySpeed.X;
                m_WorldPosition.Y += m_MovementDelta.Y * m_EntitySpeed.X;

                // If we try to cross over a solid tile, then we can't move.
                // Otherwise, go about as normal.
                //if (Map.MapHandler.GetInstance().IsTileSolid(Map.MapHandler.GetInstance().GetTileIndex(m_WorldPosition)))
                {
                //    m_WorldPosition = tempWorld;
                }
                //else
                {
                    m_DrawPosition.X += m_MovementDelta.X * m_EntitySpeed.X;
                    m_DrawPosition.Y += m_MovementDelta.Y * m_EntitySpeed.Y;

                    m_EntityCenter.X += m_MovementDelta.X * m_EntitySpeed.X;
                    m_EntityCenter.Y += m_MovementDelta.Y * m_EntitySpeed.Y;
                }

                //Change Direction of current animation
                SetCurrentAnimation(m_EntityDircetion, m_EntityAnimName);
            }
            else
            {
                FaceEntity(m_TargetEntity);
                SetCurrentAnimation(m_EntityDircetion, m_EntityAnimName);

            }
            // Simple template method pattern implementation.
            UpdateAnimation(time);
            UpdateEntity(time);
        }
        /// <summary>
        /// Entities are drawn as long as they are enabled.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Simple implementation ofthe template method pattern.
            if (!m_bEnabled)
                return; 

            DrawAnimation(spriteBatch);
            DrawEntity(spriteBatch);

        }
        /// <summary>
        /// This method makes the current animation move
        /// forward. The actual logic for moving to the next
        /// animation frame is handled by the animation itself.
        /// </summary>
        /// <param name="time"></param>
        private void UpdateAnimation(GameTime time)
        {
            if (m_currentAnimation == null)
                return;

            m_currentAnimation.Update(time.TotalGameTime.TotalMilliseconds);
        }
        /// <summary>
        /// This method draws current frame for the animation.
        /// </summary>
        /// <param name="spriteBtach"></param>
        private void DrawAnimation(SpriteBatch spriteBtach)
        {
            if (m_currentAnimation == null)
                return;

            if (m_DrawPosition == null)
                return;

            Rectangle sourceRect = m_currentAnimation.GetCurrentFrameSource();

            spriteBtach.Draw(m_spriteTexture, m_DrawPosition, sourceRect, Color.White); 
            
        }

        public void SetWorldPosition(Vector2 pos)
        {
            if (pos == null)
                return;

            if (m_WorldPosition == null)
                m_WorldPosition = new Vector2();

            m_WorldPosition.X = pos.X;
            m_WorldPosition.Y = pos.Y;
        }
        public Vector2 GetWorldPosition()
        {
            return m_WorldPosition;
        }
        public Vector2 GetCenterPosition()
        {
            return Vector2.Add(m_DrawPosition, m_EntityCenter);
        }

        public void SetDrawPosition(Vector2 pos)
        {
            if (pos == null)
                return;

            if (m_DrawPosition == null)
                m_DrawPosition = new Vector2();

            m_DrawPosition.X = pos.X;
            m_DrawPosition.Y = pos.Y;
        }

        public void SetSpriteTexture(Texture2D text)
        {
            m_spriteTexture = text;
        }

        public void SetEntitySpeed(Vector2 speed)
        {
            if (speed == null)
                return;

            if (m_EntitySpeed == null)
                m_EntitySpeed = new Vector2();

            m_EntitySpeed.X = speed.X;
            m_EntitySpeed.Y = speed.Y;
        }

        /// <summary>
        /// This method sets the destination position
        /// for future movment. It also adjust the 
        /// initial heading.
        /// </summary>
        /// <param name="targetPos"></param>
        public virtual void MoveTo(Vector2 targetPos)
        {
            if (targetPos == null)
                return;

            m_DestinationPosition = targetPos;
            AdjustHeading();
            
        }
        /// <summary>
        /// This method causes an entity to move a certain distance away from the 
        /// center of the toEntity.
        /// </summary>
        /// <param name="toEntity">Entity to move to.</param>
        /// <param name="distance">Distance to move away.</param>
        public virtual void MoveDistanceInFrontOfEntity(GameEntity toEntity, int distance)
        {
            if (toEntity == null)
                return; 
            Vector2 targetPos = Vector2.Add(toEntity.m_WorldPosition, toEntity.m_EntityCenter);
            DIRECTION movementDir;
            FaceEntity(toEntity);

            m_MovementDelta.X = 0;
            m_MovementDelta.Y = 0;
            if (m_WorldPosition.Y < targetPos.Y)
            {
                movementDir = DIRECTION.DOWN;
                m_MovementDelta.X = 0;
                m_MovementDelta.Y = 1;

                targetPos.Y -= distance;
                //targetPos.Y -= m_SpriteSize.Y / 2;
                
                if (m_WorldPosition.X < targetPos.X)
                {
                    movementDir = DIRECTION.DOWN_RIGHT;
                    m_MovementDelta.X = 1;
                    targetPos.X -= distance;
                    //targetPos.X -= m_SpriteSize.X;
                }
                else if (m_WorldPosition.X > targetPos.X)
                {
                    movementDir = DIRECTION.DOWN_LEFT;
                    m_MovementDelta.X = -1;
                    targetPos.X += distance;
                    //targetPos.X += m_SpriteSize.X / 8;
                }

            }
            else if (m_WorldPosition.Y == targetPos.Y)
            {
                movementDir = m_EntityDircetion;
                m_MovementDelta.X = 0;
                m_MovementDelta.Y = 0;


                if (m_WorldPosition.X < targetPos.X)
                {
                    movementDir = DIRECTION.RIGHT;
                    m_MovementDelta.X = 1;
                    targetPos.X -= distance;
                    //targetPos.X -= m_SpriteSize.X;
                
                }
                else if (m_WorldPosition.X > targetPos.X)
                {
                    movementDir = DIRECTION.LEFT;
                    m_MovementDelta.X = -1;
                    targetPos.X += distance;
                    //targetPos.X += m_SpriteSize.X / 8;
                
                }
            }
            else
            {
                movementDir = DIRECTION.UP;

                m_MovementDelta.X = 0;
                m_MovementDelta.Y = -1;
                targetPos.Y += distance;
                //targetPos.Y += m_SpriteSize.Y / 2;
                
                if (m_WorldPosition.X < targetPos.X)
                {
                    movementDir = DIRECTION.UP_RIGHT;
                    m_MovementDelta.X = 1;
                    targetPos.X -= distance;
                    //targetPos.X -= m_SpriteSize.X;
                }
                else if (m_WorldPosition.X > targetPos.X)
                {
                    movementDir = DIRECTION.UP_LEFT;
                    m_MovementDelta.X = -1;
                    targetPos.X += distance;
                    //targetPos.X += m_SpriteSize.X / 8;
                }
            }
            m_EntityDircetion = movementDir;
            m_DestinationPosition = targetPos;
            
        }

        /// <summary>
        /// This method changes the direction of the entity
        /// to face the desired entity.
        /// </summary>
        /// <param name="entity"></param>
        public void FaceEntity(GameEntity entity)
        {
            if (entity == null)
                return;

            DIRECTION movementDir = DIRECTION.UP;
            Vector2 targetPos = Vector2.Add( entity.m_WorldPosition, entity.m_EntityCenter);

            m_TargetEntity = entity;


            if (m_WorldPosition.Y < targetPos.Y)
            {
                movementDir = DIRECTION.DOWN;
            }
            else if (m_WorldPosition.Y > targetPos.Y)
            {
                movementDir = DIRECTION.UP;
            }
            else
            {
                
                if (m_WorldPosition.X < entity.m_WorldPosition.X)//targetPos.X)
                {
                    if (movementDir == DIRECTION.DOWN)
                        movementDir = DIRECTION.DOWN_RIGHT;
                    else if (movementDir == DIRECTION.UP)
                        movementDir = DIRECTION.UP_RIGHT;
                    else
                        movementDir = DIRECTION.RIGHT;
                }
                else if (m_WorldPosition.X > entity.m_WorldPosition.X)//targetPos.X)
                {
                    if (movementDir == DIRECTION.DOWN)
                        movementDir = DIRECTION.DOWN_LEFT;
                    else if (movementDir == DIRECTION.UP)
                        movementDir = DIRECTION.UP_LEFT;
                    else
                        movementDir = DIRECTION.LEFT;
                }
                else
                    movementDir = m_EntityDircetion;
            }

            m_EntityDircetion = movementDir;
            
        }
        protected void AdjustHeading()
        {
            DIRECTION movementDir = DIRECTION.UP;
            Vector2 targetPos = m_DestinationPosition;

            m_MovementDelta.X = 0;
            m_MovementDelta.Y = 0;
            if (m_WorldPosition.Y < targetPos.Y)
            {
                movementDir = DIRECTION.DOWN;
                m_MovementDelta.X = 0;
                m_MovementDelta.Y = 1;

                if (m_WorldPosition.X < targetPos.X)
                {
                    movementDir = DIRECTION.DOWN_RIGHT;
                    m_MovementDelta.X = 1;
               
                }
                else if (m_WorldPosition.X > targetPos.X)
                {
                    movementDir = DIRECTION.DOWN_LEFT;
                    m_MovementDelta.X = -1;
                }

            }
            else if (m_WorldPosition.Y == targetPos.Y)
            {
                movementDir = m_EntityDircetion;
                m_MovementDelta.X = 0;
                m_MovementDelta.Y = 0;

                if (m_WorldPosition.X < targetPos.X)
                {
                    movementDir = DIRECTION.RIGHT;
                    m_MovementDelta.X = 1;

                }
                else if (m_WorldPosition.X > targetPos.X)
                {
                    movementDir = DIRECTION.LEFT;
                    m_MovementDelta.X = -1;
                }
            }
            else
            {
                movementDir = DIRECTION.UP;

                m_MovementDelta.X = 0;
                m_MovementDelta.Y = -1;
                if (m_WorldPosition.X < targetPos.X)
                {
                    movementDir = DIRECTION.UP_RIGHT;
                    m_MovementDelta.X = 1;
                }
                else if (m_WorldPosition.X > targetPos.X)
                {
                    movementDir = DIRECTION.UP_LEFT;
                    m_MovementDelta.X = -1;
                }
            }
            m_EntityDircetion = movementDir;
            
        }

        /// <summary>
        /// This method checks if the entity is at the 
        /// target detination. 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsAtDestination()
        {
            if(Vector2.Equals(m_MovementDelta, Vector2.Zero))
                return true;

            return false;
        }

        /// <summary>
        /// This method checks if the entity is still
        /// in motion.
        /// </summary>
        /// <returns></returns>
        public bool IsEntityMoving()
        {
            if (Vector2.Equals(m_MovementDelta, Vector2.Zero))
                return false;

            return true;

        }

        public bool IsEnabled()
        {
            return m_bEnabled;
        }

        #endregion
    }
}
