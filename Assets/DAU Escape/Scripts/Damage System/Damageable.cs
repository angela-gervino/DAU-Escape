using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAUEscape
{
    public partial class Damageable : MonoBehaviour
    {
        public float invulnerabilityTime = 1.0f; // once damaged, cannot be damaged again for this number of seconds
        public int maxHP;
        public int currentHP { get; private set; }
        public List<MonoBehaviour> onDamageMessageReceivers;

        private bool isInvulnerable;
        private float timeSinceLastDamaged = 0.0f;

        private void Awake()
        {
            currentHP = maxHP;
        }// Awake


        private void Update()
        {
            if (isInvulnerable)
            {
                timeSinceLastDamaged += Time.deltaTime;

                if (timeSinceLastDamaged >= invulnerabilityTime)
                {
                    isInvulnerable = false;
                    timeSinceLastDamaged = 0;
                }
            }
        }// Update


        public void ApplyDamage(DamageMessage data)
        {
            if (currentHP > 0 && !isInvulnerable)
            {
                isInvulnerable = true;
                currentHP -= data.amount;

                var messageType = currentHP <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

                for (int i = 0; i < onDamageMessageReceivers.Count; i++)
                {
                    var receiver = onDamageMessageReceivers[i] as IMessageReceiver; // IMessageReceiver has the OnReceiveMessage method, MonoBehaviour doesn't always
                    receiver.OnReceiveMessage(messageType);
                }
            }
        }// ApplyDamage
    }
}

