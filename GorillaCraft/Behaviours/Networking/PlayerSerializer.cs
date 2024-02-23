﻿using ExitGames.Client.Photon;
using GorillaCraft.Interfaces;
using GorillaCraft.Models;
using GorillaCraft.Tools;
using GorillaCraft.Utilities;
using Newtonsoft.Json;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GorillaCraft.Behaviours.Networking
{
    [DisallowMultipleComponent]
    public class PlayerSerializer : MonoBehaviourPun, IPhotonViewCallback, IOnPhotonViewPreNetDestroy
    {
        public static PlayerSerializer Local;

        private PhotonView View;
        private VRRig Rig;
        private readonly List<BlockGeneralInfo> BlockInfo = new();

        public void Start()
        {
            View = GetComponent<PhotonView>();  

            if (View.IsMine)
            {
                Local = this;
            }
            else
            {
                View.RPC(nameof(RequestBlocks), View.Owner, PhotonNetwork.LocalPlayer);
            }

            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;

            Rig = RigCacheUtils.GetField<VRRig>(View.Owner);
        }

        public void DistributeBlock(bool isCreating, IBlock block, Vector3 blockPosition, Vector3 blockEuler, Vector3 blockScale)
        {
            MultiplayerManager.BlockInteraction(isCreating, block != null ? block.GetType().Name : "None", blockPosition, blockEuler, blockScale);

            if (isCreating)
            {
                BlockInfo.Add(new BlockGeneralInfo()
                {
                    Name = block.GetType().Name,
                    Position = blockPosition,
                    Euler = blockEuler,
                    Scale = blockScale
                });
            }
            else
            {
                BlockGeneralInfo info = BlockInfo.FirstOrDefault(info => info.Position == blockPosition);
                if (info != null)
                {
                    BlockInfo.Remove(info);
                }
            }
        }

        public void OnEvent(EventData data)
        {
            if (data.Code != MultiplayerManager.BlockInteractionCode && data.Code != MultiplayerManager.SurfaceTapCode) return;

            var sender = PhotonNetwork.CurrentRoom.GetPlayer(data.Sender);
            var eventData = (object[])data.CustomData;

            if (data.Code == MultiplayerManager.BlockInteractionCode)
            {
                if (sender == View.Owner)
                {
                    if ((bool)eventData[0])
                    {
                        GorillaLocomotion.Player.Instance.GetComponent<BlockHandler>().PlaceBlock(BlockPlaceType.Server, (string)eventData[1], (Vector3)eventData[2], (Vector3)eventData[3], (Vector3)eventData[4], View.Owner);
                    }
                    else
                    {
                        GorillaLocomotion.Player.Instance.GetComponent<BlockHandler>().RemoveBlock((Vector3)eventData[2], View.Owner);
                    }
                }
            }
            else if (data.Code == MultiplayerManager.SurfaceTapCode)
            {
                if (sender == View.Owner)
                {
                    Type surfaceType = typeof(Plugin).Assembly.GetTypes().First(type => type.Name == (string)eventData[0]);
                    GorillaLocomotion.Player.Instance.GetComponent<BlockHandler>().PlayTapSound(Rig, surfaceType, (bool)eventData[1]);
                }
            }
        }

        [PunRPC]
        public void RequestBlocks(Player player, PhotonMessageInfo info)
        {
            if (View.IsMine)
            {
                List<string> blocks = new();

                foreach (var block in BlockInfo)
                {
                    if (blocks.Count < 10)
                    {
                        blocks.Add(JsonConvert.SerializeObject(block, Formatting.None));
                        continue;
                    }

                    View.RPC(nameof(ServerRecoverBlocks), player, new object[] { blocks.ToArray() });
                    blocks.Clear();
                    blocks.Add(JsonConvert.SerializeObject(block, Formatting.None));
                }

                if (blocks.Count > 0)
                {
                    View.RPC(nameof(ServerRecoverBlocks), player, new object[] { blocks.ToArray() });
                }
            }
        }

        [PunRPC]
        public void ServerRecoverBlocks(string[] blocks, PhotonMessageInfo info)
        {
            if (info.photonView.Owner == info.Sender)
            {
                foreach (string block in blocks)
                {
                    BlockGeneralInfo blockInfo = JsonConvert.DeserializeObject<BlockGeneralInfo>(block);
                    GorillaLocomotion.Player.Instance.GetComponent<BlockHandler>().PlaceBlock(BlockPlaceType.Recovery, blockInfo.Name, blockInfo.Position, blockInfo.Euler, blockInfo.Scale, info.Sender);
                }
            }
        }

        public void OnPreNetDestroy(PhotonView rootView)
        {
            if (Local == this)
            {
                Local = null;
            }

            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }
    }
}
