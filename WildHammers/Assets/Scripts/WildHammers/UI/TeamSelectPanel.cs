
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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
            public List<PlayerInput> playerList;
            public GameObject buttonPanel;
            

            private void Start()
            {
                teamList = TeamController.instance.teams;
                playerList = PlayerJoinController.instance.playerList;
                ConfigureButtons();
            }
            
            private void ConfigureButtons()
            {
                int playerListIncrementer = 0;
                foreach (var _teamInfo in teamList)
                {
                    GameObject newButton = Instantiate(teamButtonPrefab, buttonPanel.transform);
                    TeamSelectButton teamButton = newButton.GetComponent<TeamSelectButton>();
                    teamButton.team = _teamInfo;
                    if (playerListIncrementer < playerList.Count)
                    {
                        PlayerInput _playerInput = playerList[playerListIncrementer];
                        EventSystem _eventSystem = _playerInput.GetComponent<EventSystem>();
                        _eventSystem.SetSelectedGameObject(teamButton.gameObject);
                        playerListIncrementer++;
                    }
                }
            }
            
        }
    }
}
