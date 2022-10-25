using Astrolabium.Models;
using Astrolabium.Sevices;
using Astrolabium.ViewModels;
using System;
using System.Collections.Generic;
using NGeoNames.Entities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrolabium.UserControls.ViewModels
{
    public class NativeEditorViewModell:ViewModelBase
    {
        public string NameCaption { get; } = "Név";
        public String BirthDateCaption { get; } = "Születés ideje";
        public string BirthLocationCaption { get; } = "Születés helye";
        public string ActualLocationCaption { get; } = "Tartózkodási hely";
        public string SaveButtonText { get; } = "Mentés";
        public string CancelButtonText { get; } = "Mégsem";

        //public DateTime TesztDatum { get; } = new DateTime(1956, 10, 23);
        public DateTime TesztDatum { get { return Native.BirthDate; } } 
        public Uri BirthDateOKUri
        {
            get
            {
                const string BirthdayCorrectImageName = "icons8-checkmark-48.png";
                const string BirthdayIncorrectImageName = "icons8-delete-48.png";
                string path = Native.BirthDateIsCorrect ? BirthdayCorrectImageName : BirthdayIncorrectImageName;
                return new Uri($"pack://application:,,,/Astrolabium;component/Icons/{path}");
            }
            set
            {
                OnPropertyChanged(nameof(BirthDateOKUri));
            }
        }
        public Uri SexUri
        {
            get
            {
                const string Female = "Female-icon.png";
                const string Male = "Male-icon.png";
                string path = Native.Sex==SexEnum.Female ? Female : Male;
                return new Uri($"pack://application:,,,/Astrolabium;component/Icons/{path}");
            }
            set
            {
                OnPropertyChanged(nameof(SexUri));
            }
        }
        public Native Native { get; set; } = new Native();
        public List<Location> GeoNames { get; } = MainGeoName.GeoNames.ToLocationList();
    }

    public static class GeoNameExtender
    {
       public static List<Location> ToLocationList(this List<ExtendedGeoName> geoNames)
        {
            var ret = new List<Location>();
            foreach (var g in geoNames)
            {
                ret.Add(new Location(g.Id));
            }
            return ret;
        }
    }
}
