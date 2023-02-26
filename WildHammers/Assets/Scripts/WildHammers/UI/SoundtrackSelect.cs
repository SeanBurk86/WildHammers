
using TMPro;
using UnityCore.Audio;
using UnityCore.Data;
using UnityEngine;
using AudioType = UnityCore.Audio.AudioType;

namespace WildHammers
{
    namespace UI
    {
        public class SoundtrackSelect : MonoBehaviour
        {
            [SerializeField] private AudioType[] m_MusicTracks;
            private int m_SelectedIndex = 0;
            [SerializeField] private TMP_Text m_DisplayText;
            [SerializeField] private SelectDirectionButton m_Forward, m_Backward;

            private void Awake()
            {
                for (int i=0;i<m_MusicTracks.Length;i++)
                {
                    if (DataController.instance.VictoryMusic == m_MusicTracks[i].ToString())
                    {
                        m_SelectedIndex = i;
                    }
                }
            }

            private void OnEnable()
            {
                m_Forward.pressed += Forward;
                m_Backward.pressed += Backward;
            }

            private void Update()
            {
                m_DisplayText.text = m_MusicTracks[m_SelectedIndex].ToString();
            }

            private void OnDisable()
            {
                m_Forward.pressed -= Forward;
                m_Backward.pressed -= Backward;
            }

            private void Forward()
            {
                if (m_SelectedIndex < m_MusicTracks.Length - 1) m_SelectedIndex++;
                else m_SelectedIndex = 0;
                AudioController.instance.PlayAudio(m_MusicTracks[m_SelectedIndex]);
                DataController.instance.VictoryMusic = m_MusicTracks[m_SelectedIndex].ToString();
            }

            private void Backward()
            {
                if (m_SelectedIndex > 0) m_SelectedIndex--;
                else m_SelectedIndex = m_MusicTracks.Length - 1;
                AudioController.instance.PlayAudio(m_MusicTracks[m_SelectedIndex]);
                DataController.instance.VictoryMusic = m_MusicTracks[m_SelectedIndex].ToString();
            }
        }
    }
}

