using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

namespace SociaPol.Games
{
    public static class SociaPolPlayer
    {
        public static Player GetPlayerByActorNumberInRoom(int actorNumber, bool errorOnNull = true)
        {
            foreach (var playerItem in PhotonNetwork.PlayerList)
                if (playerItem.ActorNumber == actorNumber)
                    return playerItem;

            if (errorOnNull)
                throw new KeyNotFoundException($"Actor Number: {actorNumber}");
            return null;
        }

        public static Player GetPlayerByActorNumber(int actorNumber, IList<Player> playerList, bool errorOnNull = true)
        {
            foreach (var playerItem in playerList)
                if (playerItem.ActorNumber == actorNumber)
                    return playerItem;

            if (errorOnNull)
                throw new KeyNotFoundException($"Actor Number: {actorNumber}");
            return null;
        }
    }
}
