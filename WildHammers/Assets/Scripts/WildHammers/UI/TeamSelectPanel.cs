
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using WildHammers.Player;
using WildHammers.Team;

namespace WildHammers
{
    namespace UI
    {
        public class TeamSelectPanel : MonoBehaviour
        {
            public TeamController.TeamInfo[] teamList;
            public GameObject teamButtonPrefab;
            public List<MultiplayerEventSystem> playerList;
            public GameObject buttonPanel;
            

            private void OnEnable()
            {
                teamList = TeamController.instance.teams;
                playerList = PlayerJoinController.instance.multiplayerEventSystems;
                ConfigureButtons();
            }

            private void OnDisable()
            {
                foreach (Transform child in buttonPanel.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            private void ConfigureButtons()
            {
                int playerListIncrementer = 0;
                foreach (var _teamInfo in teamList)
                {
                    GameObject _newButton = Instantiate(teamButtonPrefab, buttonPanel.transform);
                    TeamSelectButton _teamButton = _newButton.GetComponent<TeamSelectButton>();
                    _teamButton.team = _teamInfo;
                    if (playerListIncrementer < playerList.Count)
                    {
                        EventSystem _eventSystem = playerList[playerListIncrementer];
                        _eventSystem.SetSelectedGameObject(_teamButton.gameObject);
                        playerListIncrementer++;
                    }
                }

                while (playerListIncrementer < playerList.Count)
                {
                    EventSystem _eventSystem = playerList[playerListIncrementer];
                    _eventSystem.SetSelectedGameObject(GetComponentInChildren<TeamSelectButton>().gameObject);
                    playerListIncrementer++;
                }
            }
            
        }
    }
}

