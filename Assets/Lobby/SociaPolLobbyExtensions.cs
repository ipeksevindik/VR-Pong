using Photon.Pun;
using Photon.Realtime;

namespace SociaPol.Games
{
    public static class SociaPolLobbyExtensions
    {
        public static bool IsOwner(this ISociaPolGameLobby lobby, bool nullValue = true)
        {
            if (lobby.Owner is null)
                return nullValue;

            return lobby.Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber;
        }

        public static bool IsOwner(this ISociaPolGameLobby lobby, Player player)
        {
            if (lobby.Owner is null)
                return false;

            return lobby.Owner.ActorNumber == player.ActorNumber;
        }

        public static bool IsFull(this ISociaPolGameLobby lobby)
            => lobby.Participants.Count >= lobby.MaxParticipantCount;

        public static bool IsParticipating(this ISociaPolGameLobby lobby, Player player)
        {
            foreach (var participant in lobby.Participants)
                if (participant.ActorNumber == player.ActorNumber)
                    return true;
            return false;
        }
    }
}
