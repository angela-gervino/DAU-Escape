using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DAUEscape
{
    public class MeleeWeapon : MonoBehaviour
    {
        public int damage = 10;

        public void BeginAttack()
        {
            Debug.Log("weapon is swinging");
        }
    }

}


