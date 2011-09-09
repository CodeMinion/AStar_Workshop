using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Entities;
using IsometricMap.Logic.Base;
namespace IsometricMap.Logic.States.Goblin.Lumberjack
{
    public class GLumberjack_TreeTravel:BaseState<GoblinLumberjack>
    {
        /// <summary>
        /// Instance field for the singleton design pattern.
        /// </summary>
        private static GLumberjack_TreeTravel m_Instance = null;

        /// <summary>
        /// Private constructor for the Singleton implementation.
        /// </summary>
        private GLumberjack_TreeTravel()
        {
            m_sStateName = "TREE_TRAVEL";
        }

        /// <summary>
        /// This method ensures that there is only one instance
        /// of this state at any given time. If there is no instance
        /// then one is created.
        /// </summary>
        /// <returns>single instance of this state.</returns>
        public static GLumberjack_TreeTravel GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GLumberjack_TreeTravel();

            return m_Instance;

        }

        public override void OnEnter(GoblinLumberjack owner)
        {
            // Move to the tree.
            owner.MoveDistanceInFrontOfEntity(owner.GetTargetTree(), 0);
        }

        public override void OnUpdate(GoblinLumberjack owner)
        {
            if (owner.GetTargetTree() == null)
            {
                //If there is no tree then go back to IDLE state
                owner.GetStateMachine().ChangeState(GLumberjack_IDLE.GetInstance());
                return; 
            }

            // If we are there, then proceed to chop tree.
            if (owner.IsAtDestination())
            {
                // Transition to Chop Wood State.
                owner.GetStateMachine().ChangeState(GLumberjack_ChopWood.GetInstance());
            }
        }

        public override void OnExit(GoblinLumberjack owner)
        {
            // No Need to clean up.
        }
    }
}
