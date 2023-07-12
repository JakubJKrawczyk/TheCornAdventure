using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    internal class Cheats : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AmmoStorage AmmoStorage;

        private void Update()
        {
            if (AmmoStorage != null)
            {
                CheatAmmo();
            }
        }

        private void CheatAmmo()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                AmmoStorage.AddAmmo(0, 10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                AmmoStorage.AddAmmo(1, 10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                AmmoStorage.AddAmmo(2, 10);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                AmmoStorage.RemoveAmmo(0, 10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                AmmoStorage.RemoveAmmo(1, 10);
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                AmmoStorage.RemoveAmmo(2, 10);
            }
        }




    }
}
