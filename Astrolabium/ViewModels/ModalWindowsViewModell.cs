using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Astrolabium.Models;
using FerkopaUtils;

namespace Astrolabium.ViewModels
{
    public class ModalWindowsViewModell
    {
        private readonly List<ElementText> _elementTexts;

        public ModalWindowsViewModell() => _elementTexts = XmlSerializerUtil.Deserialize<List<ElementText>>(@".\TextData\Radix.axml");

        public ElementText GetPlantSignText(string planetName, string signName)
        {
            // var  pattern = @"Your *(Sun|Moon) *in *(Aries|Taurus|Gemini|Cancer|Leo|Virgo|Libra|Scorpio|Sagittarius|Capricorn|Aquarius|Pisces)";
            var input = $"Your {planetName} in {signName}";
            try
            {
                return 
                _elementTexts.First(e => string.Equals(e.TextId, input, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (InvalidOperationException e)
            {
                return new ElementText();
            }

        }
    }
}
