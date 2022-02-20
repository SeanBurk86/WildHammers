
using System.Collections;
using System.Collections.Generic;
using UnityCore.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using WildHammers.Player;

namespace WildHammers
{
    namespace ScreenFlow
    {
        public class ScreenFlowController : MonoBehaviour
        {
            public static ScreenFlowController instance;

            public bool debug;

            public ScreenPose[] poses;

            public ScreenPoseType entryScreenPose;

            public ScreenPoseType currentPoseType;

            private Hashtable m_Poses;

            #region Unity Functions

            private void Awake()
            {
                if(!instance) Configure();
                else Destroy(gameObject);
            }

            private void Start()
            {
                Flow(entryScreenPose);
            }

            #endregion

            #region Public Functions

            public void Flow(ScreenPoseType _incomingPoseType, bool _waitForExit = true, bool _reload = false)
            {
                Log("Attempting to flow to "+_incomingPoseType);
                if (_incomingPoseType == ScreenPoseType.None)
                {
                    return;
                } 
                if (!PoseExists(_incomingPoseType))
                {
                    LogWarning("You are trying to turn on page ["+_incomingPoseType+"] which is not registered.");
                    return;
                }

                ScreenPose _incomingPose = GetPose(_incomingPoseType);

                if (currentPoseType == _incomingPoseType && !_reload)
                {
                    LogWarning("Trying to flow to current pose from itself without reload");
                    return;
                }
                else if (currentPoseType != _incomingPoseType && _reload)
                {
                    LogWarning("Trying to flow to another pose with reload enabled");
                    return;
                }
                
                if(!_reload) TransitionPages(_incomingPose, _waitForExit);
                
                SetInitialPoseElements(_incomingPose);

                SelectFirstElement(_incomingPose);

                currentPoseType = _incomingPoseType;
            }


            #endregion

            #region Private Functions
            
            private static void SelectFirstElement(ScreenPose _incomingPose)
            {
                if (_incomingPose.firstSelectedElement != null)
                {
                    {
                        foreach (PlayerInput _playerInput in PlayerJoinController.instance.playerList)
                        {
                            _playerInput.SwitchCurrentActionMap("UI");
                            MultiplayerEventSystem _eventSystem = _playerInput.transform.GetComponent<MultiplayerEventSystem>();
                            _eventSystem.SetSelectedGameObject(_incomingPose.firstSelectedElement);
                        }
                    }
                }
            }
            
            private void TransitionPages(ScreenPose _incomingScreenPose, bool _waitForExit)
            {
                
                ScreenPose _currentPose = GetPose(currentPoseType);
                
                //Determine non-shared pages between poses
                List<PageType> _pagesToTurnOff = new List<PageType>();

                if (_currentPose != null)
                {
                    for (int i = 0; i < _currentPose.pages.Length; i++)
                    {
                        bool isMatch = false;
                        for (int j = 0; j < _incomingScreenPose.pages.Length; j++)
                        {
                            if (_currentPose.pages[i] == _incomingScreenPose.pages[j]) isMatch = true;
                        }

                        //Add the non-shared pages to a list
                        if (!isMatch) _pagesToTurnOff.Add(_currentPose.pages[i]);
                    }
                }

                //Do the same for the incoming screen pose
                List<PageType> _pagesToTurnOn = new List<PageType>();
                if (_currentPose != null)
                {
                    for (int i = 0; i < _incomingScreenPose.pages.Length; i++)
                    {
                        bool isMatch = false;
                        for (int j = 0; j < _currentPose.pages.Length; j++)
                        {
                            if (_currentPose.pages[j] == _incomingScreenPose.pages[i]) isMatch = true;
                        }

                        if (!isMatch) _pagesToTurnOn.Add(_incomingScreenPose.pages[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < _incomingScreenPose.pages.Length; i++)
                    {
                        _pagesToTurnOn.Add(_incomingScreenPose.pages[i]);
                    }
                }

                if (_pagesToTurnOn.Count < 1)
                {
                    if (_pagesToTurnOff.Count > 0)
                    {
                        for (int i = 0; i < _pagesToTurnOff.Count; i++)
                        {
                            PageController.instance.TurnPageOff(_pagesToTurnOff[i], _waitForExit: _waitForExit);
                        }
                    }

                }
                else
                {
                    if (_pagesToTurnOff.Count > 0)
                    {
                        for (int i = 0; i < _pagesToTurnOff.Count; i++)
                        {
                            //If it's the last one switch to turning pages on
                            if (i == _pagesToTurnOff.Count - 1)
                            {
                                PageController.instance.TurnPageOff(_pagesToTurnOff[i], _pagesToTurnOn[0], _waitForExit);
                                if (_incomingScreenPose.pages.Length > 1)
                                {
                                    for (int j = 1; j < _pagesToTurnOn.Count; j++)
                                    {
                                        PageController.instance.TurnPageOn(_pagesToTurnOn[j]);
                                    }
                                }
                            }
                            else
                            {
                                PageController.instance.TurnPageOff(_pagesToTurnOff[i], _waitForExit: _waitForExit);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < _pagesToTurnOn.Count; j++)
                        {
                            PageController.instance.TurnPageOn(_pagesToTurnOn[j]);
                        }
                    }
                    
                }

            }
            
            private void SetInitialPoseElements(ScreenPose _incomingScreenPose)
            {
                if (_incomingScreenPose.initialInactiveElements != null)
                {
                    foreach (GameObject _element in _incomingScreenPose.initialInactiveElements)
                    {
                        Log("Deactivating "+_element.name);
                        _element.SetActive(false);
                    }
                }

                //turn on the initialActiveElements
                if (_incomingScreenPose.initialActiveElements != null)
                {
                    foreach (GameObject _element in _incomingScreenPose.initialActiveElements)
                    {
                        Log("Activating "+_element.name);
                        _element.SetActive(true);
                    }
                }
            }

            private ScreenPose GetPose(ScreenPoseType _type)
            {
                if (!PoseExists(_type))
                {
                    LogWarning("You are trying to get page ["+_type+"] that has not been registered.");
                    return null;
                }

                return (ScreenPose) m_Poses[_type];
            }

            private void RegisterAllPoses()
            {
                foreach (ScreenPose _pose in poses)
                {
                    RegisterPose(_pose);
                }
            }

            private void RegisterPose(ScreenPose _pose)
            {
                if (_pose == null) return;
                if (PoseExists(_pose.type))
                {
                    LogWarning("You are trying to register page ["+_pose.type+"] that has already been registered: "+_pose.gameObject.name);
                }
                
                m_Poses.Add(_pose.type, _pose);
                Log("Registered new page ["+_pose.type+"]");
            }

            private bool PoseExists(ScreenPoseType _pose)
            {
                return m_Poses.Contains(_pose);
            }

            private void Configure()
            {
                instance = this;
                m_Poses = new Hashtable();
                RegisterAllPoses();

                DontDestroyOnLoad(gameObject);
            }

            private void Log(string _msg)
            {
                if (debug) Debug.Log("[ScreenFlowController]: " + _msg);
            }

            private void LogWarning(string _msg)
            {
                if (debug) Debug.LogWarning("[ScreenFlowController]: " + _msg);
            }


            #endregion
        }
    }
}

