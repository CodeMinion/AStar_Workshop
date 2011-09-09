using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IsometricMap.Entities;
using IsometricMap.Logic.Base;

namespace IsometricMap.Logic.States.Goblin.Lumberjack
{
    public class GLumberjack_MillTravel:BaseState<GoblinLumberjack>
    {
        /// <summary>
        /// Instance field for the singleton design pattern.
        /// </summary>
        private static GLumberjack_MillTravel m_Instance = null;

        /// <summary>
        /// Private constructor for the Singleton implementation.
        /// </summary>
        private GLumberjack_MillTravel()
        {
            m_sStateName = "MILL_TRAVEL";
        }

        /// <summary>
        /// This method ensures that there is only one instance
        /// of this state at any given time. If there is no instance
        /// then one is created.
        /// </summary>
        /// <returns>single instance of this state.</returns>
        public static GLumberjack_MillTravel GetInstance()
        {
            if (m_Instance == null)
                m_Instance = new GLumberjack_MillTravel();

            return m_Instance;
        }



        public override void OnEnter(GoblinLumberjack owner)
        {
            // Give the goblin the movement animation.
            owner.SetCurrentAnimation("MOVE");
            // Move to Lumbermill
            owner.MoveDistanceInFrontOfEntity(owner.GetTargetLumberMill(), 0);
            
        }

        public override void OnUpdate(GoblinLumberjack owner)
        {
            if (owner.GetTargetLumberMill() == null)
            {
                // if there is no mill the move to the IDLE State
                owner.GetStateMachine().ChangeState(GLumberjack_IDLE.GetInstance());
                return;
            }

            if (owner.IsAtDestination())
            {
                //If we arrived at the mill, go to deposit state.
                owner.GetStateMachine().ChangeState(GLumberjack_DepositWood.GetInstance());

            }
        }

        public override void OnExit(GoblinLumberjack owner)
        {
            //throw new NotImplementedException();
        }
    }
}
