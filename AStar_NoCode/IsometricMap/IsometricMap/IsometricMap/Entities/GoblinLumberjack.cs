using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using IsometricMap;
using IsometricMap.Entities;
using IsometricMap.Sprites;

using IsometricMap.Logic;
using IsometricMap.Logic.Base;
using IsometricMap.Logic.States.Goblin.Lumberjack;

using IsometricMap.Pathfinder;
using IsometricMap.Pathfinder.Base;

using IsometricMap.Map;


namespace IsometricMap.Entities
{
    /// <summary>
    /// Enumeration for the different 
    /// colors of this goblin.
    /// </summary>
    public enum ENTITY_COLOR 
    {
        BLACK,
        BLUE,
        CYAN,
        GREEN,
        ORANGE,
        PINK,
        PURPLE,
        RED,
        WHITE,
        YELLOW

    };

    /// <summary>
    /// Class representing a lumberjack gobling.
    /// </summary>
    public class GoblinLumberjack:GameEntity
    {
        #region Nothing to see here, move along.
        /// <summary>
        /// This contains all the sprite files for all
        /// the possible lumberjack goblin.
        /// </summary>
        List<string> m_ResourceFiles = new List<string>() 
        { 
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_black",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_blue",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_cyan",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_green",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_orange",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_pink",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_purple",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_red",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_white",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_yellow"
        };

        /// <summary>
        /// This holds the name for all the animations of this
        /// gobling.
        /// </summary>
        public List<string> m_AnimationIDs = new List<string>()
        {
            "IDLE",
            "MOVE",
            "CARRY_LOGS",
            "CHOP",
            "PUT_DOWN",
            "BLOCK",
            "DIE"
        };

        /// <summary>
        /// This holds the size of each animation 
        /// for this goblin.
        /// </summary>
        List<int> m_AnimationSizes = new List<int>()
        {
            4, // IDLE
            8, // MOVE
            8, // CARRY_LOGS
            6, // CHOP
            4, // PUT_DOWN
            2, // BLOCK
            6  // DIE 
        };

        ENTITY_COLOR m_myColor;
        int m_currLogCarried = 0;
        int m_maxLogCapacity = 10;
        int m_chopDelay = 6*150; // This is the amount of time it takes to obtain on log.
        double m_dNextChop = 0;

        int m_depositDelay = 150;
        double m_dNextDeposit = 0;
        bool m_bChoopingWood = false;
        bool m_bDepositingWood = false;

        SoundEffect m_chopSound;
        SoundEffectInstance m_ChopInstance;

        TreeEntity m_TargetTree;
        LumberMill m_TargetLumberMill;


        StateMachine<GoblinLumberjack> m_MyStateMachine;
#endregion
 


        // Add Reference to the path and the pathfinder 
        PathNode m_pnCurrPath;
        // Pathfinder used.
        PathFinder m_pfMyPathFinder;

        #region Nothing to see here, move along.
 
        public GoblinLumberjack(ENTITY_COLOR color)
        {
            m_myColor = color;
            m_SpriteSize = new Vector2(64, 64);
            LoadAnimations("");
            m_MyStateMachine = new StateMachine<GoblinLumberjack>(this);
            m_MyStateMachine.ChangeState(GLumberjack_IDLE.GetInstance());

#endregion

            // Initialize the Pathfindr
            
            #region Nothing to see here, Move along.

        }

        public GoblinLumberjack(ENTITY_COLOR color, Vector2 start_postion, DIRECTION dir):this(color)
        {
            m_WorldPosition = start_postion;
            m_DrawPosition = start_postion;
            m_DestinationPosition = start_postion;
            SetCurrentAnimation(dir, "IDLE");
        }

        /// <summary>
        /// This returns the path to the sprite file for
        /// this goblin.
        /// </summary>
        /// <returns></returns>
        public string GetResroucePath()
        {
            return m_ResourceFiles[(int)m_myColor];
        }

        /// <summary>
        /// This returns the path to the sound
        /// rsource file. 
        /// </summary>
        /// <returns></returns>
        public string GetSoundResourcePath()
        {
            return "SoundEffects/Goblin/Lumberjack/wood_chop";
        }

        /// <summary>
        /// This sets the sound effect for the wood chop.
        /// </summary>
        /// <param name="effect"></param>
        public void SetSoundEffect(SoundEffect effect)
        {
            m_chopSound = effect;
            m_ChopInstance = m_chopSound.CreateInstance();
        }

        protected override void DrawEntity(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Handles the logic for the goblin.
        /// </summary>
        /// <param name="time"></param>
        protected override void UpdateEntity(GameTime time)
        {
            // if the goblin is moving an carrying logs then 
            // we play the carry animation, otherwise we play 
            // the default animation.
            if (IsEntityMoving())
            {
                if (m_currLogCarried > 0)
                    SetCurrentAnimation(m_EntityDircetion, m_AnimationIDs[2]);
                else
                    SetCurrentAnimation(m_EntityDircetion, m_AnimationIDs[1]);
            }
        
            // If we are chopping wood then play the wood_chop sound.
            if(m_currentAnimation.Equals(m_Animations[m_EntityDircetion][m_AnimationIDs[3]]))
            {
                PlayChopSound();
            }
            // only gather if we have waited enough.
            if (m_dNextChop < time.TotalGameTime.TotalMilliseconds)
            {
                GatherWood();
                m_dNextChop = time.TotalGameTime.TotalMilliseconds + m_chopDelay;
            }
            // only deposit wood if we have waited enough.
            if (m_dNextDeposit < time.TotalGameTime.TotalMilliseconds)
            {
                RemoveWood();
                m_dNextDeposit = time.TotalGameTime.TotalMilliseconds + m_chopDelay;
            }
            
            // Move iF possible
            Move();
            // Update statemachine.
            m_MyStateMachine.Update();
        }

        protected override void LoadAnimations(string animationFile)
        {
            String animationID = "";
            Animation anim = null;
            DIRECTION dir = DIRECTION.UP;
            int frameDelay = 150;
            int spriteWidth = 64;
            int spriteHeight = 64;

            int animCount =0;
            for(int i = 0; i < m_AnimationSizes.Count; i ++)
            {
                animationID = m_AnimationIDs[i];
                if(i <=0)
                    animCount = 0;
                else
                    animCount += m_AnimationSizes[i-1];

                for(int j =0;  j< 8 ; j++)
                {
                    anim = new Animation(animCount * spriteWidth, j * spriteHeight, m_AnimationSizes[i], spriteWidth, spriteHeight, frameDelay, true);
                    dir = (DIRECTION)j;
                    AddAnimation(dir, animationID, anim);
            
                }
            }
            SetCurrentAnimation(dir, animationID);

        }

        /// <summary>
        /// This method plays the sound of wood chopping.
        /// Not implemented yet.
        /// </summary>
        public void PlayChopSound()
        {
            //if (m_ChopInstance.State == SoundState.Stopped)
            //    m_ChopInstance.Play();
        }

        /// <summary>
        /// If the entity to go to is a tree then we set it as our target tree.
        /// If the rntity to go to is a lumber mill then we set it as our target
        /// mill.
        /// 
        /// Otherwise it delegates to the parent's implementation.
        /// </summary>
        /// <param name="toEntity"></param>
        /// <param name="distance"></param>
        public override void MoveDistanceInFrontOfEntity(GameEntity toEntity, int distance)
        {
            // If the target is a tree then save it
            // as a target.
            if (toEntity is TreeEntity)
            {
                m_TargetTree = (TreeEntity)toEntity;
                if(!m_MyStateMachine.IsInState(GLumberjack_TreeTravel.GetInstance()))
                    m_MyStateMachine.ChangeState(GLumberjack_TreeTravel.GetInstance());
            }
            // If it is a lumbermill then save it 
            // as a target.
            else if (toEntity is LumberMill)
            {
                m_TargetLumberMill = (LumberMill)toEntity;
                if(!m_MyStateMachine.IsInState(GLumberjack_MillTravel.GetInstance()))
                    m_MyStateMachine.ChangeState(GLumberjack_MillTravel.GetInstance());
            }

            
            // Perform the normal logic.
            base.MoveDistanceInFrontOfEntity(toEntity, distance);
            Vector2 tempOut;
            if (FindPathToDestination(m_WorldPosition, m_DestinationPosition, out tempOut))
            {
                m_DestinationPosition = tempOut;
            }
            //Vector2 startIndex = Map.MapHandler.GetInstance().GetTileIndex(m_WorldPosition);
            //Vector2 endIndex = Map.MapHandler.GetInstance().GetTileIndex(m_DestinationPosition);
            //// Call path finder to find path from the startIndex to the endIndex.
            //m_pnCurrPath = m_pfMyPathFinder.FindPath(startIndex, endIndex);
            //// If a pass was found then set the first node in the path 
            //// as the first destination.
            //if (m_pnCurrPath != null)
            //{
            //    m_DestinationPosition = Map.MapHandler.GetInstance().GetTilePosition(m_pnCurrPath.GetNodePosition());
            //}
           
        }

        /// <summary>
        /// Sets both target mill and tree to null.
        /// Then it refers to the parent.
        /// </summary>
        /// <param name="targetPos"></param>
        public override void MoveTo(Vector2 targetPos)
        {
            m_TargetLumberMill = null;
            m_TargetEntity = null;

            if (FindPathToDestination(m_WorldPosition, targetPos, out targetPos))
            {
                base.MoveTo(targetPos);
            }
            //Vector2 startIndex = Map.MapHandler.GetInstance().GetTileIndex(m_WorldPosition);
            //Vector2 endIndex = Map.MapHandler.GetInstance().GetTileIndex(targetPos);
            
            //// Call Pathfinder find a path from the startIndex to the end index.
            //m_pnCurrPath = m_pfMyPathFinder.FindPath(startIndex, endIndex);

            //// If a path was found, then make the first node in the 
            //// path the first destination of this entity.
            //if (m_pnCurrPath != null)
            //{
            //    targetPos = Map.MapHandler.GetInstance().GetTilePosition(m_pnCurrPath.GetNodePosition());

            //    base.MoveTo(targetPos);
            //}
        }

            #endregion

        
        /// <summary>
        /// This method attempts to plot a path between the startPosition and the endPosition.
        /// If a path is found,  the method returns true and the destPos contains the position
        /// of the first node in the path list.
        /// </summary>
        /// <param name="startPosition">X,Y coordinates of the start position in pixels.</param>
        /// <param name="endPosition">X,Y coordinates of the end position in pixels.</param>
        /// <param name="destPos">X,Y coordinates of the first path node position in pixels, if one is found.</param>
        /// <returns>True if a path is found.</returns>
        public bool FindPathToDestination(Vector2 startPosition, Vector2 endPosition, out Vector2 destPos)
        {
            Vector2 startIndex = Map.MapHandler.GetInstance().GetTileIndex(startPosition);
            Vector2 endIndex = Map.MapHandler.GetInstance().GetTileIndex(endPosition);

            Vector2 closestSoFar = endIndex;
            // Check if the destination is solid
            bool isDestSolid = MapHandler.GetInstance().IsTileSolid(endIndex);
            if (isDestSolid)
            {
                List<Vector2> adj = MapHandler.GetInstance().GetAdjacentsToTile(endIndex);
                closestSoFar = adj[0];
                foreach (Vector2 vec in adj)
                {
                    if (!MapHandler.GetInstance().IsTileSolid(vec))
                    {
                        // Find the closest adjecent to the caller.
                        if (Vector2.Distance(closestSoFar, startIndex) > Vector2.Distance(vec, startIndex))
                        {
                            closestSoFar = vec;
                        }
                        //endIndex = vec;
                        //break;
                    }
                }
            }
            endIndex = closestSoFar;
            // Call Pathfinder find a path from the startIndex to the end index.
            
            // If a path was found, then make the first node in the 
            // path the first destination of this entity.
            
            destPos = Vector2.Zero;
            return false ;
        }
        #region Nothing to see here, Move along.

        public void UpdateMoveTo(Vector2 targetPos)
        {
            base.MoveTo(targetPos);
        }
        public TreeEntity GetTargetTree()
        {
            return m_TargetTree;
        }
        public void SetTargetTree(TreeEntity tree)
        {
            m_TargetTree = tree;
        }
        public LumberMill GetTargetLumberMill()
        {
            return m_TargetLumberMill;
        }
        public void SetTargetLumberMill(LumberMill mill)
        {
            m_TargetLumberMill = mill;
        }
        public StateMachine<GoblinLumberjack> GetStateMachine()
        {
            return m_MyStateMachine;
        }
        /// <summary>
        /// This method checks if there is more to the path 
        /// than we have walked so far. If there is more
        /// then it sets the next node for the entity's
        /// destination.
        /// </summary>
        public void Move()
        {
#endregion

            // If there is no path then dont move.

            if (m_pnCurrPath == null)
                return;

            #region Nothing to see here, move along.

            //Vector2 entPosIndex = MapHandler.GetInstance().GetTileIndex(m_WorldPosition);
            //Vector2 currNodePos = MapHandler.GetInstance().GetTileIndex(m_DestinationPosition);

            if (!Vector2.Equals(m_WorldPosition, m_DestinationPosition))//entPosIndex, currNodePos))
                return;

            SetCurrentPath(m_pnCurrPath.GetNextNode());
            if (m_pnCurrPath != null)
            {
                UpdateMoveTo(MapHandler.GetInstance().GetTilePosition(m_pnCurrPath.GetNodePosition()));
            }
                    
        }

        /// <summary>
        /// Sets the goblin ready to chop wood.
        /// </summary>
        public void ChopWood()
        {
            if (m_TargetTree == null)
                return;

            m_bChoopingWood = true;
         
        }
        public void StopChoppingWood()
        {
            m_bChoopingWood = false;
        }
        /// <summary>
        /// Gets the actual wood.
        /// </summary>
        private void GatherWood()
        {
            if (m_bChoopingWood)
            {
                if (m_currLogCarried < 0)
                    m_currLogCarried = 0;

                if (m_TargetTree == null)
                    return; 
                // Chop for as long as you can carry.
                if (m_currLogCarried < m_maxLogCapacity)
                    m_currLogCarried += m_TargetTree.GetWood();

            }  
        }
        public bool IsDoneChoppingWood()
        {
            if (m_maxLogCapacity <= m_currLogCarried || m_TargetTree == null || !m_TargetTree.IsEnabled())
            {
                m_bChoopingWood = false;

                if (!m_TargetTree.IsEnabled())
                    m_TargetTree = null;

                return true;
            }
            return false;
        }

        public void DepositWood()
        {
            if (m_TargetLumberMill == null)
                return;

            m_bDepositingWood = true;

        }
        public void StopDepositingWood()
        {
            m_bDepositingWood = false;
        }
        private void RemoveWood()
        {
            if (m_bDepositingWood)
            {
                // Drop for as long as you carry logs.
                if (m_currLogCarried > 0)
                    m_currLogCarried -= 9;
            }
        }
        public bool IsDoneDropingWood()
        {
            if (m_currLogCarried <= 0 || m_TargetLumberMill == null || !m_TargetLumberMill.IsEnabled())
            {
                if (m_currLogCarried < 0)
                    m_currLogCarried = 0;

                m_bDepositingWood = false;

                return true;
            }
            return false;
        }
        public int GetCarriedWood()
        {
            return m_currLogCarried;
        }
        public int GetTargetTreeWoodLeft()
        {
            if (m_TargetTree == null)
                return -1;

            return m_TargetTree.GetWoodCount();
        }
        public int GetLogMaxCapacity()
        {
            return m_maxLogCapacity;
        }
        public PathNode GetCurrentPath()
        {
            return m_pnCurrPath;
        }
        public void SetCurrentPath(PathNode path)
        {
            m_pnCurrPath = path;
        }
        public override bool IsAtDestination()
        {
            return m_pnCurrPath == null && base.IsAtDestination();
        }
        #endregion

    }
}
