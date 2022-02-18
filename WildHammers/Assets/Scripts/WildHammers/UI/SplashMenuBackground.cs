
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WildHammers
{
    namespace UI
    {
        public class SplashMenuBackground : MonoBehaviour
        {
            public int width = 71;
            public int height = 38;
            public double vectorX = 35.5;
            public double vectorY = 19.0;
            public double length = 8.0;
            public double frequency = 200.0;
            [SerializeField] private TMP_Text m_TextSpace;
            private float m_UnlimitedTimer = 0f;

            private string m_AsciiLookup = ".:*o&8@#";

            private double[] m_SineTable  = new double[256];

            private void Awake()
            {
                for (int i = 0; i < 256; i++)
                {
                   m_SineTable[i] = Math.Round(((Math.Sin(i * 2 * Math.PI / 255) * 255) + 255) / 2);
                }
                
            }


            private void Update()
            {
                if (gameObject.activeInHierarchy)
                {
                    string _pattern = "";

                    m_UnlimitedTimer += Time.deltaTime;
                    
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            _pattern += ConvertDoubleToAsciiValue(
                                Chebyshev(x, y, vectorX, vectorY, m_UnlimitedTimer*frequency, length));
                        }
                    }
                    

                    m_TextSpace.text = _pattern;
                    
                }
            }

            private double Sine(double _x)
            {
                _x = _x % 256;
                return (_x < 0) ? m_SineTable[(int)-_x] : m_SineTable[(int)_x];
            }

            //Might play with this a bit more, combine waves and whatnot
            private double Plane(double _x, double _y, double _vx, double _vy, double _phase, double _length)
            {
                return Sine(Math.Floor((_vx * _x + _vy * _y) * _length + _phase));
            }
            
            private double Chebyshev (double _x, double _y, double _centerX, double _centerY, double _phase, double _diameter)
            {
                double _dx = Math.Abs(_x - _centerX);
                double _dy = Math.Abs(_y - _centerY);
                return Sine(Math.Floor(Math.Max(_dx, _dy) * _diameter + _phase) );
            }

            private string ConvertDoubleToAsciiValue(double _x)
            {
                int _scaledX = ConvertRange(0.0, 255.0, 0.0,m_AsciiLookup.Length-1, _x);
                return m_AsciiLookup.Substring(_scaledX,1);
            }

            private int ConvertRange(double originalStart, double originalEnd, double newStart, double newEnd, double value)
            {
                double scale = (newEnd - newStart) / (originalEnd - originalStart);
                return (int)(newStart + (value - originalStart) * scale);
            }
        }
        
    }
}
