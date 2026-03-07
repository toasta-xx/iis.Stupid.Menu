/*
 * ii's Stupid Menu  Networked/NetworkedHPUpdater.cs
 * MonoBehaviour that drives networked HP updates, nametags, and per-frame logic.
 */

using iiMenu.Mods;
using UnityEngine;

namespace iiMenu.Networked
{
    public class NetworkedHPUpdater : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (!NetworkedHP.Enabled) return;

            NetworkedHP.UpdateLocalHPDisplay();
            NetworkedHP.RefreshWorldNametags();
            NetworkedHP.TickKRDoT();
            NetworkedHP.TickBoneCollisions();
            NetworkedHP.TickBoxingPunch();
            NetworkedHP.TickGasterBeamDamage();
        }
    }
}
