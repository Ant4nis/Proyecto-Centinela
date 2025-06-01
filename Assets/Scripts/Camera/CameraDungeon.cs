using System;
using Managers;
using Unity.Cinemachine;
using UnityEngine;

namespace Camera
{
    public class CameraDungeon : MonoBehaviour
    {
        private CinemachineCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineCamera>();
        }

        private void Start()
        {
            //camera.Follow = LevelManager.Instance.SelectedPlayer.transform;
        }
    }
}