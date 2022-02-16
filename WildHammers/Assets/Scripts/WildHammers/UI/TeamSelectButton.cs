
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WildHammers.Match;
using WildHammers.Player;
using WildHammers.Team;

namespace WildHammers
{
    namespace UI
    {
        public class TeamSelectButton : Button
        {
            public TeamController.TeamInfo team;
            
            [SerializeField]
            private GameObject[] playerSelectIDUIs;

            private List<PlayerInfo> selectingPlayerInfos = new List<PlayerInfo>();

            private bool buttonHasSubmitted;

            protected override void Start()
            {
                TMP_Text buttonText = GetComponentInChildren<TMP_Text>();
                buttonText.text = team.city + " " + team.name;
                image.sprite = team.livery;
                buttonHasSubmitted = false;
            }

            public override void OnSelect(BaseEventData eventData)
            {
                base.OnSelect(eventData);
                GameObject selectingGameObject = eventData.currentInputModule.transform.gameObject;
                PlayerInfo selectingPlayerInfo = selectingGameObject.GetComponent<PlayerInfo>();
                int selectingGOPlayerIndex = selectingGameObject.GetComponent<PlayerInput>().playerIndex;
                playerSelectIDUIs[selectingGOPlayerIndex].gameObject.GetComponentInChildren<TMP_Text>().text 
                    = selectingPlayerInfo.playerInitials;
                playerSelectIDUIs[selectingGOPlayerIndex].SetActive(true);
                selectingPlayerInfos.Add(selectingPlayerInfo);
            }

            public override void OnDeselect(BaseEventData eventData)
            {
                base.OnDeselect(eventData);
                GameObject deselectingGameObject = eventData.currentInputModule.transform.gameObject;
                PlayerInfo deselectingPlayerInfo = deselectingGameObject.GetComponent<PlayerInfo>();
                int deselectingGOPlayerIndex = deselectingGameObject.GetComponent<PlayerInput>().playerIndex;
                playerSelectIDUIs[deselectingGOPlayerIndex].SetActive(false);
                selectingPlayerInfos.Remove(deselectingPlayerInfo);
            }

            public override void OnSubmit(BaseEventData eventData)
            {
                base.OnSubmit(eventData);
                if ((selectingPlayerInfos.Count == 2 && buttonHasSubmitted == false) 
                    || (PlayerJoinController.instance.maxPlayerCount == 2 && buttonHasSubmitted == false) )
                {
                    PlayerInfo[] _roster;
                    if (PlayerJoinController.instance.maxPlayerCount == 4)
                        _roster = TeamController.instance.MatchPlayers(selectingPlayerInfos[0],
                            selectingPlayerInfos[1]);
                    else _roster = new PlayerInfo[] {selectingPlayerInfos[0]};
                    TeamController.MatchTeam _matchTeam = TeamController.instance.MatchTeamToPlayers(team, _roster);
                    MatchController.instance.AddTeamToMatch(_matchTeam);
                    GameObject selectingGameObject = eventData.currentInputModule.transform.gameObject;
                    PlayerInfo selectingPlayerInfo = selectingGameObject.GetComponent<PlayerInfo>();
                    int selectingGOPlayerIndex = selectingGameObject.GetComponent<PlayerInput>().playerIndex;
                    playerSelectIDUIs[selectingGOPlayerIndex].gameObject.GetComponentInChildren<TMP_Text>().text 
                        = selectingPlayerInfo.playerInitials;
                    playerSelectIDUIs[selectingGOPlayerIndex].SetActive(true);
                    interactable = false;
                    if (PlayerJoinController.instance.maxPlayerCount == 4)
                    {
                        Navigation noneModeNav = new Navigation();
                        noneModeNav.mode = Navigation.Mode.None;
                        navigation = noneModeNav;
                    }
                    buttonHasSubmitted = true;
                }
            }
        
    }
}

    
}
