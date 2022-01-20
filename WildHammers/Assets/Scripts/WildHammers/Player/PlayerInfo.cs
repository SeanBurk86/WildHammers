
using UnityEngine;

namespace WildHammers
{
    namespace Player
    {
        public class PlayerInfo : MonoBehaviour
        {
            public string playerInitials { get; private set; }
            public ZodiacType zodiacSign { get; private set; }
            public Color hammerColor { get; private set; }

            #region Public Functions

                public bool SetInitials(string _initials)
                {
                    if (_initials.Length==0)
                    {
                        Log("Attempting to set initials with no characters");
                        return false;
                    }

                    playerInitials = _initials;
                    return true;
                }

                public bool SetZodiacSign(int _index)
                {
                    ZodiacType _zodiacType = (ZodiacType)_index;
                    if (_zodiacType == ZodiacType.None)
                    {
                        Log("Attempting to set player data with no zodiac sign");
                        return false;
                    }
                    
                    zodiacSign = _zodiacType;
                    return true;
                }

                public void SetHammerColor(Color _color)
                {
                    hammerColor = _color;
                }
            

            #endregion

            #region Private Functions

                private void Log(string _msg)
                {
                    Debug.Log("[Player Data Model]: "+_msg);
                }
                
                private void LogWarning(string _msg)
                {
                    Debug.LogWarning("[Player Data Model]: "+_msg);
                }
            

            #endregion
        }
        
    }
}

