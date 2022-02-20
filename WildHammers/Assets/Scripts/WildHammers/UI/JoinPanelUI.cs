
using TMPro;
using UnityCore.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WildHammers.Player;

namespace WildHammers
{
    namespace UI
    {
        public class JoinPanelUI : MonoBehaviour
        {
            public bool debug;

            public GameObject firstSelectedInitialsButton;
            
            [SerializeField] PlayerInfo playerInfo = null;
            [SerializeField] private Button submitInitialsButton;
            [SerializeField] private Button deleteCharacterButton;
            [SerializeField] private TMP_Text playerPromptDisplay;
            [SerializeField] private TMP_Text joinPhaseText;
            [SerializeField] private GameObject initialsPanel;
            [SerializeField] private GameObject zodiacPanel;
            [SerializeField] private TMP_Text playerReadyText;
            [SerializeField] private EventSystem eventSystem;
            [SerializeField] private GameObject postDeleteSelectButton;
            [SerializeField] private GameObject firstSelectedZodiacButton;

            private bool hasSubmittedInitials;
            private bool hasSubmittedZodiac;
            public string playerNameInput = "";


            #region Unity Functions

                private void OnEnable()
                {
                    hasSubmittedInitials = false;
                    hasSubmittedZodiac = false;
                    joinPhaseText.text = "Initials";
                    playerPromptDisplay.text = "Enter your initials!";
                }
                
                private void Update()
                {
                    if (playerNameInput.Length <= 0)
                    {
                        submitInitialsButton.interactable = false;
                        deleteCharacterButton.interactable = false;
                        playerPromptDisplay.text = "Enter your initials!";
                    }
                    else if (!hasSubmittedInitials && playerNameInput.Length>0)
                    {
                        deleteCharacterButton.interactable = true;
                        submitInitialsButton.interactable = true;
                        playerPromptDisplay.text = playerNameInput;
                    }
                    else if (hasSubmittedInitials && !hasSubmittedZodiac)
                    {
                        initialsPanel.SetActive(false);
                        zodiacPanel.SetActive(true);
                        playerPromptDisplay.text = "Your initials are: "+playerInfo.playerInitials+"\n"
                                                   +"Now pick your zodiac sign!";
                        joinPhaseText.text = "Zodiac";
                    }
                    else if (hasSubmittedInitials && hasSubmittedZodiac)
                    {
                        zodiacPanel.SetActive(false);
                        playerReadyText.gameObject.SetActive(true);
                        playerPromptDisplay.text = playerInfo.playerInitials + " the " + playerInfo.zodiacSign;
                        playerReadyText.text = DataController.instance.GetPlayerMatchesWon(playerInfo.GetID())
                                               +"-"+DataController.instance.GetPlayerMatchesLost(playerInfo.GetID())+"-"+
                                               DataController.instance.GetPlayerMatchesDrawn(playerInfo.GetID())+
                                               "\n"+"-----"+
                                                "\n" +"Total Goals Scored: "+ DataController.instance.GetPlayerTotalGoals(playerInfo.GetID())
                                               + "\n" 
                                               + "\n" + "Goals Scored For Team: " + DataController.instance.GetPlayerGoalsScoredForTeam(playerInfo.GetID())
                                               + "\n" 
                                               + "\n" + "Goals Scored On Self: " + DataController.instance.GetPlayerGoalsScoredOnSelf(playerInfo.GetID());
                        joinPhaseText.text = "Here we go!";
                    }
                }
            

            #endregion

            #region Public Functions

                public void AddLetter(string _letter)
                {
                    if (playerNameInput.Length < 3)
                    {
                        playerNameInput += _letter;
                    }
                    else if(playerNameInput.Length>=3)
                    {
                        Log("Can't add anymore characters to this player's initials");
                    }
                }

                public void DeleteLetter()
                {
                    if (playerNameInput.Length > 0)
                    {
                        playerNameInput = playerNameInput.Remove(playerNameInput.Length-1);
                        if(playerNameInput.Length==0) eventSystem.SetSelectedGameObject(postDeleteSelectButton);
                    }
                    else
                    {
                        Log("Can't take away anymore characters from this player's initials");
                    }
                }

                public void SubmitInitials()
                {
                    hasSubmittedInitials = playerInfo.SetInitials(playerNameInput);
                    if (hasSubmittedInitials)
                    {
                        Log("Successfully submitted initials");
                        eventSystem.SetSelectedGameObject(firstSelectedZodiacButton);
                    }
                }

                public void SubmitZodiac(int _index)
                {
                    hasSubmittedZodiac = playerInfo.SetZodiacSign(_index);
                    if (hasSubmittedZodiac)
                    {
                        Log("Successfully submitted zodiac");
                        PlayerJoinController.instance.IncrementReadyCount();
                    }
                }

                public void ClearPlayerInfo()
                {
                    hasSubmittedInitials = false;
                    hasSubmittedZodiac = false;
                    joinPhaseText.text = "Initials";
                    playerPromptDisplay.text = "Enter your initials!";
                }

            #endregion

            #region Private Functions

                private void Log(string _msg)
                {
                    if(!debug) return;
                    Debug.Log("[JoinPanelUI]: "+_msg);
                }
                
            #endregion

        }
        
    }
}

