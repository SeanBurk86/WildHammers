
using System;
using System.Collections.Generic;
using System.Linq;
using UnityCore.Audio;
using UnityEngine;
using UnityEngine.InputSystem;
using WildHammers.Player;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace GameplayObjects
    {
        public class GoalTendingZone : MonoBehaviour
        {
            public bool debug;
            
            [SerializeField] private float m_EjectionForce = 1500f;
            [SerializeField] private float m_GoalTendingClock = 3f;


            private Dictionary<int,GoalTendingTimer> m_OffenderTable = new Dictionary<int, GoalTendingTimer>();

            private Vector2 m_EjectionVector;

            public class GoalTendingTimer
            {
                public float currentTime { get; set; }

                public GoalTendingTimer(float _initTime)
                {
                    currentTime = _initTime;
                }
            }

            private void Start()
            {
                if (transform.parent.GetComponentInChildren<Goal>().goalType == GoalType.WEST)
                {
                    m_EjectionVector = Vector2.left;
                } 
                else if (transform.parent.GetComponentInChildren<Goal>().goalType == GoalType.EAST)
                {
                    m_EjectionVector = Vector2.right;
                }
                else
                {
                    LogWarning("The attached goal is not assigned to a team");
                }
            }

            private void OnTriggerEnter2D(Collider2D _other)
            {
                // if the Collider is the pommel
                if (_other.attachedRigidbody.gameObject.CompareTag("Pommel"))
                {
                    // get the playerindex
                    int _offendingIndex = _other.transform.parent.parent.GetComponent<PlayerInput>().playerIndex;
                    m_OffenderTable.Add(_offendingIndex,new GoalTendingTimer(m_GoalTendingClock));
                    Log("Added playerIndex "+_offendingIndex+" to offending index");
                }
            }

            private void Update()
            {
                if (m_OffenderTable != null)
                {
                    //iterate through the offender table
                    foreach (KeyValuePair<int,GoalTendingTimer> _pair in m_OffenderTable)
                    {
                        //decrement their clock by Time.deltaTime
                        _pair.Value.currentTime -= Time.deltaTime;
                        //if offender's float is <= 0
                        if (_pair.Value.currentTime <= 0f)
                        {
                            // Shove the hammer
                            ShoveGoalTendingHammers(_pair.Key);
                        }
                    }
                    
                }
            }

            private void OnTriggerExit2D(Collider2D _other)
            {
                if (_other.attachedRigidbody.gameObject.CompareTag("Pommel"))
                {
                    // get the playerindex
                    int _leavingIndex = _other.transform.parent.parent.GetComponent<PlayerInput>().playerIndex;
                    m_OffenderTable.Remove(_leavingIndex);
                    Log("Removed playerIndex "+_leavingIndex+" to offending index");
                }
            }


            private void ShoveGoalTendingHammers(int _penalizedIndex)
            {
                List<PlayerInput> _playerInputs =  PlayerJoinController.instance.playerList;
                for (int i = 0; i < _playerInputs.Count; i++)
                {
                    if (_playerInputs[i].playerIndex == _penalizedIndex)
                    {
                        GameObject _penalizedHammer = _playerInputs[i].transform.GetChild(0).gameObject;
                        Rigidbody2D[] _penalizedRBs = _penalizedHammer.GetComponentsInChildren<Rigidbody2D>();
                        for (int j = 0; j < _penalizedRBs.Length; j++)
                        {
                            _penalizedRBs[j].AddForce(m_EjectionVector*m_EjectionForce, ForceMode2D.Impulse);
                        }
                    }
                }
                AudioController.instance.PlayAudio(AudioType.SFX_07);
            }

            private void Log(string _msg)
            {
                if(debug)
                    Debug.Log("[GoalTendingZone]: "+_msg);
            }
            
            private void LogWarning(string _msg)
            {
                if(debug)
                    Debug.LogWarning("[GoalTendingZone]: "+_msg);
            }
        }
        
    }
}

