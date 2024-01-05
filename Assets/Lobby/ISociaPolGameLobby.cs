using System;
using System.Collections.Generic;

using Photon.Realtime;

namespace SociaPol.Games
{
    public interface ISociaPolGameLobby
    {
        Action<ISociaPolGameLobby> OnMaxParticipantsChanged { get; set; }
        Action<ISociaPolGameLobby> OnOwnerChanged { get; set; }
        Action<ISociaPolGameLobby> OnDisbanded { get; set; }
        Action<ISociaPolGameLobby> OnInitialized { get; set; }

        Action<ISociaPolGameLobby, Player> OnParticipantJoined { get; set; }
        Action<ISociaPolGameLobby, Player> OnParticipantLeft { get; set; }

        int MaxParticipantCount { get; set; }

        Player Owner { get; }
        IReadOnlyList<Player> Participants { get; }

        void Join(Player player);
        void Leave(Player player);
        void Disband();
    }
}
