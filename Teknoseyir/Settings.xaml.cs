using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Teknoseyir
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {

        public Settings()
        {
            InitializeComponent();
            //NavigationCacheMode = NavigationCacheMode.Enabled;
            SettingsReader();
            

        }

        private async void SettingsReader()
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;

            try
            {
                if (local != null)
                {
                    var dataFolder = await local.GetFolderAsync("Data Folder");
                    var file = await dataFolder.GetFileAsync("SwitchSplitDisplayMode.txt");
                    String SwitchSplitDisplayMode = await FileIO.ReadTextAsync(file);
                    if (SwitchSplitDisplayMode == "on")
                    {
                        tglSwitchSplitDisplayMode.IsOn = true;
                    }
                    else tglSwitchSplitDisplayMode.IsOn = false;
                }

                if (local != null)
                {
                    var dataFolder = await local.GetFolderAsync("Data Folder");
                    var file3 = await dataFolder.GetFileAsync("SwitchAddressBarDisplayMode.txt");
                    String SwitchAddressBarDisplayMode = await FileIO.ReadTextAsync(file3);
                    if (SwitchAddressBarDisplayMode == "on")
                    {
                        tglSwitchAddressBarDisplayMode.IsOn = true;
                    }
                    else tglSwitchAddressBarDisplayMode.IsOn = false;
                }

                if (local != null)
                {
                    var dataFolder = await local.GetFolderAsync("Data Folder");
                    var file2 = await dataFolder.GetFileAsync("ID.txt");
                    String ID = await FileIO.ReadTextAsync(file2);
                    if (ID != null)
                    {
                        tbxNotification.Text = ID;
                    }
                    
                }
            }
            catch (Exception)
            {

            }
            
        }


        private async void btnKaydet_Click(object sender, RoutedEventArgs e)
        {
            await WriteToFile();

            //Show Success Message
            var dlg = new MessageDialog("Artık uygulamanın tüm özelliklerinden istediğin gibi yararlanabilirsin.", "Kullanıcı adın kaydedildi!");
            dlg.Commands.Add(new UICommand("Tamam", null, "YES"));
            //dlg.Commands.Add(new UICommand("Hayır", null, "NO"));
            var op = await dlg.ShowAsync();
            if ((string)op.Id == "YES")
            {
                
            }

        }

        public async Task WriteToFile()
        {
            // Get the text data from the textbox
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(this.tbxNotification.Text.ToCharArray());

            //Get the local folder
            StorageFolder local = ApplicationData.Current.LocalFolder;

            //Create new folder name DataFolder
            var dataFolder = await local.CreateFolderAsync("Data Folder", CreationCollisionOption.OpenIfExists);

            //Create txt file
            var file = await dataFolder.CreateFileAsync("ID.txt", CreationCollisionOption.ReplaceExisting);

            //write the data from the text box
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }

        }

        public async Task ReadFile()
        {
            // Get the local folder.
            StorageFolder local = ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                // Get the DataFolder folder.
                var dataFolder = await local.GetFolderAsync("Data Folder");

                // Get the file.
                var file = await dataFolder.OpenStreamForReadAsync("ID.txt");

                // Read the data.
                using (StreamReader streamReader = new StreamReader(file))
                {
                    this.tbxNotification.Text = streamReader.ReadToEnd();
                }

            }

        }


        private async void tglSwitchSplitDisplayMode_Toggled(object sender, RoutedEventArgs e)
        {

            StorageFolder local = ApplicationData.Current.LocalFolder;
            var dataFolder = await local.CreateFolderAsync("Data Folder", CreationCollisionOption.OpenIfExists);
            var file = await dataFolder.CreateFileAsync("SwitchSplitDisplayMode.txt", CreationCollisionOption.ReplaceExisting);

            if (tglSwitchSplitDisplayMode.IsOn)
            { 
                await FileIO.WriteTextAsync(file,"on");
            }
            else await FileIO.WriteTextAsync(file, "off");


        }

        private async void tglSwitchAddressBarDisplayMode_Toggled(object sender, RoutedEventArgs e)
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            var dataFolder = await local.CreateFolderAsync("Data Folder", CreationCollisionOption.OpenIfExists);
            var file3 = await dataFolder.CreateFileAsync("SwitchAddressBarDisplayMode.txt", CreationCollisionOption.ReplaceExisting);

            if (tglSwitchAddressBarDisplayMode.IsOn)
            {
                await FileIO.WriteTextAsync(file3, "on");
            }
            else await FileIO.WriteTextAsync(file3, "off");
        }

        //private async void ComposeEmail(Contact Torecipient, string messageBody)
        //{
        //    var to = Torecipient.Emails.FirstOrDefault();
        //    var emailRecipient = new EmailRecipient(to.Address);
        //    EmailMessage objEmail = new EmailMessage();
        //    objEmail.Subject = "Suresh";
        //    objEmail.To.Add(emailRecipient);
        //    await EmailManager.ShowComposeNewEmailAsync(objEmail);
        //}
    }      
}
