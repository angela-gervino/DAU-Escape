using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DAUEscape
{
    public partial class Damageable : MonoBehaviour
    {
        public float invulnerabilityTime = 1.0f; // once damaged, cannot be damaged again for this number of seconds
        public int maxHP;
        public int currentHP { get; private set; }
        public List<MonoBehaviour> onDamageMessageReceivers;
        public Slider healthBar;

        private bool isInvulnerable;
        private float timeSinceLastDamaged = 0.0f;

        private void Awake()
        {
            currentHP = maxHP;
        }// Awake


        private void Update()
        {
            UpdateHealthBar(); // Update UI

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


        private void UpdateHealthBar()
        {
            if (healthBar != null)
            {
                float hpRatio = (float)currentHP / (float)maxHP;
                healthBar.value = hpRatio;
            }

        }// UpdateHealthBar


        public void ApplyDamage(DamageMessage data)
        {
            if (currentHP > 0 && !isInvulnerable)
            {
                isInvulnerable = true;
                currentHP = Math.Max(currentHP - data.amount, 0); // HP shouldn't drop below 0

                var messageType = currentHP <= 0 ? MessageType.DEAD : MessageType.DAMAGED;

                for (int i = 0; i < onDamageMessageReceivers.Count; i++)
                {
                    var receiver = onDamageMessageReceivers[i] as IMessageReceiver; // IMessageReceiver has the OnReceiveMessage method, MonoBehaviour doesn't always
                    receiver.OnReceiveMessage(messageType);
                }
            }
        }// ApplyDamage


        public void ResetHP()
        {
            currentHP = maxHP;
        }// ResetHP
    }
}

