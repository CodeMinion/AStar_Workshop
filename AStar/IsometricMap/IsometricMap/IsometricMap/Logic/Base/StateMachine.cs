using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IsometricMap.Logic.Base
{
    public class StateMachine<T>
    {
        // Owner of the machine.
        protected T m_Owner;

        // Previously visited state.
        protected BaseState<T> m_prevState;

        // Current active state.
        protected BaseState<T> m_currState;

        public StateMachine (T owner)
        {
            m_Owner = owner;
            m_prevState = null;
            m_currState = null;
        }

        /// <summary>
        /// This method causes the state machine to transition
        /// from one state to the next. It also saves the state 
        /// of the machine in case we wish to revert.
        /// </summary>
        /// <param name="newState">Next state to transition to.</param>
        public void ChangeState(BaseState<T> newState)
        {
            if (newState == null || m_Owner == null)
                return;

            if (m_currState != null)
            {
                // Perform any clean up that must be done.
                m_currState.OnExit(m_Owner);
            }
            // Save the current state in case we want to revert.
            m_prevState = m_currState;

            // Set the current state.
            m_currState = newState;

            // Perform any init for this state.
            m_currState.OnEnter(m_Owner);

        }
        /// <summary>
        /// This method reverts the machine to
        /// the previous state it was in. The change
        /// only happens if there is a previous state
        /// to revert to.
        /// </summary>
        public void RevertToPreviousState()
        {
            if (m_prevState == null || m_Owner == null)
                return;

            if (m_currState != null)
            {
                // Call any clean up needed.
                m_currState.OnExit(m_Owner);
            }
            m_currState = m_prevState;

            // Perform any init needed.
            m_currState.OnEnter(m_Owner);
        }

        /// <summary>
        /// This method updates the statemachine 
        /// by calling the OnUpdate of the current
        /// state. This allows the state logic to
        /// move forward on time step.
        /// </summary>
        public void Update()
        {
            if (m_currState != null)
            {
                m_currState.OnUpdate(m_Owner);
            }
        }

        public bool IsInState(BaseState<T> state)
        {
            if (m_currState == null)
                return false;

            return m_currState.Equals(state);
        }

        public BaseState<T> GetCurrentState()
        {
            return m_currState;
        }
    }
}
