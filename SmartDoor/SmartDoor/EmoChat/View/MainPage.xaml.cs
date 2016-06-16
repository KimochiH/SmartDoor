using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using EmoChat.Model;
using EmoChat.Service;
using EmoChat.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmoChat.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel Vm => (MainPageViewModel)DataContext;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var currentDoor = DoorList.SelectedItem as MainPageViewModel.DoorInfo;
            if (currentDoor != null)
            {
                var button = sender as Button;
                switch (button.Content.ToString())
                {
                    case "CLR":
                        if (PassTextBox.Text != "")
                        {
                            PassTextBox.Text = "";
                        }
                        break;
                    case "":
                        if (PassTextBox.Text != "")
                        {
                            var tempPass = PassTextBox.Text;
                            var pos = tempPass.Count();
                            tempPass = tempPass.Remove(pos - 1, 1);
                            PassTextBox.Text = tempPass;
                        }
                        break;
                    default:
                        if (PassTextBox.Text.Length < 4)
                        {
                            PassTextBox.Text += button.Content;
                            if (PassTextBox.Text.Length == 4)
                            {
                                var result =
                                await SubmitDoorCode(new Door() { Code = PassTextBox.Text, DoorId = currentDoor.DoorId.Substring(currentDoor.DoorId.Length-1,1) });
                                if (result == "success")
                                {
                                    var dialog = new Windows.UI.Popups.MessageDialog("The door will open in a moment :D");
                                    await dialog.ShowAsync();
                                }
                                else
                                {
                                    var dialog = new Windows.UI.Popups.MessageDialog("Sry your code is wrong :D");
                                    await dialog.ShowAsync();
                                }
                            }
                        }
                        else
                        {

                            var result =
                               await SubmitDoorCode(new Door() { Code = PassTextBox.Text, DoorId = currentDoor.DoorId });
                            if (result == "success")
                            {
                                var dialog = new Windows.UI.Popups.MessageDialog("The door will open in a moment :D");
                                await dialog.ShowAsync();
                            }
                            else
                            {
                                var dialog = new Windows.UI.Popups.MessageDialog("Sry your code is wrong :D");
                                await dialog.ShowAsync();
                            }
                        }
                        break;
                }
           
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Please choice door u want to connect :D");
                await dialog.ShowAsync();
            }

          

        }

        WebAPIHelper _helper = new WebAPIHelper();

        Uri Uri = new Uri("http://smartdoorapi20160609040037.azurewebsites.net/api/data/up");
        Uri UpUri = new Uri("http://smartdoorapi20160609040037.azurewebsites.net/api/door/up");
        
        public async Task<string> SubmitDoorCode(Door _door)
        {
            var _postMethodRespond = await _helper.PostAsync(Uri, _door);
            var tempBody = _helper.Deserialize<DataTransfer>(_postMethodRespond);
            if (tempBody !=null)
            {
                return tempBody.Result;
            }
            return "false";
        }

        public async Task<string> RegisterDoor(Door _door)
        {
            var _postMethodRespond = await _helper.PostAsync(UpUri, _door);
            var tempBody = _helper.Deserialize<DataTransfer>(_postMethodRespond);

                return "success";
        }

        //    static async void MakeRequest()
        //    {
        //    WebRequest request = WebRequest.Create("ftp://host.com/directory");

        //    request.Credentials = new NetworkCredential("user", "pass");
        //    using (var resp = (FtpWebResponse)request.GetResponse())
        //        }


        //}


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            NumbericPad.Visibility = Visibility.Collapsed;
            ChangePassword.Visibility = Visibility.Visible;
            CreateNewDoor.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NumbericPad.Visibility = Visibility.Visible;
            ChangePassword.Visibility = Visibility.Collapsed;
            CreateNewDoor.Visibility = Visibility.Collapsed;
        }

        private void DoorList_ItemClick(object sender, ItemClickEventArgs e)
        {
            PassTextBox.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NumbericPad.Visibility = Visibility.Collapsed;
            ChangePassword.Visibility = Visibility.Collapsed;
            CreateNewDoor.Visibility = Visibility.Visible;
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var result = await RegisterDoor(new Door()
            {
                Code = NewCode.Text,
                DoorId = "3",
                LastModified = DateTime.Now,
        });
            if (result == "success")
            {
                var dialog = new Windows.UI.Popups.MessageDialog("All Done :D");
                await dialog.ShowAsync();
            }
            else
            {
                var dialog = new Windows.UI.Popups.MessageDialog("Something not wrong here :(");
                await dialog.ShowAsync();
            }
        }
    }
}
