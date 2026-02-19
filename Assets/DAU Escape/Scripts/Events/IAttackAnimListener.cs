using System;

namespace DAUEscape
{
    public interface IAttackAnimListener
    {
        // methods that must be implemented for animation events
        public void MeleeAttackStart();
        public void MeleeAttackEnd();
    }
}