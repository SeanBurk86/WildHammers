
using UnityCore.Menu;
using UnityEngine;

namespace WildHammers
{
    namespace ScreenFlow
    {
        public class ScreenPose : MonoBehaviour
        {
            public ScreenPoseType type;
            public GameObject[] initialActiveElements;
            public GameObject[] initialInactiveElements;
            public GameObject firstSelectedElement;
            [SerializeField] private PageType[] m_Pages;
            public PageType[] pages
            {
                get
                {
                    return m_Pages;
                }
                private set
                {
                    m_Pages = value;
                }
            }
        }
        
    }
}

