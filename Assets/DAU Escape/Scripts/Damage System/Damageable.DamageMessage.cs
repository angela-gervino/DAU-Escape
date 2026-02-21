using UnityEngine;
using System.Collections;


namespace DAUEscape
{
    public partial class Damageable : MonoBehaviour
    {
        public struct DamageMessage
        {
            public MonoBehaviour damager;
            public int amount;
        }
    }
}
