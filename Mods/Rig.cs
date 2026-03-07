/*
 * ii's Stupid Menu  Mods/Rig.cs
 * A mod menu for Gorilla Tag with over 1000+ mods
 *
 * Copyright (C) 2026  Goldentrophy Software
 * https://github.com/iiDk-the-actual/iis.Stupid.Menu
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using ExitGames.Client.Photon;
using GorillaExtensions;
using GorillaLocomotion;
using iiMenu.Classes.Menu;
using iiMenu.Extensions;
using iiMenu.Menu;
using iiMenu.Patches.Menu;
using iiMenu.Utilities;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static iiMenu.Menu.Main;
using static iiMenu.Utilities.RandomUtilities;
using static iiMenu.Utilities.RigUtilities;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace iiMenu.Mods
{
    public static class Rig
    {
        public static bool wasDisabledAlready;
        public static bool invisMonke;

        public static bool lastHit;
        public static bool lastHit2;
        public static bool lastRG;

        public static int spinHeadMode;

        public static void ChangeSpinHeadMode(bool positive = true)
        {
            string[] axisNames = { "X", "Y", "Z" };

            if (positive)
                spinHeadMode++;
            else
                spinHeadMode--;

            spinHeadMode %= axisNames.Length;
            if (spinHeadMode < 0)
                spinHeadMode = axisNames.Length - 1;

            string label = "Spin Head <color=grey>[</color><color=green>" + axisNames[spinHeadMode] + "</color><color=grey>]</color>";
            Buttons.GetIndex("Spin Head").overlapText = label;
            Buttons.GetIndex("Change Spin Head Axis").overlapText = "Change Spin Head Axis <color=grey>[</color><color=green>" + axisNames[spinHeadMode] + "</color><color=grey>]</color>";
        }

        public static void SpinHeadMod()
        {
            switch (spinHeadMode)
            {
                case 0: Fun.SpinHead("x"); break;
                case 1: Fun.SpinHead("y"); break;
                case 2: Fun.SpinHead("z"); break;
            }
        }

        public static int spazHeadMode;

        public static void ChangeSpazHeadMode(bool positive = true)
        {
            string[] spazNames = {
                "Random Position",
                "Random Rotation",
                "Position",
                "Rotation",
                "X",
                "Y",
                "Z"
            };

            if (positive)
                spazHeadMode++;
            else
                spazHeadMode--;

            spazHeadMode %= spazNames.Length;
            if (spazHeadMode < 0)
                spazHeadMode = spazNames.Length - 1;

            string label = "Spaz Head <color=grey>[</color><color=green>" + spazNames[spazHeadMode] + "</color><color=grey>]</color>";
            Buttons.GetIndex("Spaz Head").overlapText = label;
            Buttons.GetIndex("Change Spaz Head Mode").overlapText = "Change Spaz Head Mode <color=grey>[</color><color=green>" + spazNames[spazHeadMode] + "</color><color=grey>]</color>";
        }

        public static void SpazHeadMod()
        {
            switch (spazHeadMode)
            {
                case 0: RandomSpazHeadPosition(); break;
                case 1: RandomSpazHead(); break;
                case 2: SpazHeadPosition(); break;
                case 3: SpazHead(); break;
                case 4: Fun.SpazHead("x"); break;
                case 5: Fun.SpazHead("y"); break;
                case 6: Fun.SpazHead("z"); break;
            }
        }

        public static void EnableSpazHeadMod()
        {
            if (spazHeadMode == 0 || spazHeadMode == 2)
                EnableSpazHead();
        }

        public static void DisableSpazHeadMod()
        {
            if (spazHeadMode == 0 || spazHeadMode == 2)
                FixHeadPosition();
            else
                Fun.FixHead();
        }

        public static int bodyPatchMode;

        public static void ChangeBodyPatchMode(bool positive = true)
        {
            string[] bodyNames = {
                "Spin",
                "Spaz",
                "Reverse",
                "Rec Room",
                "Freeze"
            };

            if (positive)
                bodyPatchMode++;
            else
                bodyPatchMode--;

            bodyPatchMode %= bodyNames.Length;
            if (bodyPatchMode < 0)
                bodyPatchMode = bodyNames.Length - 1;

            string label = "Body Mod <color=grey>[</color><color=green>" + bodyNames[bodyPatchMode] + "</color><color=grey>]</color>";
            Buttons.GetIndex("Body Mod").overlapText = label;
            Buttons.GetIndex("Change Body Mod Mode").overlapText = "Change Body Mod Mode <color=grey>[</color><color=green>" + bodyNames[bodyPatchMode] + "</color><color=grey>]</color>";
        }

        public static void BodyPatchMod()
        {
            switch (bodyPatchMode)
            {
                case 0: SetBodyPatch(true); break;
                case 1: SetBodyPatch(true, 1); break;
                case 2: SetBodyPatch(true, 2); break;
                case 3: RecRoomBody(); break;
                case 4: FreezeBodyRotation(); break;
            }
        }

        public static int beybladeMode;

        public static void ChangeBeybladeMode(bool positive = true)
        {
            string[] bladeNames = {
                "Normal",
                "Still"
            };

            if (positive)
                beybladeMode++;
            else
                beybladeMode--;

            beybladeMode %= bladeNames.Length;
            if (beybladeMode < 0)
                beybladeMode = bladeNames.Length - 1;

            string label = "Beyblade <color=grey>[</color><color=green>" + bladeNames[beybladeMode] + "</color><color=grey>]</color>";
            Buttons.GetIndex("Beyblade").overlapText = label;
            Buttons.GetIndex("Change Beyblade Mode").overlapText = "Change Beyblade Mode <color=grey>[</color><color=green>" + bladeNames[beybladeMode] + "</color><color=grey>]</color>";
        }

        public static void BeybladeMod()
        {
            switch (beybladeMode)
            {
                case 0: Beyblade(); break;
                case 1: StillBeyblade(); break;
            }
        }

        public static void DisableBeybladeMod()
        {
            if (beybladeMode == 1)
                stillBeybladeStartPos = Vector3.zero;
        }

        public static void Invisible()
        {
            bool hit = rightSecondary;
            if (Buttons.GetIndex("Non-Togglable Invisible").enabled)
                invisMonke = hit;
            if (invisMonke)
            {
                VRRig.LocalRig.enabled = false;
                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position - Vector3.up * 99999f;
            }
            if (hit && !lastHit2)
            {
                invisMonke = !invisMonke;
                if (invisMonke)
                    wasDisabledAlready = VRRig.LocalRig.enabled;
                else
                    VRRig.LocalRig.enabled = wasDisabledAlready;
            }
            lastHit2 = hit;
        }

        private static bool ghostMonke;
        public static void Ghost()
        {
            bool hit = rightPrimary;
            if (Buttons.GetIndex("Non-Togglable Ghost").enabled)
                ghostMonke = hit;
            
            VRRig.LocalRig.enabled = !ghostMonke;
            if (hit && !lastHit)
                ghostMonke = !ghostMonke;
            
            lastHit = hit;
        }

        public static void EnableRig()
        {
            VRRig.LocalRig.enabled = true;
            ghostException = false;
        }

        public static void RigGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                GameObject NewPointer = GunData.NewPointer;

                if (GetGunInput(true))
                {
                    VRRig.LocalRig.enabled = false;
                    VRRig.LocalRig.transform.position = NewPointer.transform.position + new Vector3(0, 1, 0);
                }
                else
                    VRRig.LocalRig.enabled = true;
            }
        }

        private static Quaternion grabHeadRot;

        private static Vector3 grabLeftHandPos;
        private static Quaternion grabLeftHandRot;

        private static Vector3 grabRightHandPos;
        private static Quaternion grabRightHandRot;

        public static void GrabRig()
        {
            if (rightGrab)
            {
                if (grabHeadRot == Quaternion.identity)
                    grabHeadRot = VRRig.LocalRig.transform.InverseTransformRotation(VRRig.LocalRig.head.rigTarget.transform.rotation);

                if (grabLeftHandPos == Vector3.zero)
                    grabLeftHandPos = VRRig.LocalRig.transform.InverseTransformPoint(VRRig.LocalRig.leftHand.rigTarget.transform.position);

                if (grabLeftHandRot == Quaternion.identity)
                    grabLeftHandRot = VRRig.LocalRig.transform.InverseTransformRotation(VRRig.LocalRig.leftHand.rigTarget.transform.rotation);

                if (grabRightHandPos == Vector3.zero)
                    grabRightHandPos = VRRig.LocalRig.transform.InverseTransformPoint(VRRig.LocalRig.rightHand.rigTarget.transform.position);

                if (grabRightHandRot == Quaternion.identity)
                    grabRightHandRot = VRRig.LocalRig.transform.InverseTransformRotation(VRRig.LocalRig.rightHand.rigTarget.transform.rotation);

                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(new Vector3(0f, GorillaTagger.Instance.rightHandTransform.rotation.eulerAngles.y, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.TransformRotation(grabHeadRot);

                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.rightHandTransform.TransformPoint(grabLeftHandPos);
                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.TransformRotation(grabLeftHandRot);

                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.rightHandTransform.TransformPoint(grabRightHandPos);
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.TransformRotation(grabRightHandRot);
            }
            else
            {
                VRRig.LocalRig.enabled = true;

                grabHeadRot = Quaternion.identity;

                grabLeftHandPos = Vector3.zero;
                grabRightHandPos = Vector3.zero;

                grabLeftHandRot = Quaternion.identity;
                grabRightHandRot = Quaternion.identity;
            }
        }

        public static Vector3 offsetLH = Vector3.zero;
        public static Vector3 offsetRH = Vector3.zero;
        public static Vector3 offsetH = Vector3.zero;
        public static void EnableSpazRig()
        {
            ghostException = true;
            offsetLH = VRRig.LocalRig.leftHand.trackingPositionOffset;
            offsetRH = VRRig.LocalRig.rightHand.trackingPositionOffset;
            offsetH = VRRig.LocalRig.head.trackingPositionOffset;
        }

        public static void SpazRig()
        {
            if (rightPrimary)
            {
                float spazAmount = 0.1f;
                ghostException = true;
                VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH + RandomVector3(spazAmount);
                VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH + RandomVector3(spazAmount);
                VRRig.LocalRig.head.trackingPositionOffset = offsetH + RandomVector3(spazAmount);
            }
            else
            {
                ghostException = false;
                VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH;
                VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH;
                VRRig.LocalRig.head.trackingPositionOffset = offsetH;
            }
        }

        public static void DisableSpazRig()
        {
            ghostException = false;
            VRRig.LocalRig.leftHand.trackingPositionOffset = offsetLH;
            VRRig.LocalRig.rightHand.trackingPositionOffset = offsetRH;
            VRRig.LocalRig.head.trackingPositionOffset = offsetH;
        }

        public static void SpazHands()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();

                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.leftHandTransform.position + VRRig.LocalRig.leftHand.rigTarget.transform.forward * 3f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.rightHandTransform.position + VRRig.LocalRig.rightHand.rigTarget.transform.forward * 3f;
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void SpazRealHands()
        {
            if (rightPrimary)
            {
                GTPlayer.Instance.GetControllerTransform(true).rotation = RandomQuaternion();
                GTPlayer.Instance.GetControllerTransform(true).position = GorillaTagger.Instance.leftHandTransform.position + GTPlayer.Instance.GetControllerTransform(true).forward * 3f;

                GTPlayer.Instance.GetControllerTransform(false).rotation = RandomQuaternion();
                GTPlayer.Instance.GetControllerTransform(false).position = GorillaTagger.Instance.rightHandTransform.position + GTPlayer.Instance.GetControllerTransform(false).forward * 3f;
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void FreezeRigLimbs()
        {
            VRRig.LocalRig.enabled = false;

            VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
        }

        public static void FixRigHandRotation()
        {
            VRRig.LocalRig.leftHand.rigTarget.transform.rotation *= Quaternion.Euler(VRRig.LocalRig.leftHand.trackingRotationOffset);
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation *= Quaternion.Euler(VRRig.LocalRig.rightHand.trackingRotationOffset);
        }

        public static void FreezeRigBody()
        {
            VRRig.LocalRig.enabled = false;

            var (leftPosition, leftRotation, _, _, _) = ControllerUtilities.GetTrueLeftHand();
            var (rightPosition, rightRotation, _, _, _) = ControllerUtilities.GetTrueRightHand();

            VRRig.LocalRig.leftHand.rigTarget.transform.position = leftPosition;
            VRRig.LocalRig.rightHand.rigTarget.transform.position = rightPosition;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = leftRotation;
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = rightRotation;

            FixRigHandRotation();

            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;
        }

        public static Vector3? startPosition;
        public static void FreezeRig()
        {
            if (startPosition == null)
                startPosition = VRRig.LocalRig.transform.position;

            VRRig.LocalRig.enabled = true;
            VRRig.LocalRig.PostTick();
            VRRig.LocalRig.transform.position = startPosition.Value;
            VRRig.LocalRig.enabled = false;
        }

        public static void ParalyzeRig()
        {
            VRRig.LocalRig.enabled = false;
            VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;

            VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;
            VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
        }

        public static void ChickenRig()
        {
            VRRig.LocalRig.enabled = false;
            VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.headCollider.transform.rotation;
        
            VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.2f + GorillaTagger.Instance.bodyCollider.transform.up * -0.2f;
            VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.2f + GorillaTagger.Instance.bodyCollider.transform.up * -0.2f;
        
            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
        }

        public static void AmputateRig()
        {
            VRRig.LocalRig.enabled = false;
            VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(160f, 90f, 0f);

            VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;
            VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.08f + GorillaTagger.Instance.bodyCollider.transform.up * 0.12f;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(0f, 180f, 180f);
        }

        public static void DecapitateRigUpdate() =>
            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation * Quaternion.Euler(160f, 90f, 0f);

        public static void SetBodyPatch(bool enabled, int mode = 0)
        {
            TorsoPatch.enabled = enabled;
            TorsoPatch.mode = mode;

            if (!enabled && recBodyRotary != null)
                Object.Destroy(recBodyRotary);
        }

        public static GameObject recBodyRotary;
        public static void RecRoomBody()
        {
            SetBodyPatch(true, 3);

            if (recBodyRotary == null)
                recBodyRotary = new GameObject("ii_recBodyRotary");

            recBodyRotary.transform.rotation = Quaternion.Lerp(recBodyRotary.transform.rotation, Quaternion.Euler(0f, GorillaTagger.Instance.headCollider.transform.rotation.eulerAngles.y, 0f), Time.deltaTime * 6.5f);
        }

        public static void FreezeBodyRotation()
        {
            SetBodyPatch(true, 3);

            if (recBodyRotary == null)
                recBodyRotary = new GameObject("ii_recBodyRotary");

            recBodyRotary.transform.rotation = rightGrab ? recBodyRotary.transform.rotation : Quaternion.Euler(0f, GorillaTagger.Instance.headCollider.transform.rotation.eulerAngles.y, 0f);
        }

        public static void AutoDance()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                Vector3 bodyOffset = GorillaTagger.Instance.bodyCollider.transform.right * (Mathf.Cos(Time.frameCount / 20f) * 0.3f) + new Vector3(0f, Mathf.Abs(Mathf.Sin(Time.frameCount / 20f) * 0.2f), 0f);
                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f) + bodyOffset;
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
                
                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.forward * 0.2f + VRRig.LocalRig.transform.right * -0.4f + VRRig.LocalRig.transform.up * (0.3f + Mathf.Sin(Time.frameCount / 20f) * 0.2f);
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.forward * 0.2f + VRRig.LocalRig.transform.right * 0.4f + VRRig.LocalRig.transform.up * (0.3f + Mathf.Sin(Time.frameCount / 20f) * -0.2f);

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void AutoGriddy()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                Vector3 bodyOffset = VRRig.LocalRig.transform.forward * (5f * Time.deltaTime);
                VRRig.LocalRig.transform.position = VRRig.LocalRig.transform.position + bodyOffset;
                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -0.33f + VRRig.LocalRig.transform.forward * (0.5f * Mathf.Cos(Time.frameCount / 10f)) + VRRig.LocalRig.transform.up * (-0.5f * Mathf.Abs(Mathf.Sin(Time.frameCount / 10f)));
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 0.33f + VRRig.LocalRig.transform.forward * (0.5f * Mathf.Cos(Time.frameCount / 10f)) + VRRig.LocalRig.transform.up * (-0.5f * Mathf.Abs(Mathf.Sin(Time.frameCount / 10f)));

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void AutoTPose()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void Helicopter()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position += new Vector3(0f, 0.05f, 0f);
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static void Beyblade()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        public static Vector3 stillBeybladeStartPos = Vector3.zero;
        public static void StillBeyblade()
        {
            if (rightPrimary)
            {
                if (stillBeybladeStartPos == Vector3.zero)
                    stillBeybladeStartPos = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = stillBeybladeStartPos;
                VRRig.LocalRig.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 10f, 0f));

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
            {
                stillBeybladeStartPos = Vector3.zero;
                VRRig.LocalRig.enabled = true;
            }
        }

        public static void TorsoPatch_VRRigLateUpdate() =>
            VRRig.LocalRig.transform.rotation *= Quaternion.Euler(0f, Time.time * 180f % 360f, 0f);

        public static void Fan()
        {
            if (rightPrimary)
            {
                VRRig.LocalRig.enabled = false;

                VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
                VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

                VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + (VRRig.LocalRig.transform.up * (Mathf.Cos(Time.time * 15f) * 2f) + VRRig.LocalRig.transform.right * (Mathf.Sin(Time.time * 15f) * 2f));
                VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position - (VRRig.LocalRig.transform.up * (Mathf.Cos(Time.time * 15f) * 2f) + VRRig.LocalRig.transform.right * (Mathf.Sin(Time.time * 15f) * 2f));

                VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                FixRigHandRotation();
            }
            else
                VRRig.LocalRig.enabled = true;
        }

        private static Vector3 headPos = Vector3.zero;
        private static Vector3 headRot = Vector3.zero;

        private static Vector3 handPos_L = Vector3.zero;
        private static Vector3 handRot_L = Vector3.zero;

        private static Vector3 handPos_R = Vector3.zero;
        private static Vector3 handRot_R = Vector3.zero;

        public static void GhostAnimations()
        {
            VRRig.LocalRig.enabled = false;

            if (headPos == Vector3.zero)
                headPos = GorillaTagger.Instance.headCollider.transform.position;
            if (headRot == Vector3.zero)
                headRot = GorillaTagger.Instance.headCollider.transform.rotation.eulerAngles;

            if (handPos_L == Vector3.zero)
                handPos_L = GorillaTagger.Instance.leftHandTransform.transform.position;
            if (handRot_L == Vector3.zero)
                handRot_L = GorillaTagger.Instance.leftHandTransform.transform.rotation.eulerAngles;

            if (handPos_R == Vector3.zero)
                handPos_R = GorillaTagger.Instance.rightHandTransform.transform.position;
            if (handRot_R == Vector3.zero)
                handRot_R = GorillaTagger.Instance.rightHandTransform.transform.rotation.eulerAngles;

            float positionSpeed = 0.01f; 
            float rotationSpeed = 2.0f; 
            float positionThreshold = 0.05f;
            float rotationThreshold = 11.5f; 
            if (Vector3.Distance(headPos, GorillaTagger.Instance.headCollider.transform.position) > positionThreshold)
                headPos += Vector3.Normalize(GorillaTagger.Instance.headCollider.transform.position - headPos) * positionSpeed;

            if (Quaternion.Angle(Quaternion.Euler(headRot), GorillaTagger.Instance.headCollider.transform.rotation) > rotationThreshold)
                headRot = Quaternion.RotateTowards(Quaternion.Euler(headRot), GorillaTagger.Instance.headCollider.transform.rotation, rotationSpeed).eulerAngles;

            if (Vector3.Distance(handPos_L, GorillaTagger.Instance.leftHandTransform.transform.position) > positionThreshold)
                handPos_L += Vector3.Normalize(GorillaTagger.Instance.leftHandTransform.transform.position - handPos_L) * positionSpeed;

            if (Quaternion.Angle(Quaternion.Euler(handRot_L), GorillaTagger.Instance.leftHandTransform.transform.rotation) > rotationThreshold)
                handRot_L = Quaternion.RotateTowards(Quaternion.Euler(handRot_L), GorillaTagger.Instance.leftHandTransform.transform.rotation, rotationSpeed).eulerAngles;

            if (Vector3.Distance(handPos_R, GorillaTagger.Instance.rightHandTransform.transform.position) > positionThreshold)
                handPos_R += Vector3.Normalize(GorillaTagger.Instance.rightHandTransform.transform.position - handPos_R) * positionSpeed;

            if (Quaternion.Angle(Quaternion.Euler(handRot_R), GorillaTagger.Instance.rightHandTransform.transform.rotation) > rotationThreshold)
                handRot_R = Quaternion.RotateTowards(Quaternion.Euler(handRot_R), GorillaTagger.Instance.rightHandTransform.transform.rotation, rotationSpeed).eulerAngles;

            VRRig.LocalRig.transform.position = headPos - new Vector3(0f, 0.15f, 0f);
            VRRig.LocalRig.transform.rotation = Quaternion.Euler(new Vector3(0f, headRot.y, 0f));

            VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(headRot);

            VRRig.LocalRig.leftHand.rigTarget.transform.position = handPos_L;
            VRRig.LocalRig.rightHand.rigTarget.transform.position = handPos_R;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(handRot_L);
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(handRot_R);

            VRRig.LocalRig.leftIndex.calcT = leftTrigger;
            VRRig.LocalRig.leftMiddle.calcT = leftGrab ? 1 : 0;
            VRRig.LocalRig.leftThumb.calcT = leftPrimary || leftSecondary ? 1 : 0;

            VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
            VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
            VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

            VRRig.LocalRig.rightIndex.calcT = rightTrigger;
            VRRig.LocalRig.rightMiddle.calcT = rightGrab ? 1 : 0;
            VRRig.LocalRig.rightThumb.calcT = rightPrimary || rightSecondary ? 1 : 0;

            VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
            VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
            VRRig.LocalRig.rightThumb.LerpFinger(1f, false);

            FixRigHandRotation();
        }

        public static void DisableGhostAnimations()
        {
            headPos = Vector3.zero;
            headRot = Vector3.zero;

            handPos_L = Vector3.zero;
            handRot_L = Vector3.zero;

            handPos_R = Vector3.zero;
            handRot_R = Vector3.zero;

            VRRig.LocalRig.enabled = true;
        }

        public static void MinecraftAnimations()
        {
            VRRig.LocalRig.enabled = false;

            VRRig.LocalRig.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + new Vector3(0f, 0.15f, 0f);
            VRRig.LocalRig.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            VRRig.LocalRig.head.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.bodyCollider.transform.rotation;

            if (rightPrimary)
            {
                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f + GorillaTagger.Instance.bodyCollider.transform.forward * Mathf.Sin(Time.frameCount / 10f);
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f + -(GorillaTagger.Instance.bodyCollider.transform.forward * Mathf.Sin(Time.frameCount / 10f));
            } else
            {
                VRRig.LocalRig.leftHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * -0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.25f + GorillaTagger.Instance.bodyCollider.transform.up * -1f;
            }

            if (rightSecondary)
            {
                VRRig.LocalRig.rightHand.rigTarget.transform.position = GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.bodyCollider.transform.right * 0.25f + Vector3.Lerp(GorillaTagger.Instance.rightHandTransform.forward, - GorillaTagger.Instance.rightHandTransform.up, 0.5f) * 2f;
                VRRig.LocalRig.rightHand.rigTarget.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
            }

            FixRigHandRotation();
        }

        public static void StareAtNearby() =>
            VRRig.LocalRig.head.rigTarget.LookAt(GetClosestVRRig().headMesh.transform.position);

        public static void StareAtTarget() =>
            VRRig.LocalRig.head.rigTarget.LookAt(lockTarget.headMesh.transform.position);

        private static bool hasAdded;
        public static void StareAtGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        if (!hasAdded)
                        {
                            hasAdded = true;
                            TorsoPatch.VRRigLateUpdate += StareAtTarget;
                        }

                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    if (hasAdded)
                    {
                        hasAdded = false;
                        TorsoPatch.VRRigLateUpdate -= StareAtTarget;
                    }
                }
            }
        }

        public static void StareAtAll()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Quaternion headRotArchive = VRRig.LocalRig.head.rigTarget.transform.rotation;
                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.LookRotation(Vector3.Normalize(GetVRRigFromPlayer(Player).headMesh.transform.position));
                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();
                VRRig.LocalRig.head.rigTarget.transform.rotation = headRotArchive;

                return false;
            };
        }

        public static void EyeContact()
        {
            foreach (VRRig rig in VRRigCache.ActiveRigs.Where(rig => !rig.IsLocal()))
            {
                if (Physics.SphereCast(rig.headMesh.transform.position + (rig.headMesh.transform.forward * 0.25f), 0.25f, rig.headMesh.transform.forward, out _, 512f, NoInvisLayerMask()))
                {
                    VRRig.LocalRig.head.rigTarget.LookAt(rig.headMesh.transform.position);
                    break;
                }
            }
        }

        public static void EnableFloatingRig() =>
            offsetH = VRRig.LocalRig.head.trackingPositionOffset;

        public static void FloatingRig() =>
            VRRig.LocalRig.head.trackingPositionOffset = offsetH + new Vector3(0f, 0.65f + Mathf.Sin(Time.frameCount / 40f) * 0.2f, 0f);

        public static void DisableFloatingRig() =>
            VRRig.LocalRig.head.trackingPositionOffset = offsetH;

        public static float beesDelay;
        public static void Bees()
        {
            VRRig.LocalRig.enabled = false;
            if (Time.time > beesDelay)
            {
                VRRig target = GetRandomVRRig(false);

                VRRig.LocalRig.transform.position = target.transform.position + Vector3.up;

                VRRig.LocalRig.leftHand.rigTarget.transform.position = target.transform.position;
                VRRig.LocalRig.rightHand.rigTarget.transform.position = target.transform.position;

                beesDelay = Time.time + 0.777f;
            }
        }

        public static void PiggybackGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    TeleportPlayer(lockTarget.transform.position + new Vector3(0f, 0.5f, 0f));
                    GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                    gunLocked = false;
            }
        }

        public static void PiggybackAll()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig.LocalRig.transform.position = GetVRRigFromPlayer(Player).headMesh.transform.position;
                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.transform.position = archivePos;

                return false;
            };
        }

        public static void CopyMovementGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                    CopyMovementPlayer(GetPlayerFromVRRig(lockTarget));
                
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static void CopyMovementPlayer(NetPlayer player, bool fingers = true)
        {
            VRRig targetRig = GetVRRigFromPlayer(player);
            VRRig.LocalRig.enabled = false;

            VRRig.LocalRig.transform.position = targetRig.transform.position;
            VRRig.LocalRig.transform.rotation = targetRig.transform.rotation;

            VRRig.LocalRig.leftHand.rigTarget.transform.position = targetRig.leftHandTransform.position;
            VRRig.LocalRig.rightHand.rigTarget.transform.position = targetRig.rightHandTransform.position;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = targetRig.leftHandTransform.rotation;
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = targetRig.rightHandTransform.rotation;

            if (fingers)
            {
                VRRig.LocalRig.leftIndex.calcT = targetRig.leftIndex.calcT;
                VRRig.LocalRig.leftMiddle.calcT = targetRig.leftMiddle.calcT;
                VRRig.LocalRig.leftThumb.calcT = targetRig.leftThumb.calcT;

                VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                VRRig.LocalRig.rightIndex.calcT = targetRig.rightIndex.calcT;
                VRRig.LocalRig.rightMiddle.calcT = targetRig.rightMiddle.calcT;
                VRRig.LocalRig.rightThumb.calcT = targetRig.rightThumb.calcT;

                VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                VRRig.LocalRig.rightThumb.LerpFinger(1f, false);
            }

            VRRig.LocalRig.head.rigTarget.transform.rotation = targetRig.headMesh.transform.rotation;
        }

        public static void CopyMovementAll()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;
                Quaternion archiveRot = VRRig.LocalRig.transform.rotation;

                Vector3 archivePosLeft = VRRig.LocalRig.leftHand.rigTarget.position;
                Quaternion archiveRotLeft = VRRig.LocalRig.leftHand.rigTarget.rotation;

                Vector3 archivePosRight = VRRig.LocalRig.rightHand.rigTarget.position;
                Quaternion archiveRotRight = VRRig.LocalRig.rightHand.rigTarget.rotation;

                Quaternion archiveHeadRot = VRRig.LocalRig.head.rigTarget.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    CopyMovementPlayer(Player, false);
                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.enabled = true;

                VRRig.LocalRig.transform.position = archivePos;
                VRRig.LocalRig.transform.rotation = archiveRot;

                VRRig.LocalRig.leftHand.rigTarget.position = archivePosLeft;
                VRRig.LocalRig.leftHand.rigTarget.rotation = archiveRotLeft;

                VRRig.LocalRig.rightHand.rigTarget.position = archivePosRight;
                VRRig.LocalRig.rightHand.rigTarget.rotation = archiveRotRight;

                VRRig.LocalRig.head.rigTarget.transform.rotation = archiveHeadRot;

                return false;
            };
        }

        public static void FollowPlayer(NetPlayer player, bool fingers = true)
        {
            VRRig targetRig = GetVRRigFromPlayer(player);
            VRRig.LocalRig.enabled = false;

            Vector3 look = targetRig.transform.position - VRRig.LocalRig.transform.position;
            look.Normalize();

            Vector3 position = VRRig.LocalRig.transform.position + look * (Movement.FlySpeed / 2f * Time.deltaTime);

            VRRig.LocalRig.transform.position = position;
            VRRig.LocalRig.transform.LookAt(targetRig.transform.position);

            VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
            VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
            VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

            VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
            VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

            FixRigHandRotation();

            if (fingers)
            {
                VRRig.LocalRig.leftIndex.calcT = 0f;
                VRRig.LocalRig.leftMiddle.calcT = 0f;
                VRRig.LocalRig.leftThumb.calcT = 0f;

                VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                VRRig.LocalRig.rightIndex.calcT = 0f;
                VRRig.LocalRig.rightMiddle.calcT = 0f;
                VRRig.LocalRig.rightThumb.calcT = 0f;

                VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                VRRig.LocalRig.rightThumb.LerpFinger(1f, false);
            }
        }

        public static void FollowPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                    FollowPlayer(GetPlayerFromVRRig(lockTarget));
                
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static readonly Dictionary<VRRig, Vector3> followPositions = new Dictionary<VRRig, Vector3>();
        public static void FollowAllPlayers()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;
                Quaternion archiveRot = VRRig.LocalRig.transform.rotation;

                Vector3 archivePosLeft = VRRig.LocalRig.leftHand.rigTarget.position;
                Quaternion archiveRotLeft = VRRig.LocalRig.leftHand.rigTarget.rotation;

                Vector3 archivePosRight = VRRig.LocalRig.rightHand.rigTarget.position;
                Quaternion archiveRotRight = VRRig.LocalRig.rightHand.rigTarget.rotation;

                Quaternion archiveHeadRot = VRRig.LocalRig.head.rigTarget.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig targetRig = GetVRRigFromPlayer(Player);

                    Vector3 position = followPositions.GetValueOrDefault(targetRig, archivePos);

                    Vector3 look = targetRig.transform.position - position;
                    look.Normalize();

                    position += look * (Movement.FlySpeed / 2f * Time.deltaTime);

                    followPositions.Remove(targetRig);

                    followPositions.Add(targetRig, position);

                    VRRig.LocalRig.transform.position = position;
                    VRRig.LocalRig.transform.LookAt(targetRig.transform.position);

                    VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                    VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                    FixRigHandRotation();

                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.transform.position = archivePos;
                VRRig.LocalRig.transform.rotation = archiveRot;

                VRRig.LocalRig.leftHand.rigTarget.position = archivePosLeft;
                VRRig.LocalRig.leftHand.rigTarget.rotation = archiveRotLeft;

                VRRig.LocalRig.rightHand.rigTarget.position = archivePosRight;
                VRRig.LocalRig.rightHand.rigTarget.rotation = archiveRotRight;

                VRRig.LocalRig.head.rigTarget.transform.rotation = archiveHeadRot;

                return false;
            };
        }

        public static void OrbitPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    VRRig.LocalRig.enabled = false;

                    VRRig.LocalRig.transform.position = lockTarget.transform.position + new Vector3(Mathf.Cos(Time.frameCount / 20f), 0.5f, Mathf.Sin(Time.frameCount / 20f));
                    VRRig.LocalRig.transform.LookAt(lockTarget.transform.position);

                    VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                    VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                    FixRigHandRotation();

                    VRRig.LocalRig.leftIndex.calcT = 0f;
                    VRRig.LocalRig.leftMiddle.calcT = 0f;
                    VRRig.LocalRig.leftThumb.calcT = 0f;

                    VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                    VRRig.LocalRig.rightIndex.calcT = 0f;
                    VRRig.LocalRig.rightMiddle.calcT = 0f;
                    VRRig.LocalRig.rightThumb.calcT = 0f;

                    VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.rightThumb.LerpFinger(1f, false);
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static void OrbitAllPlayers()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;
                Quaternion archiveRot = VRRig.LocalRig.transform.rotation;

                Vector3 archivePosLeft = VRRig.LocalRig.leftHand.rigTarget.position;
                Quaternion archiveRotLeft = VRRig.LocalRig.leftHand.rigTarget.rotation;

                Vector3 archivePosRight = VRRig.LocalRig.rightHand.rigTarget.position;
                Quaternion archiveRotRight = VRRig.LocalRig.rightHand.rigTarget.rotation;

                Quaternion archiveHeadRot = VRRig.LocalRig.head.rigTarget.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig targetRig = GetVRRigFromPlayer(Player);

                    VRRig.LocalRig.transform.position = targetRig.transform.position + new Vector3(Mathf.Cos(Time.frameCount / 20f), 0.5f, Mathf.Sin(Time.frameCount / 20f));
                    VRRig.LocalRig.transform.LookAt(targetRig.transform.position);

                    VRRig.LocalRig.head.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                    VRRig.LocalRig.leftHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * -1f;
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = VRRig.LocalRig.transform.position + VRRig.LocalRig.transform.right * 1f;

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = VRRig.LocalRig.transform.rotation;

                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.transform.position = archivePos;
                VRRig.LocalRig.transform.rotation = archiveRot;

                VRRig.LocalRig.leftHand.rigTarget.position = archivePosLeft;
                VRRig.LocalRig.leftHand.rigTarget.rotation = archiveRotLeft;

                VRRig.LocalRig.rightHand.rigTarget.position = archivePosRight;
                VRRig.LocalRig.rightHand.rigTarget.rotation = archiveRotRight;

                VRRig.LocalRig.head.rigTarget.transform.rotation = archiveHeadRot;

                return false;
            };
        }

        public static void JumpscareGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    VRRig.LocalRig.enabled = false;

                    VRRig.LocalRig.transform.position = lockTarget.headMesh.transform.position + lockTarget.headMesh.transform.forward * Random.Range(0.1f, 0.5f);
                    VRRig.LocalRig.head.rigTarget.transform.LookAt(lockTarget.headMesh.transform.position);
                    Quaternion dirLook = VRRig.LocalRig.head.rigTarget.transform.rotation;

                    VRRig.LocalRig.transform.rotation = dirLook;

                    VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.headMesh.transform.position + lockTarget.headMesh.transform.right * 0.2f;
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.headMesh.transform.position + lockTarget.headMesh.transform.right * -0.2f;

                    VRRig.LocalRig.head.rigTarget.transform.rotation = dirLook;

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                    FixRigHandRotation();

                    VRRig.LocalRig.leftIndex.calcT = 0f;
                    VRRig.LocalRig.leftMiddle.calcT = 0f;
                    VRRig.LocalRig.leftThumb.calcT = 0f;

                    VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                    VRRig.LocalRig.rightIndex.calcT = 0f;
                    VRRig.LocalRig.rightMiddle.calcT = 0f;
                    VRRig.LocalRig.rightThumb.calcT = 0f;

                    VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.rightThumb.LerpFinger(1f, false);
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static void JumpscareAll()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;
                Quaternion archiveRot = VRRig.LocalRig.transform.rotation;

                Vector3 archivePosLeft = VRRig.LocalRig.leftHand.rigTarget.position;
                Quaternion archiveRotLeft = VRRig.LocalRig.leftHand.rigTarget.rotation;

                Vector3 archivePosRight = VRRig.LocalRig.rightHand.rigTarget.position;
                Quaternion archiveRotRight = VRRig.LocalRig.rightHand.rigTarget.rotation;

                Quaternion archiveHeadRot = VRRig.LocalRig.head.rigTarget.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig targetRig = GetVRRigFromPlayer(Player);

                    VRRig.LocalRig.transform.position = targetRig.headMesh.transform.position + targetRig.headMesh.transform.forward * Random.Range(0.1f, 0.5f);
                    VRRig.LocalRig.head.rigTarget.transform.LookAt(targetRig.headMesh.transform.position);
                    Quaternion dirLook = VRRig.LocalRig.head.rigTarget.transform.rotation;

                    VRRig.LocalRig.transform.rotation = dirLook;

                    VRRig.LocalRig.leftHand.rigTarget.transform.position = targetRig.headMesh.transform.position + targetRig.headMesh.transform.right * 0.2f;
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = targetRig.headMesh.transform.position + targetRig.headMesh.transform.right * -0.2f;

                    VRRig.LocalRig.head.rigTarget.transform.rotation = dirLook;

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(VRRig.LocalRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.transform.position = archivePos;
                VRRig.LocalRig.transform.rotation = archiveRot;

                VRRig.LocalRig.leftHand.rigTarget.position = archivePosLeft;
                VRRig.LocalRig.leftHand.rigTarget.rotation = archiveRotLeft;

                VRRig.LocalRig.rightHand.rigTarget.position = archivePosRight;
                VRRig.LocalRig.rightHand.rigTarget.rotation = archiveRotRight;

                VRRig.LocalRig.head.rigTarget.transform.rotation = archiveHeadRot;

                return false;
            };
        }

        public static void AnnoyPlayerGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    VRRig.LocalRig.enabled = false;

                    Vector3 position = lockTarget.transform.position + RandomVector3();

                    VRRig.LocalRig.transform.position = position;
                    VRRig.LocalRig.transform.LookAt(lockTarget.transform.position);

                    VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
                    VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.transform.position + RandomVector3();
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.transform.position + RandomVector3();

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();

                    VRRig.LocalRig.leftIndex.calcT = 0f;
                    VRRig.LocalRig.leftMiddle.calcT = 0f;
                    VRRig.LocalRig.leftThumb.calcT = 0f;

                    VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                    VRRig.LocalRig.rightIndex.calcT = 0f;
                    VRRig.LocalRig.rightMiddle.calcT = 0f;
                    VRRig.LocalRig.rightThumb.calcT = 0f;

                    VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.rightThumb.LerpFinger(1f, false);

                    Sound.SoundSpam(337, true);
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static void AnnoyAllPlayers()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;
                Quaternion archiveRot = VRRig.LocalRig.transform.rotation;

                Vector3 archivePosLeft = VRRig.LocalRig.leftHand.rigTarget.position;
                Quaternion archiveRotLeft = VRRig.LocalRig.leftHand.rigTarget.rotation;

                Vector3 archivePosRight = VRRig.LocalRig.rightHand.rigTarget.position;
                Quaternion archiveRotRight = VRRig.LocalRig.rightHand.rigTarget.rotation;

                Quaternion archiveHeadRot = VRRig.LocalRig.head.rigTarget.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig targetRig = GetVRRigFromPlayer(Player);

                    Vector3 position = targetRig.transform.position + RandomVector3();

                    VRRig.LocalRig.transform.position = position;
                    VRRig.LocalRig.transform.LookAt(targetRig.transform.position);

                    VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
                    VRRig.LocalRig.leftHand.rigTarget.transform.position = targetRig.transform.position + RandomVector3();
                    VRRig.LocalRig.rightHand.rigTarget.transform.position = targetRig.transform.position + RandomVector3();

                    VRRig.LocalRig.leftHand.rigTarget.transform.rotation = RandomQuaternion();
                    VRRig.LocalRig.rightHand.rigTarget.transform.rotation = RandomQuaternion();

                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.transform.position = archivePos;
                VRRig.LocalRig.transform.rotation = archiveRot;

                VRRig.LocalRig.leftHand.rigTarget.position = archivePosLeft;
                VRRig.LocalRig.leftHand.rigTarget.rotation = archiveRotLeft;

                VRRig.LocalRig.rightHand.rigTarget.position = archivePosRight;
                VRRig.LocalRig.rightHand.rigTarget.rotation = archiveRotRight;

                VRRig.LocalRig.head.rigTarget.transform.rotation = archiveHeadRot;

                return false;
            };
        }

        public static void IntercourseGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    VRRig.LocalRig.enabled = false;

                    if (!Buttons.GetIndex("Reverse Intercourse").enabled)
                    {
                        VRRig.LocalRig.transform.position = lockTarget.transform.position + lockTarget.transform.forward * -(0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f);
                        VRRig.LocalRig.transform.rotation = lockTarget.transform.rotation;

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * -0.2f + lockTarget.transform.up * -0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * 0.2f + lockTarget.transform.up * -0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = lockTarget.transform.rotation;
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = lockTarget.transform.rotation;
                    } else
                    {
                        VRRig.LocalRig.transform.position = lockTarget.transform.position + lockTarget.transform.forward * (0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f);
                        VRRig.LocalRig.transform.rotation = lockTarget.transform.rotation;

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * -0.2f + lockTarget.transform.up * -0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * 0.2f + lockTarget.transform.up * -0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.head.rigTarget.transform.rotation = lockTarget.transform.rotation;
                    }

                    FixRigHandRotation();
                    IntercourseNoises();

                    VRRig.LocalRig.leftIndex.calcT = 0f;
                    VRRig.LocalRig.leftMiddle.calcT = 0f;
                    VRRig.LocalRig.leftThumb.calcT = 0f;

                    VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                    VRRig.LocalRig.rightIndex.calcT = 0f;
                    VRRig.LocalRig.rightMiddle.calcT = 0f;
                    VRRig.LocalRig.rightThumb.calcT = 0f;

                    VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.rightThumb.LerpFinger(1f, false);

                    VRRig.LocalRig.head.rigTarget.transform.rotation = lockTarget.transform.rotation;
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static void IntercourseNoises()
        {
            if (Time.frameCount % 45 == 0)
            {
                if (PhotonNetwork.InRoom)
                {
                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_PlayHandTap", RpcTarget.All, 64, true, 999999f);

                    if (Buttons.GetIndex("Splash Intercourse").enabled)
                        Fun.BetaWaterSplash(VRRig.LocalRig.transform.position, VRRig.LocalRig.transform.rotation, 4f, 100f, true, false);

                    RPCProtection();
                }
                else
                    VRRig.LocalRig.PlayHandTapLocal(64, true, 999999f);
            }
        }

        public static void IntercourseAll()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;
                Quaternion archiveRot = VRRig.LocalRig.transform.rotation;

                Vector3 archivePosLeft = VRRig.LocalRig.leftHand.rigTarget.position;
                Quaternion archiveRotLeft = VRRig.LocalRig.leftHand.rigTarget.rotation;

                Vector3 archivePosRight = VRRig.LocalRig.rightHand.rigTarget.position;
                Quaternion archiveRotRight = VRRig.LocalRig.rightHand.rigTarget.rotation;

                Quaternion archiveHeadRot = VRRig.LocalRig.head.rigTarget.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig targetRig = GetVRRigFromPlayer(Player);

                    if (!Buttons.GetIndex("Reverse Intercourse").enabled)
                    {
                        VRRig.LocalRig.transform.position = targetRig.transform.position + targetRig.transform.forward * -(0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f);
                        VRRig.LocalRig.transform.rotation = targetRig.transform.rotation;

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * -0.2f + targetRig.transform.up * -0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * 0.2f + targetRig.transform.up * -0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = targetRig.transform.rotation;
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = targetRig.transform.rotation;
                    }
                    else
                    {
                        VRRig.LocalRig.transform.position = targetRig.transform.position + targetRig.transform.forward * (0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f);
                        VRRig.LocalRig.transform.rotation = targetRig.transform.rotation;

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * -0.2f + targetRig.transform.up * -0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * 0.2f + targetRig.transform.up * -0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.head.rigTarget.transform.rotation = targetRig.transform.rotation;
                    }

                    FixRigHandRotation();

                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.transform.position = archivePos;
                VRRig.LocalRig.transform.rotation = archiveRot;

                VRRig.LocalRig.leftHand.rigTarget.position = archivePosLeft;
                VRRig.LocalRig.leftHand.rigTarget.rotation = archiveRotLeft;

                VRRig.LocalRig.rightHand.rigTarget.position = archivePosRight;
                VRRig.LocalRig.rightHand.rigTarget.rotation = archiveRotRight;

                VRRig.LocalRig.head.rigTarget.transform.rotation = archiveHeadRot;

                return false;
            };
        }

        public static void HeadGun()
        {
            if (GetGunInput(false))
            {
                var GunData = RenderGun();
                RaycastHit Ray = GunData.Ray;

                if (gunLocked && lockTarget != null)
                {
                    VRRig.LocalRig.enabled = false;

                    if (!Buttons.GetIndex("Reverse Intercourse").enabled)
                    {
                        VRRig.LocalRig.transform.position = lockTarget.transform.position + lockTarget.transform.forward * (0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f) + lockTarget.transform.up * -0.4f;
                        VRRig.LocalRig.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * 0.2f + lockTarget.transform.up * -0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * -0.2f + lockTarget.transform.up * -0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    }
                    else
                    {
                        VRRig.LocalRig.transform.position = lockTarget.transform.position + lockTarget.transform.forward * (0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f) + lockTarget.transform.up * 0.4f;
                        VRRig.LocalRig.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * 0.2f + lockTarget.transform.up * 0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = lockTarget.transform.position + lockTarget.transform.right * -0.2f + lockTarget.transform.up * 0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(lockTarget.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    }

                    VRRig.LocalRig.leftIndex.calcT = 0f;
                    VRRig.LocalRig.leftMiddle.calcT = 0f;
                    VRRig.LocalRig.leftThumb.calcT = 0f;

                    VRRig.LocalRig.leftIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.leftMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.leftThumb.LerpFinger(1f, false);

                    VRRig.LocalRig.rightIndex.calcT = 0f;
                    VRRig.LocalRig.rightMiddle.calcT = 0f;
                    VRRig.LocalRig.rightThumb.calcT = 0f;

                    VRRig.LocalRig.rightIndex.LerpFinger(1f, false);
                    VRRig.LocalRig.rightMiddle.LerpFinger(1f, false);
                    VRRig.LocalRig.rightThumb.LerpFinger(1f, false);

                    FixRigHandRotation();
                    IntercourseNoises();
                }
                if (GetGunInput(true))
                {
                    VRRig gunTarget = Ray.collider != null ? Ray.collider.GetComponentInParent<VRRig>() : null;
                    if (gunTarget && !gunTarget.IsLocal())
                    {
                        gunLocked = true;
                        lockTarget = gunTarget;
                    }
                }
            }
            else
            {
                if (gunLocked)
                {
                    gunLocked = false;
                    VRRig.LocalRig.enabled = true;
                }
            }
        }

        public static void HeadAll()
        {
            SerializePatch.OverrideSerialization = () => {
                MassSerialize(true, new[] { GorillaTagger.Instance.myVRRig.GetView });

                Vector3 archivePos = VRRig.LocalRig.transform.position;
                Quaternion archiveRot = VRRig.LocalRig.transform.rotation;

                Vector3 archivePosLeft = VRRig.LocalRig.leftHand.rigTarget.position;
                Quaternion archiveRotLeft = VRRig.LocalRig.leftHand.rigTarget.rotation;

                Vector3 archivePosRight = VRRig.LocalRig.rightHand.rigTarget.position;
                Quaternion archiveRotRight = VRRig.LocalRig.rightHand.rigTarget.rotation;

                Quaternion archiveHeadRot = VRRig.LocalRig.head.rigTarget.transform.rotation;

                foreach (NetPlayer Player in NetworkSystem.Instance.PlayerListOthers)
                {
                    VRRig targetRig = GetVRRigFromPlayer(Player);

                    if (!Buttons.GetIndex("Reverse Intercourse").enabled)
                    {
                        VRRig.LocalRig.transform.position = targetRig.transform.position + targetRig.transform.forward * (0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f) + targetRig.transform.up * -0.4f;
                        VRRig.LocalRig.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * 0.2f + targetRig.transform.up * -0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * -0.2f + targetRig.transform.up * -0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    }
                    else
                    {
                        VRRig.LocalRig.transform.position = targetRig.transform.position + targetRig.transform.forward * (0.2f + Mathf.Sin(Time.frameCount / 8f) * 0.1f) + targetRig.transform.up * 0.4f;
                        VRRig.LocalRig.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.leftHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * 0.2f + targetRig.transform.up * 0.4f;
                        VRRig.LocalRig.rightHand.rigTarget.transform.position = targetRig.transform.position + targetRig.transform.right * -0.2f + targetRig.transform.up * 0.4f;

                        VRRig.LocalRig.leftHand.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                        VRRig.LocalRig.rightHand.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));

                        VRRig.LocalRig.head.rigTarget.transform.rotation = Quaternion.Euler(targetRig.transform.rotation.eulerAngles + new Vector3(0f, 180f, 0f));
                    }

                    FixRigHandRotation();

                    SendSerialize(GorillaTagger.Instance.myVRRig.GetView, new RaiseEventOptions { TargetActors = new[] { Player.ActorNumber } });
                }

                RPCProtection();

                VRRig.LocalRig.transform.position = archivePos;
                VRRig.LocalRig.transform.rotation = archiveRot;

                VRRig.LocalRig.leftHand.rigTarget.position = archivePosLeft;
                VRRig.LocalRig.leftHand.rigTarget.rotation = archiveRotLeft;

                VRRig.LocalRig.rightHand.rigTarget.position = archivePosRight;
                VRRig.LocalRig.rightHand.rigTarget.rotation = archiveRotRight;

                VRRig.LocalRig.head.rigTarget.transform.rotation = archiveHeadRot;

                return false;
            };
        }

        public static void RemoveCopy()
        {
            gunLocked = false;
            lockTarget = null;
            VRRig.LocalRig.enabled = true;
        }

        public static void SpazHead()
        {
            if (VRRig.LocalRig.enabled)
            {
                VRRig.LocalRig.head.trackingRotationOffset.x = Random.Range(0f, 360f);
                VRRig.LocalRig.head.trackingRotationOffset.y = Random.Range(0f, 360f);
                VRRig.LocalRig.head.trackingRotationOffset.z = Random.Range(0f, 360f);
            }
            else
                VRRig.LocalRig.head.rigTarget.transform.rotation = RandomQuaternion();
        }

        public static float headspazDelay;
        public static bool headspazType;

        public static void RandomSpazHead()
        {
            if (headspazType)
            {
                SpazHead();
                if (Time.time > headspazDelay)
                {
                    headspazType = false;
                    headspazDelay = Time.time + Random.Range(1000f, 4000f) / 1000f;
                }
            }
            else
            {
                Fun.FixHead();
                if (Time.time > headspazDelay)
                {
                    headspazType = true;
                    headspazDelay = Time.time + Random.Range(200f, 1000f) / 1000f;
                }
            }
        }

        private static Vector3 headoffs = Vector3.zero;
        public static void EnableSpazHead() =>
            headoffs = VRRig.LocalRig.head.trackingPositionOffset;

        public static void SpazHeadPosition() =>
            VRRig.LocalRig.head.trackingPositionOffset = headoffs + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));

        public static void FixHeadPosition() =>
            VRRig.LocalRig.head.trackingPositionOffset = headoffs;

        public static void RandomSpazHeadPosition()
        {
            if (headspazType)
            {
                SpazHeadPosition();
                if (Time.time > headspazDelay)
                {
                    headspazType = false;
                    headspazDelay = Time.time + Random.Range(1000f, 4000f) / 1000f;
                }
            }
            else
            {
                FixHeadPosition();
                if (Time.time > headspazDelay)
                {
                    headspazType = true;
                    headspazDelay = Time.time + Random.Range(200f, 1000f) / 1000f;
                }
            }
        }

        public static float laggyRigDelay;
        public static void LaggyRig()
        {
            VRRig.LocalRig.enabled = false;
            ghostException = true;
            if (Time.time > laggyRigDelay)
            {
                VRRig.LocalRig.enabled = true;
                VRRig.LocalRig.PostTick();
                VRRig.LocalRig.enabled = false;

                laggyRigDelay = Time.time + 0.5f;
            }
        }

        public static bool wasRightPrimaryPressed;
        public static void UpdateRig()
        {
            VRRig.LocalRig.enabled = false;
            ghostException = true;
            if (rightPrimary && !wasRightPrimaryPressed)
            {
                VRRig.LocalRig.enabled = true;
                VRRig.LocalRig.PostTick();
                VRRig.LocalRig.enabled = false;
            }

            wasRightPrimaryPressed = rightPrimary;
        }
    }
}
