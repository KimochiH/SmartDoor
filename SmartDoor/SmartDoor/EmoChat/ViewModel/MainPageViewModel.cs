using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.System;
using EmoChat.Model;
using EmoChat.Service;

namespace EmoChat.ViewModel
{
    public class MainPageViewModel : BaseViewModel
    {
        private ObservableCollection<DoorInfo> _doorListSource;

        public ObservableCollection<DoorInfo> DoorListSource
        {
            get { return _doorListSource; }
            set
            {
                if (Equals(value, _doorListSource)) return;
                _doorListSource = value;
                OnPropertyChanged();
            }
        }

        public class DoorInfo 
        {
            public string DoorId { get; set; }
            public string LastUpdate { get; set; }
        }

        WebAPIHelper _helper = new WebAPIHelper();

        Uri Uri = new Uri("http://smartdoorapi20160609040037.azurewebsites.net/api/door");

        public async Task<List<Door>> GetDoors()
        {
            string StrData = await _helper.GetAsync(Uri);
            return _helper.Deserialize<List<Door>>(StrData);
        }

        private async void Init()
        {
            DoorListSource = new ObservableCollection<DoorInfo>();
            var DoorList = await GetDoors();
            foreach (var door in DoorList)
            {
                DoorListSource.Add(new DoorInfo()
                {
                    DoorId = "Door #" + door.DoorId,
                    LastUpdate = "Last changed on " + string.Format("{0:d}", door.LastModified),
                });
            }
        }

        public MainPageViewModel()
        {
           Init();
            //DoorListSource = new List<DoorInfo>()
            //{
            //    new DoorInfo()
            //    {
            //        DoorId = "Door #0",
            //        LastUpdate = "Last changed on "+  string.Format("{0:d}",DateTime.Now),
            //    },
            //    new DoorInfo()
            //    {
            //        DoorId = "Door #1",
            //        LastUpdate = "Last changed on "+ string.Format("{0:d}",DateTime.Now),
            //    }
            //};
        }
    }
}
