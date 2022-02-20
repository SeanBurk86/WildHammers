
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WildHammers.GameplayObjects;

namespace WildHammers
{
    namespace UI
    {
        public class ArenaSelect : MonoBehaviour
        {
            [SerializeField] private ArenaType[] m_Arenas;
            private int m_SelectedIndex;
            [SerializeField] private TMP_Text m_DisplayText;
            [SerializeField] private SelectDirectionButton m_Forward, m_Backward;

            private void Awake()
            {
                m_SelectedIndex = 0;
            }

            private void OnEnable()
            {
                m_Forward.pressed += Forward;
                m_Backward.pressed += Backward;
            }

            private void Update()
            {
                m_DisplayText.text = m_Arenas[m_SelectedIndex].ToString();
            }

            private void OnDisable()
            {
                m_Forward.pressed -= Forward;
                m_Backward.pressed -= Backward;
            }

            private void Forward()
            {
                if (m_SelectedIndex < m_Arenas.Length - 1) m_SelectedIndex++;
                else m_SelectedIndex = 0;
            }

            private void Backward()
            {
                if (m_SelectedIndex > 0) m_SelectedIndex--;
                else m_SelectedIndex = m_Arenas.Length - 1;
            }
            
            private void Log(string _msg)
            {
                Debug.Log("[ArenaSelectPanel]: "+_msg);
            }
            
            
        }
        
    }
}

