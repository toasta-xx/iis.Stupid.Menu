using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

namespace iiMenu.Utilities
{
    public static class NetworkHelpers
    {
        public static void RaiseCommand(byte eventCode, string command, RaiseEventOptions options, params object[] parameters)
        {
            if (!NetworkSystem.Instance.InRoom)
                return;

            PhotonNetwork.RaiseEvent(eventCode,
                new object[] { command }
                    .Concat(parameters)
                    .ToArray(),
                options, SendOptions.SendReliable);
        }

        public static void RaiseCommand(byte eventCode, string command, int[] targets, params object[] parameters) =>
            RaiseCommand(eventCode, command, new RaiseEventOptions { TargetActors = targets }, parameters);

        public static void RaiseCommand(byte eventCode, string command, int target, params object[] parameters) =>
            RaiseCommand(eventCode, command, new RaiseEventOptions { TargetActors = new[] { target } }, parameters);

        public static void RaiseCommand(byte eventCode, string command, ReceiverGroup target, params object[] parameters) =>
            RaiseCommand(eventCode, command, new RaiseEventOptions { Receivers = target }, parameters);
    }
}
