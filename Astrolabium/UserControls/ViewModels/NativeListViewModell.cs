using Astrolabium.Database;
using Astrolabium.Models;
using Astrolabium.Services;
using Astrolabium.Sevices;
using Astrolabium.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrolabium.UserControls.ViewModels
{
    internal class NativeListViewModell:ViewModelBase
    {
        private ObservableCollection<Native> nativeList = new ObservableCollection<Native>();
        private readonly ICalcService _service = new CalcService();
        private int viewType;
        
        public int ViewType
        {
            get { return viewType; }
            set
            {
                viewType = value;
                OnPropertyChanged(nameof(ViewType));
            }
        }

        public ObservableCollection<Native> NativeList
        {
            get { return nativeList; }
            set { nativeList = value;
                OnPropertyChanged(nameof(NativeList));
            }
        }

        public static NativeListViewModell GenerateTestModell()
        {
            var ret = new NativeListViewModell();
            ret.NativeList.Add(
                new Native
                {
                    ID = Guid.NewGuid(),
                    Name = "Kukugyenka Tógyel",
                    BirthLocation = Location.Default,
                    ActualLocation = Location.Default,
                    BirthDate = new DateTime(1987, 10, 12, 8, 12, 54)
                   
                }) ;
            ret.NativeList.Add(
               new Native
               {
                   ID = Guid.NewGuid(),
                   Name = "Glázser Bozsó",
                   BirthLocation = Location.Default,
                   ActualLocation = Location.Default,
                   BirthDate = new DateTime(1967, 10, 12, 8, 12, 54)
                   
               }) ;
            return ret;

        }

        public static NativeListViewModell ReadFromDatabase()
        {
            var ret = new NativeListViewModell();
            using var db = new Context();
            foreach (var p in db.Persons)
            {
                ret.NativeList.Add(
                    new Native
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Sex = (SexEnum)p.Sex,
                        BirthDate = p.BirthDate,
                        BirthDateIsCorrect = p.BirthDateIsCorret,
                        BirthLocation = new Location(MainGeoName.GetGeoName(p.BirthLocationID).Id),
                        ActualLocation = new Location(MainGeoName.GetGeoName(p.ActualLocationID).Id)
                    });
            }
            return ret;
        }

    }
}
