using System;
using UnityEngine;

namespace T0xanGames.UnitsManagement
{
    public class PlayerComponent : MonoBehaviour
    {
        [SerializeField]
        private PlayerBase m_Base;

        public PlayerBase Base => m_Base;

        private void OnEnable()
        {
            if (m_Base == null)
                throw new NullReferenceException();

            m_Base.AddComponent(this);

            Enabled();
        }
        protected virtual void Enabled()
        {

        }

        private void OnDisable()
        {
            if (m_Base != null)
                m_Base.RemoveComponent(this);

            Disabled();
        }
        protected virtual void Disabled()
        {

        }
    }
}
