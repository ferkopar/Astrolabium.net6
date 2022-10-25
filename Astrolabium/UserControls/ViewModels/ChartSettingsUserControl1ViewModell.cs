using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrolabium.Helpers;
using Astrolabium.Models;
using Astrolabium.Sevices;
using Astrolabium.ViewModels;
using SweCore.Planets;

namespace Astrolabium.UserControls.ViewModels
{
    public  record struct ShowBody(bool Showed, Planet Body,  string Name)
    {
        public string DisplayCharacter => PlanetHelper.GetPlanetString(Body);
        public string Name => PlanetHelper.GetPlanetName(Body);
    }
    
    internal class ChartSettingsUserControl1ViewModell : ViewModelBase
    {
        readonly ObservableCollection<ShowBody> _showBody = new ObservableCollection<ShowBody>
        {
            new ShowBody {Showed=true,Body=Planet.Sun},
            new ShowBody {Showed=true,Body=Planet.Moon,},
            new ShowBody {Showed=true,Body=Planet.Mercury},
            new ShowBody {Showed=true,Body=Planet.Venus},
            new ShowBody {Showed=true,Body=Planet.Mars},
            new ShowBody {Showed=true,Body=Planet.Jupiter},
            new ShowBody {Showed=true,Body=Planet.Saturn },
            new ShowBody {Showed=true,Body=Planet.Uranus},
            new ShowBody {Showed=true,Body=Planet.Neptune},
            new ShowBody {Showed=true,Body=Planet.Pluto},
            new ShowBody {Showed=false,Body=Planet.Eros},
            new ShowBody {Showed=false,Body=Planet.Sappho},
            new ShowBody {Showed=false,Body=Planet.Psyhe},
            new ShowBody {Showed=false,Body=Planet.Ceres},
            new ShowBody {Showed=false,Body=Planet.Juno},
            new ShowBody {Showed=false,Body=Planet.Pholus },
            new ShowBody {Showed=false,Body=Planet.Chiron},
            new ShowBody {Showed=true,Body=Planet.TrueNode}
        };

        public ObservableCollection<ShowBody> ShowBody => _showBody;
    
 
    }
}
