using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrolabium.Models
{
    public class SignValues : IAspectable
    {
        private int _sign;
        public int Sign
        {
            get => _sign;
            set
            {
                _sign = value;
                Cusp = value * 30;
            }
        }
        public string SignName { get; set; }
        public double Cusp { get; private set; }
        public double Longitude { get => Cusp; set => throw new Exception("Ez a proerti nem írható "); }
        public override string ToString() => $"House{SignName}";
    }

    public class Signs : IEnumerable<SignValues>,IEnumerator<SignValues>
    {
        private readonly List<SignValues> _signs = new List<SignValues>();

        private string getLocalisedSignName(int id)
        {
            return string.Empty;
        }

        public Signs()
        {
            _signs.Add( new SignValues() { Sign = 0, SignName = "Aries" });
            _signs.Add( new SignValues() { Sign = 1, SignName = "Taurus" });
            _signs.Add(new SignValues() { Sign = 2, SignName = "Gemini" });
            _signs.Add(new SignValues() { Sign = 3, SignName = "Cancer" });
            _signs.Add(new SignValues() { Sign = 4, SignName = "Leo" });
            _signs.Add(new SignValues() { Sign = 5, SignName = "Virgo" });
            _signs.Add(new SignValues() { Sign = 6, SignName = "Libra" });
            _signs.Add(new SignValues() { Sign = 7, SignName = "Scorpio" });
            _signs.Add(new SignValues() { Sign = 8, SignName = "Sagittarius" });
            _signs.Add(new SignValues() { Sign = 9, SignName = "Capricorn" });
            _signs.Add(new SignValues() { Sign = 10, SignName = "Aquarius" });
            _signs.Add(new SignValues() { Sign = 11, SignName = "Pisces" });
    }

        public SignValues this[int i] => _signs[i];

        public SignValues this[double d] => this[(int)(d / 30)];

        private int _current;

        public SignValues Current => _signs[_current];

        object IEnumerator.Current => _signs[_current];

        public void Dispose() => _current = 0;

        public IEnumerator<SignValues> GetEnumerator() => this;

        public bool MoveNext() => ++_current < _signs.Count;

        public void Reset() => _current = 0;

        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}
