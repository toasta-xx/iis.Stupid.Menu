/*
 * ii's Stupid Menu  Mods/NetworkedHP.cs
 * Networked HP system (Console-synced), damage types, attacks.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GorillaLocomotion;
using iiMenu.Classes.Menu;
using iiMenu.Managers;
using iiMenu.Menu;
using iiMenu.Utilities;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using static iiMenu.Utilities.RigUtilities;

namespace iiMenu.Mods
{
    public static class NetworkedHP
    {
        public const int MaxHP = 92;
        public const string NethpPrefix = "nethp-";

        public static bool Enabled;
        public static bool BoxingEnabled;
        public static bool GasterBlasterActive;
        public static bool SoulThrowActive;
        public static float GasterBlasterCooldownUntil;
        public const float GasterBlasterDuration = 2f;
        public const float GasterBlasterCooldown = 7f;
        public const float GasterDmgPerFrame = 1f;
        public const float SoulThrowVelocityThreshold = 2f;
        public const int SoulThrowDamage = 1;
        public const int BoneDamage = 1;
        public const int PunchDamage = 8;

        public enum DamageMode { Regular, KR }
        public static DamageMode CurrentDamageMode = DamageMode.Regular;
        public static int IFrameFrames = 7;
        public const int IFrameMin = 5;
        public const int IFrameMax = 10;
        public static float KRDuration = 5f;
        public static float KRDamagePerSecond = 4f;

        private static readonly Dictionary<string, int> PlayerHP = new Dictionary<string, int>();
        private static readonly Dictionary<string, float> PlayerDisplayedHP = new Dictionary<string, float>();
        private static readonly Dictionary<string, float> PlayerIFrameEndTime = new Dictionary<string, float>();
        private static readonly Dictionary<string, float> PlayerKREndTime = new Dictionary<string, float>();
        private static GameObject _localHPObject;
        private static TextMeshPro _localHPText;
        private static readonly Dictionary<VRRig, GameObject> WorldNametags = new Dictionary<VRRig, GameObject>();
        private static readonly List<GameObject> TrackedBones = new List<GameObject>();
        private static float _punchCooldownUntil;
        private static GameObject _gasterBeam;
        private static float _gasterStartTime;
        private static readonly HashSet<int> _gasterHitThisBurst = new HashSet<int>();

        public static int GetHP(string userId)
        {
            return PlayerHP.TryGetValue(userId, out int hp) ? hp : MaxHP;
        }

        public static float GetDisplayedHP(string userId)
        {
            return PlayerDisplayedHP.TryGetValue(userId, out float d) ? d : MaxHP;
        }

        public static void Enable()
        {
            Enabled = true;
            string uid = PhotonNetwork.LocalPlayer?.UserId;
            if (!string.IsNullOrEmpty(uid))
            {
                PlayerHP[uid] = MaxHP;
                PlayerDisplayedHP[uid] = MaxHP;
            }
            CreateLocalHPUI();
            if (PhotonNetwork.InRoom)
                iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "sync", ReceiverGroup.Others, uid ?? "", MaxHP);
        }

        public static void Disable()
        {
            Enabled = false;
            BoxingEnabled = false;
            DisableGasterBlaster();
            DisableSoulThrow();
            DestroyLocalHPUI();
            DestroyAllWorldNametags();
            PlayerHP.Clear();
            PlayerDisplayedHP.Clear();
            PlayerIFrameEndTime.Clear();
            PlayerKREndTime.Clear();
            TrackedBones.Clear();
        }

        public static void EnableBoxing()
        {
            BoxingEnabled = true;
            ButtonInfo punchMod = Buttons.GetIndex("Admin Punch Mod");
            if (punchMod != null && !punchMod.enabled)
                Main.Toggle(punchMod);
        }

        public static void DisableBoxing()
        {
            BoxingEnabled = false;
        }

        public static void FireBoneAttack()
        {
            if (!Enabled || !PhotonNetwork.InRoom) return;
            VRRig rig = VRRig.LocalRig;
            Transform hand = rig.rightHandTransform;
            Vector3 fwd = hand.forward;
            fwd.y = 0f;
            fwd.Normalize();
            Vector3 pos = hand.position + fwd * 0.3f;
            Vector3 scale = new Vector3(0.15f, 0.15f, 0.8f);
            iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "bone", ReceiverGroup.Others, pos, fwd * 35f, scale);
            SpawnLocalBone(pos, fwd, scale);
        }

        private static void SpawnLocalBone(Vector3 pos, Vector3 dir, Vector3 scale)
        {
            GameObject bone = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bone.transform.position = pos;
            bone.transform.localScale = scale;
            UnityEngine.Object.Destroy(bone.GetComponent<Collider>());
            bone.AddComponent<BoxCollider>().isTrigger = true;
            Rigidbody rb = bone.AddComponent<Rigidbody>();
            rb.velocity = dir * 35f;
            rb.useGravity = true;
            bone.GetComponent<Renderer>().material.color = Color.white;
            RegisterBone(bone);
            CoroutineManager.instance.StartCoroutine(DestroyAfter(bone, 2f));
        }

        private static IEnumerator DestroyAfter(GameObject go, float t)
        {
            yield return new WaitForSeconds(t);
            if (go) UnityEngine.Object.Destroy(go);
        }

        public static void RegisterBone(GameObject bone)
        {
            if (bone != null && !TrackedBones.Contains(bone))
                TrackedBones.Add(bone);
        }

        public static void EnableGasterBlaster() { GasterBlasterActive = true; }
        public static void DisableGasterBlaster()
        {
            GasterBlasterActive = false;
            if (_gasterBeam != null) { UnityEngine.Object.Destroy(_gasterBeam); _gasterBeam = null; }
            _gasterHitThisBurst.Clear();
        }

        public static void GasterBlasterUpdate()
        {
            if (!Enabled || !GasterBlasterActive) return;
            if (_gasterBeam != null)
            {
                if (Time.time - _gasterStartTime >= GasterBlasterDuration)
                {
                    UnityEngine.Object.Destroy(_gasterBeam);
                    _gasterBeam = null;
                    GasterBlasterCooldownUntil = Time.time + GasterBlasterCooldown;
                    _gasterHitThisBurst.Clear();
                    return;
                }
                return;
            }
            if (Time.time < GasterBlasterCooldownUntil) return;
            bool trigger = ControllerInputPoller.instance != null &&
                (ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f || ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f);
            if (!trigger) return;
            StartGasterBlaster();
        }

        private static void StartGasterBlaster()
        {
            Transform palm = GorillaTagger.Instance.rightHandTransform;
            Vector3 fwd = palm.forward;
            fwd.y = 0f;
            fwd.Normalize();
            _gasterBeam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            UnityEngine.Object.Destroy(_gasterBeam.GetComponent<Collider>());
            _gasterBeam.transform.localScale = new Vector3(0.2f, 0.4f, 8f);
            _gasterBeam.transform.position = palm.position + fwd * 4f;
            _gasterBeam.transform.rotation = Quaternion.LookRotation(fwd);
            _gasterBeam.GetComponent<Renderer>().material.color = new Color(1f, 0.9f, 0.6f);
            _gasterStartTime = Time.time;
            if (PhotonNetwork.InRoom)
                iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "gaster", ReceiverGroup.Others, palm.position, fwd, 8f, GasterBlasterDuration);
        }

        public static void EnableSoulThrow() { SoulThrowActive = true; }
        public static void DisableSoulThrow() { SoulThrowActive = false; }
        public static void SoulThrowUpdate() { }

        public static void CycleDamageType(bool forward = true)
        {
            CurrentDamageMode = forward
                ? (CurrentDamageMode == DamageMode.Regular ? DamageMode.KR : DamageMode.Regular)
                : (CurrentDamageMode == DamageMode.KR ? DamageMode.Regular : DamageMode.KR);
            ButtonInfo b = Buttons.GetIndex("HP Damage Type");
            if (b != null)
                b.overlapText = $"HP Damage Type <color=grey>[</color><color=green>{CurrentDamageMode}</color><color=grey>]</color>";
        }
        public static void CycleDamageType() => CycleDamageType(true);

        public static void CycleIFrameLength(bool forward = true)
        {
            IFrameFrames = Mathf.Clamp(IFrameFrames + (forward ? 1 : -1), IFrameMin, IFrameMax);
            ButtonInfo b = Buttons.GetIndex("I-Frame Length");
            if (b != null)
                b.overlapText = $"I-Frame Length <color=grey>[</color><color=green>{IFrameFrames}</color><color=grey>]</color>";
        }
        public static void CycleIFrameLength() => CycleIFrameLength(true);

        public static void HandleNetworkCommand(Player sender, object[] args, string command)
        {
            if (command == null || !command.StartsWith(NethpPrefix)) return;
            string sub = command.Substring(NethpPrefix.Length);

            if (sub == "damage" && args.Length >= 3)
            {
                int amount = (int)(args.Length > 1 ? args[1] : 1);
                bool bypassIFrames = (bool)(args.Length > 2 ? args[2] : false);
                ApplyDamageLocal(amount, bypassIFrames);
                return;
            }

            if (sub == "sync" && args.Length >= 3)
            {
                string userId = (string)args[1];
                int hp = Convert.ToInt32(args[2]);
                if (!string.IsNullOrEmpty(userId))
                {
                    PlayerHP[userId] = Mathf.Clamp(hp, 0, MaxHP);
                    PlayerDisplayedHP[userId] = hp;
                }
                return;
            }

            if (sub == "bone" && args.Length >= 4)
            {
                Vector3 pos = (Vector3)args[1];
                Vector3 vel = (Vector3)args[2];
                Vector3 size = (Vector3)args[3];
                GameObject bone = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(bone.GetComponent<Collider>());
                bone.transform.localScale = size;
                bone.transform.position = pos;
                bone.AddComponent<BoxCollider>().isTrigger = true;
                Rigidbody rb = bone.AddComponent<Rigidbody>();
                rb.velocity = vel;
                rb.useGravity = true;
                bone.GetComponent<Renderer>().material.color = Color.white;
                RegisterBone(bone);
                CoroutineManager.instance.StartCoroutine(DestroyAfter(bone, 2f));
                return;
            }

            if (sub == "gaster" && args.Length >= 5)
            {
                Vector3 pos = (Vector3)args[1];
                Vector3 fwd = (Vector3)args[2];
                float length = (float)args[3];
                float duration = (float)args[4];
                GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
                UnityEngine.Object.Destroy(beam.GetComponent<Collider>());
                beam.transform.localScale = new Vector3(0.2f, 0.4f, length);
                beam.transform.position = pos + fwd * (length * 0.5f);
                beam.transform.rotation = Quaternion.LookRotation(fwd);
                beam.GetComponent<Renderer>().material.color = new Color(1f, 0.9f, 0.6f);
                CoroutineManager.instance.StartCoroutine(DestroyAfter(beam, duration));
                return;
            }

            if (sub == "punch" && args.Length >= 2)
            {
                int amount = (int)(args.Length > 1 ? args[1] : PunchDamage);
                ApplyDamageLocal(amount, false);
                return;
            }
        }

        private static void ApplyDamageLocal(int amount, bool bypassIFrames)
        {
            string uid = PhotonNetwork.LocalPlayer?.UserId;
            if (string.IsNullOrEmpty(uid)) return;

            if (!PlayerHP.ContainsKey(uid)) PlayerHP[uid] = MaxHP;
            if (!PlayerDisplayedHP.ContainsKey(uid)) PlayerDisplayedHP[uid] = MaxHP;

            bool inIFrames = PlayerIFrameEndTime.TryGetValue(uid, out float end) && Time.time < end;
            if (!bypassIFrames && inIFrames) return;

            if (CurrentDamageMode == DamageMode.KR)
            {
                PlayerKREndTime[uid] = Time.time + KRDuration;
                PlayerDisplayedHP[uid] = Mathf.Min(PlayerDisplayedHP[uid] + amount, MaxHP);
            }

            PlayerHP[uid] = Mathf.Max(0, PlayerHP[uid] - amount);
            if (CurrentDamageMode == DamageMode.Regular)
                PlayerIFrameEndTime[uid] = Time.time + (IFrameFrames / 60f);

            if (PlayerHP[uid] <= 0)
            {
                PlayerHP[uid] = MaxHP;
                PlayerDisplayedHP[uid] = MaxHP;
                if (PhotonNetwork.InRoom)
                    iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "sync", ReceiverGroup.Others, uid, MaxHP);
                NotificationManager.SendNotification("<color=red>YOU DIED</color> (respawn)", 3000);
                return;
            }

            if (PhotonNetwork.InRoom)
                iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "sync", ReceiverGroup.Others, uid, PlayerHP[uid]);
        }

        public static void SendPunchDamage(NetPlayer target, int amount = PunchDamage)
        {
            if (!Enabled || target == null || !PhotonNetwork.InRoom) return;
            iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "punch", target.ActorNumber, amount);
        }

        public static void SendDamageTo(int targetActorNumber, int amount, bool bypassIFrames)
        {
            if (!Enabled || !PhotonNetwork.InRoom) return;
            iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "damage", targetActorNumber, amount, bypassIFrames);
        }

        private static void CreateLocalHPUI()
        {
            if (_localHPObject != null) return;
            _localHPObject = new GameObject("NetworkedHP_Local");
            _localHPText = _localHPObject.AddComponent<TextMeshPro>();
            _localHPText.richText = true;
            _localHPText.fontSize = 2f;
            _localHPText.alignment = TextAlignmentOptions.Center;
            _localHPText.transform.localScale = Vector3.one * 0.09f;
            if (GorillaTagger.Instance?.offlineVRRig?.playerText1 != null)
            {
                _localHPText.material = UnityEngine.Object.Instantiate(GorillaTagger.Instance.offlineVRRig.playerText1.material);
                _localHPText.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            }
            _localHPObject.transform.SetParent(Camera.main != null ? Camera.main.transform : null, false);
        }

        private static void DestroyLocalHPUI()
        {
            if (_localHPObject != null) { UnityEngine.Object.Destroy(_localHPObject); _localHPObject = null; _localHPText = null; }
        }

        public static void UpdateLocalHPDisplay()
        {
            if (!Enabled || _localHPText == null) return;
            string uid = PhotonNetwork.LocalPlayer?.UserId ?? "";
            int hp = GetHP(uid);
            float disp = GetDisplayedHP(uid);
            int totalBars = MaxHP / 10;
            float yellowPercent = (float)hp / MaxHP;
            float purplePercent = Mathf.Clamp(disp / MaxHP, 0f, 1f);
            int yellowBars = Mathf.RoundToInt(yellowPercent * totalBars);
            int purpleBars = Mathf.RoundToInt(purplePercent * totalBars);
            var bar = new StringBuilder(totalBars);
            for (int i = 0; i < totalBars; i++)
            {
                if (i < yellowBars) bar.Append("<color=yellow>█</color>");
                else if (i < purpleBars) bar.Append("<color=#C800FF>█</color>");
                else bar.Append("<color=black>-</color>");
            }
            _localHPText.text = $"HP: {bar} <color=white>KR {(int)disp}/{MaxHP}</color>";
            if (GorillaTagger.Instance != null && GorillaTagger.Instance.headCollider != null)
            {
                var head = GorillaTagger.Instance.headCollider.transform;
                _localHPObject.transform.position = head.position + head.forward * 0.5f - new Vector3(0, 0.2f, 0);
                _localHPObject.transform.rotation = Quaternion.LookRotation(head.forward, Vector3.up);
            }
        }

        public static void RefreshWorldNametags()
        {
            if (!Enabled) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null) continue;
                NetPlayer p = GetPlayerFromVRRig(rig);
                string uid = p?.UserId;
                if (string.IsNullOrEmpty(uid)) continue;
                int hp = GetHP(uid);
                string name = p?.NickName ?? "?";
                if (!WorldNametags.TryGetValue(rig, out GameObject go) || go == null)
                {
                    go = new GameObject("HPNametag_" + uid);
                    var tm = go.AddComponent<TextMeshPro>();
                    tm.richText = true;
                    tm.fontSize = 3f;
                    tm.alignment = TextAlignmentOptions.Center;
                    go.transform.localScale = Vector3.one * 0.08f;
                    if (GorillaTagger.Instance?.offlineVRRig?.playerText1 != null)
                    {
                        tm.material = UnityEngine.Object.Instantiate(GorillaTagger.Instance.offlineVRRig.playerText1.material);
                        tm.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
                    }
                    WorldNametags[rig] = go;
                }
                var text = go.GetComponent<TextMeshPro>();
                if (text != null)
                    text.text = $"{name}\n<color=yellow>HP: {hp}/{MaxHP}</color>";
                if (rig.headMesh != null)
                {
                    go.transform.position = rig.headMesh.transform.position + Vector3.up * 0.6f;
                    if (Camera.main != null)
                        go.transform.rotation = Quaternion.LookRotation(go.transform.position - Camera.main.transform.position);
                }
            }
            foreach (var kvp in WorldNametags.Where(x => x.Key == null || !VRRigCache.ActiveRigs.Contains(x.Key)).ToList())
            {
                if (kvp.Value != null) UnityEngine.Object.Destroy(kvp.Value);
                WorldNametags.Remove(kvp.Key);
            }
        }

        private static void DestroyAllWorldNametags()
        {
            foreach (var go in WorldNametags.Values)
                if (go != null) UnityEngine.Object.Destroy(go);
            WorldNametags.Clear();
        }

        public static void TickKRDoT()
        {
            string uid = PhotonNetwork.LocalPlayer?.UserId;
            if (string.IsNullOrEmpty(uid)) return;
            if (!PlayerKREndTime.TryGetValue(uid, out float end) || Time.time >= end) return;
            if (!PlayerHP.TryGetValue(uid, out int hp)) return;
            int drain = Mathf.Max(0, (int)(KRDamagePerSecond * Time.deltaTime));
            if (drain <= 0) return;
            PlayerHP[uid] = Mathf.Max(0, hp - drain);
            PlayerDisplayedHP[uid] = Mathf.Max(PlayerHP[uid], PlayerDisplayedHP.TryGetValue(uid, out float d) ? d - drain : MaxHP - drain);
            if (PhotonNetwork.InRoom)
                iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "sync", ReceiverGroup.Others, uid, PlayerHP[uid]);
            if (PlayerHP[uid] <= 0)
            {
                PlayerHP[uid] = MaxHP;
                PlayerDisplayedHP[uid] = MaxHP;
                PlayerKREndTime.Remove(uid);
                if (PhotonNetwork.InRoom)
                    iiMenu.Classes.Menu.Console.ExecuteCommand(NethpPrefix + "sync", ReceiverGroup.Others, uid, MaxHP);
                NotificationManager.SendNotification("<color=red>YOU DIED</color> (KR)", 3000);
            }
        }

        public static void TickBoneCollisions()
        {
            if (!Enabled) return;
            for (int i = TrackedBones.Count - 1; i >= 0; i--)
            {
                GameObject bone = TrackedBones[i];
                if (bone == null) { TrackedBones.RemoveAt(i); continue; }
                var col = bone.GetComponent<Collider>();
                if (col == null) continue;
                if (GorillaTagger.Instance == null) continue;
                if (col.bounds.Intersects(GorillaTagger.Instance.headCollider.bounds) ||
                    col.bounds.Intersects(GorillaTagger.Instance.bodyCollider.bounds))
                {
                    ApplyDamageLocal(BoneDamage, true);
                    TrackedBones.RemoveAt(i);
                    UnityEngine.Object.Destroy(bone);
                    break;
                }
            }
        }

        public static void TickBoxingPunch()
        {
            if (!Enabled || !BoxingEnabled || Time.time < _punchCooldownUntil) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isLocal) continue;
                float left = Vector3.Distance(GorillaTagger.Instance.leftHandTransform.position, rig.headMesh.transform.position);
                float right = Vector3.Distance(GorillaTagger.Instance.rightHandTransform.position, rig.headMesh.transform.position);
                if (left < 0.25f || right < 0.25f)
                {
                    NetPlayer p = GetPlayerFromVRRig(rig);
                    if (p != null) SendPunchDamage(p, PunchDamage);
                    _punchCooldownUntil = Time.time + 0.2f;
                    break;
                }
            }
        }

        public static void TickGasterBeamDamage()
        {
            if (_gasterBeam == null || !PhotonNetwork.InRoom) return;
            var bounds = _gasterBeam.GetComponent<Renderer>()?.bounds;
            if (bounds == null) return;
            foreach (VRRig rig in VRRigCache.ActiveRigs)
            {
                if (rig == null || rig.isLocal) continue;
                int actor = GetPlayerFromVRRig(rig)?.ActorNumber ?? -1;
                if (actor < 0 || _gasterHitThisBurst.Contains(actor)) continue;
                Renderer headRenderer = rig.headMesh != null ? rig.headMesh.GetComponent<Renderer>() : null;
                if (headRenderer != null && bounds.Value.Intersects(headRenderer.bounds))
                {
                    _gasterHitThisBurst.Add(actor);
                    SendDamageTo(actor, (int)GasterDmgPerFrame, true);
                }
            }
        }
    }
}
