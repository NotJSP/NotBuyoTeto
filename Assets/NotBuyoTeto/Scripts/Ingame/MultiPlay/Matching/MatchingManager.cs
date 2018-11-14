using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using NotBuyoTeto.SceneManagement;
using NotBuyoTeto.Constants;

namespace NotBuyoTeto.Ingame.MultiPlay.Matching {
    public class MatchingManager : MonoBehaviourPunCallbacks {
        [SerializeField]
        private GameObject matchingWindow;
        [SerializeField]
        private Text messageLabel;
        [SerializeField]
        private Text statusLabel;
        [SerializeField]
        private Button cancelButton;
        [SerializeField]
        private Text startingCounter;
        
        private void Start() {
            matchingWindow.SetActive(true);
            matchingWindow.GetComponent<Animator>().Play(@"OpenWindow");
            startingCounter.text = "";
        }
        
        public void CancelMatching() {
        	StopAllCoroutines();
            matchingWindow.GetComponent<Animator>().Play(@"CloseWindow");
            
//            PhotonNetwork.Disconnect();
            SceneController.Instance.LoadScene(SceneName.Title, SceneTransition.Duration);
        }
    }
}