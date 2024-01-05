using System;
using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;

using UnityEngine;
using UnityEngine.Events;

namespace SociaPol.Games
{
    public class SociaPolGameLobby : MonoBehaviourPunCallbacks, ISociaPolGameLobby
    {
        [SerializeField] private UnityEvent OnFullUnityEvent = null;
        [SerializeField] private UnityEvent OnHasVacancyUnityEvent = null;

        [SerializeField] private UnityEvent OnInitializedUnityEvent = null;
        [SerializeField] private UnityEvent OnDisbandedUnityEvent = null;

        [SerializeField] private UnityEvent OnParticipantJoinedUnityEvent = null;
        [SerializeField] private UnityEvent OnParticipantLeftUnityEvent = null;

        private const int MAX_ALLOWED_PARTICIPANT_COUNT = 8;

        public Action<ISociaPolGameLobby> OnMaxParticipantsChanged { get; set; }
        public Action<ISociaPolGameLobby> OnOwnerChanged { get; set; }
        public Action<ISociaPolGameLobby> OnInitialized { get; set; }
        public Action<ISociaPolGameLobby> OnDisbanded { get; set; }
        public Action<ISociaPolGameLobby, Player> OnParticipantJoined { get; set; }
        public Action<ISociaPolGameLobby, Player> OnParticipantLeft { get; set; }

        [SerializeField, Range(1, MAX_ALLOWED_PARTICIPANT_COUNT)] private int _maxParticipants = 2;

        private Player _owner = null;

        protected List<Player> participants = new List<Player>(16);


        public IReadOnlyList<Player> Participants => participants;

        public Player Owner
        {
            get => _owner;
            protected set => photonView.RPC(nameof(ForceOwnerChangeRPC), RpcTarget.AllBufferedViaServer, value?.ActorNumber);
        }

        public int MaxParticipantCount
        {
            get => _maxParticipants;
            set
            {
                if (this.IsOwner(nullValue: false))
                    return;

                photonView.RPC(nameof(ChangeMaxParticipantCountRPC), RpcTarget.AllBuffered, value);
            }
        }

        [ContextMenu("Join Local")]
        private void JoinLocal() => Join(PhotonNetwork.LocalPlayer);

        [ContextMenu("Leave Local")]
        private void LeaveLocal() => Leave(PhotonNetwork.LocalPlayer);

        private void Awake()
        {
            OnOwnerChanged += (_1) => Debug.Log($"Owner: {Owner?.NickName ?? "null"}", gameObject);
            OnParticipantJoined += (_1, _2) => Debug.Log($"Joined: {_2.NickName}", gameObject);
            OnParticipantLeft += (_1, _2) => Debug.Log($"Left: {_2.NickName}", gameObject);
        }

        public void Join(Player player)
        {
            if (player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                return;

            if (participants.Count == 0)
                Owner = player;

            if (participants.Count >= MaxParticipantCount)
                return;

            photonView.RPC(nameof(JoinRPC), RpcTarget.AllBufferedViaServer, player.ActorNumber);
        }

        public void Leave(Player player)
        {
            if (player.ActorNumber != PhotonNetwork.LocalPlayer.ActorNumber)
                return;

            photonView.RPC(nameof(LeaveRPC), RpcTarget.AllBufferedViaServer, player.ActorNumber);
        }

        public void Disband()
        {
            if (this.IsOwner(nullValue: true))
                return;

            for (int i = participants.Count; i >= 0; i--)
                photonView.RPC(nameof(LeaveRPC), RpcTarget.AllViaServer, participants[i].ActorNumber);
            PhotonNetwork.OpCleanRpcBuffer(photonView);
        }

        [PunRPC]
        private void LeaveRPC(int actorNumber)
        {
            Player player = SociaPolPlayer.GetPlayerByActorNumber(actorNumber, participants, errorOnNull: false);

            if (player is null)
                return;

            if (!participants.Remove(player))
                return;

            if (player.ActorNumber == Owner.ActorNumber)
                if (participants.Count == 0)
                    Owner = null;
                else
                    Owner = participants[0];

            OnParticipantLeft?.Invoke(this, player);
            OnParticipantLeftUnityEvent?.Invoke();

            if (participants.Count == MaxParticipantCount - 1)
                OnHasVacancyUnityEvent?.Invoke();
            else if (participants.Count == 0)
            {
                OnDisbanded?.Invoke(this);
                OnDisbandedUnityEvent?.Invoke();
            }
        }

        [PunRPC]
        private void JoinRPC(int actorNumber)
        {
            if (participants.Count >= MaxParticipantCount)
                return;

            Player player = SociaPolPlayer.GetPlayerByActorNumberInRoom(actorNumber);

            if (participants.Contains(player))
                return;

            if (participants.Count == 0)
                Owner = player;

            participants.Add(player);
            OnParticipantJoined?.Invoke(this, player);
            OnParticipantJoinedUnityEvent?.Invoke();

            if (participants.Count == MaxParticipantCount)
                OnFullUnityEvent?.Invoke();
            else if (participants.Count == 1)
            {
                OnInitialized?.Invoke(this);
                OnInitializedUnityEvent?.Invoke();
            }
        }

        [PunRPC]
        private void ForceOwnerChangeRPC(int? actorNumber)
        {
            Player newOwner = null;

            if (actorNumber.HasValue)
                newOwner = SociaPolPlayer.GetPlayerByActorNumberInRoom(actorNumber.Value);

            if (_owner == newOwner)
                return;

            _owner = newOwner;

            OnOwnerChanged?.Invoke(this);
        }

        [PunRPC]
        private void ChangeMaxParticipantCountRPC(int value)
        {
            value = Math.Clamp(value, participants.Count, MAX_ALLOWED_PARTICIPANT_COUNT);

            if (value == _maxParticipants)
                return;

            _maxParticipants = value;
            OnMaxParticipantsChanged?.Invoke(this);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
            => photonView.RPC(nameof(LeaveRPC), RpcTarget.AllBufferedViaServer, otherPlayer.ActorNumber);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (MaxParticipantCount == _maxParticipants)
                return;

            MaxParticipantCount = _maxParticipants;
        }
#endif
    }
}
