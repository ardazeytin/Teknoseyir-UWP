using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409



namespace Teknoseyir
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {

            this.InitializeComponent();
            DetectPlatform();
            Application.Current.Resources["SystemControlHighlightListAccentLowBrush"] = new SolidColorBrush(Colors.Transparent);
            Application.Current.Resources["SystemControlHighlightListAccentMediumBrush"] = new SolidColorBrush(Colors.Transparent);
            Application.Current.Resources["SystemControlHighlightAltBaseHighBrush"] = new SolidColorBrush(Colors.White); //foreground
            Application.Current.Resources["SystemControlHighlightListlowBrush"] = new SolidColorBrush(Colors.DarkRed);
            Application.Current.Resources["SystemControlHighlightListMediumBrush"] = new SolidColorBrush(Colors.DarkRed);
            //Application.Current.Resources["SystemControlForegroundBaseMediumHighBrush"] = new SolidColorBrush(Colors.DarkRed);
            ApplyUserSettings();
            AddressBarSwitcher();
            NavigationCacheMode = NavigationCacheMode.Enabled;

            //Check Device Platform
            

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                
                
                if (webView.CanGoBack)
                {
                    webView.GoBack();
                    a.Handled = true;
                }
            
                ApplyUserSettings();
                AddressBarSwitcher();
            };
        }

       private void CloseSplitPanel()
        {
            if (ShellSplitView.IsPaneOpen == true)
            {
                this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
            }    
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {
            if (webView.CanGoForward)
            {
                webView.GoForward();
            }
        }

        public static Platform DetectPlatform()
        {

            bool isHardwareButtonsAPIPresent =
                ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons");

            if (isHardwareButtonsAPIPresent)
            {
                return Platform.WindowsPhone;
            }
            else
            {
                return Platform.Windows;
            }
        }

        

        private async void ApplyUserSettings()
        {
            try
            {
                StorageFolder local = ApplicationData.Current.LocalFolder;

                var dataFolder = await local.GetFolderAsync("Data Folder");
                var file = await dataFolder.GetFileAsync("SwitchSplitDisplayMode.txt");
                String SwitchSplitDisplayMode = await FileIO.ReadTextAsync(file);

                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                {
                    // code for phone
                    if (SwitchSplitDisplayMode == "on" && SwitchSplitDisplayMode != null)
                    {
                        ShellSplitView.DisplayMode = SplitViewDisplayMode.Inline;

                    }
                    else ShellSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                }
                else
                {
                    // other code
                    if (SwitchSplitDisplayMode == "on" && SwitchSplitDisplayMode != null)
                    {
                        ShellSplitView.DisplayMode = SplitViewDisplayMode.Inline;

                    }
                    else ShellSplitView.DisplayMode = SplitViewDisplayMode.CompactInline;
                }

                


            }
            catch (Exception)
            {
                //ShellSplitView.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                
            }

            
            
        }

        private async void AddressBarSwitcher()
        {
            try
            {
                StorageFolder local = ApplicationData.Current.LocalFolder;

                var dataFolder = await local.GetFolderAsync("Data Folder");
                var file3 = await dataFolder.GetFileAsync("SwitchAddressBarDisplayMode.txt");
                String SwitchAddressBarDisplayMode = await FileIO.ReadTextAsync(file3);

                if (SwitchAddressBarDisplayMode == "on" && SwitchAddressBarDisplayMode != null)
                {
                    //do work
                    mainpageAppBarGrid.Visibility = Visibility.Collapsed;
                    webViewGrid.Margin = new Thickness(0, 0, 0, 0);
                    btnGoHome.Height = 48;
                }
                else
                {
                    mainpageAppBarGrid.Visibility = Visibility.Visible;
                    webViewGrid.Margin = new Thickness(0, 48, 0, 0);
                    btnGoHome.Height = 1;
                }
            }
            catch (Exception)
            {
                mainpageAppBarGrid.Visibility = Visibility.Visible;
                webViewGrid.Margin = new Thickness(0, 48, 0, 0);
                btnGoHome.Height = 1;
            }
        }

        //Webview new tab request handler
        private void webView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            //var newWebView = new WebView();
            //newWebView.Navigate(args.Uri);
            //webViewGrid.Children.Add(newWebView);
            //args.Handled = true;

            //Open new tab in same webview
            webView.Navigate(args.Uri);
            //Block the default browser to launch
            args.Handled = true;

        }

        // ProgressRing enable
        private void webView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            progRing.Opacity = 1;
            btnRefresh.Opacity = 0;
            btnRefresh.IsEnabled = false;
            btnStop.IsEnabled = true;
            btnStop.Opacity = 1;
            tbxWebPageAdress.Text = Convert.ToString(webView.Source);
        }

        //ProgressRing disable
        private void webView_LoadCompleted(object sender, NavigationEventArgs e)
        {
            btnStop.Opacity = 0;
            btnStop.IsEnabled = false;
            btnRefresh.IsEnabled = true;
            btnRefresh.Opacity = 1;
            progRing.Opacity = 0;
        }

        private void HamburgerRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            //(sender as RadioButton).IsChecked = false;

            this.ShellSplitView.IsPaneOpen = !this.ShellSplitView.IsPaneOpen;
            
        }

       

        private void Mainpage_Click(object sender, RoutedEventArgs e)
        {
            webView.Navigate(new Uri("http://teknoseyir.com/"));
            btnGo.IsEnabled = false;
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
                    this.tbxNotifiMain.Text = streamReader.ReadToEnd();
                }

            }
        }


        private void buttonRefresh_Click(object sender, RoutedEventArgs e)
        {
            webView.Refresh();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            webView.Stop();
        }



        private async void btnGo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                webView.Navigate(new Uri(tbxWebPageAdress.Text));
            }
            catch (Exception)
            {
                var messageDialog = new MessageDialog("Geçersiz adres.","Bir şey oldu :(");
                messageDialog.Commands.Add(new UICommand("Tamam", delegate (IUICommand command)
                {

                }));
                await
                                // call the ShowAsync() method to display the message dialog
                                messageDialog.ShowAsync();
            }
        }

        private void tbxWebPageAdress_LostFocus(object sender, RoutedEventArgs e)
        {
            btnGo.Opacity = 0;  
            btnHome.IsEnabled = true;
            btnHome.Opacity = 1;
        }

        private void tbxWebPageAdress_GotFocus(object sender, RoutedEventArgs e)
        {
            btnHome.Opacity = 0;
            btnHome.IsEnabled = false;
            btnGo.IsEnabled = true;
            btnGo.Opacity = 1;
            
        }



        private async void tbxWebPageAdress_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                try
                {
                    webView.Navigate(new Uri(tbxWebPageAdress.Text));
                }
                catch (Exception)
                {
                    var messageDialog = new MessageDialog("Geçersiz adres.", "Bir şey oldu :(");
                    messageDialog.Commands.Add(new UICommand("Tamam", delegate (IUICommand command)
                    {

                    }));
                    await
                                    // call the ShowAsync() method to display the message dialog
                                    messageDialog.ShowAsync();
                }
            }
        }



        //listbox items start------------------
        private async void btnNotification_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await ReadFile();
                webView.Navigate(new Uri("http://teknoseyir.com/u/" + tbxNotifiMain.Text + "/bildirim"));
            }
            catch (Exception)
            {
                var messageDialog = new MessageDialog("Kullanıcı adınızı uygulamaya kayıt etmemiş ya da giriş yapmamış olabilirsiniz. Lütfen ayarlardan kullanıcı adınızı kaydedin ve hesabınız ile giriş yaptığınızdan emin olun.");
                messageDialog.Commands.Add(new UICommand("Tamam", delegate (IUICommand command)
                {

                }));
                await


                                // call the ShowAsync() method to display the message dialog
                                messageDialog.ShowAsync();

            }

            CloseSplitPanel();
        }

        private async void btnMentions_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await ReadFile();
                webView.Navigate(new Uri("http://teknoseyir.com/u/" + tbxNotifiMain.Text + "/bahseden"));
            }
            catch (Exception)
            {
                var messageDialog = new MessageDialog("Kullanıcı adınızı uygulamaya kayıt etmemiş ya da giriş yapmamış olabilirsiniz. Lütfen ayarlardan kullanıcı adınızı kaydedin ve hesabınız ile giriş yaptığınızdan emin olun.");
                messageDialog.Commands.Add(new UICommand("Tamam", delegate (IUICommand command)
                {

                }));
                await


                                // call the ShowAsync() method to display the message dialog
                                messageDialog.ShowAsync();

            }
            CloseSplitPanel();
        }

        private async void btnFav_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await ReadFile();
                webView.Navigate(new Uri("http://teknoseyir.com/u/" + tbxNotifiMain.Text + "/favori"));
            }
            catch (Exception)
            {
                var messageDialog = new MessageDialog("Kullanıcı adınızı uygulamaya kayıt etmemiş ya da giriş yapmamış olabilirsiniz. Lütfen ayarlardan kullanıcı adınızı kaydedin ve hesabınız ile giriş yaptığınızdan emin olun.");
                messageDialog.Commands.Add(new UICommand("Tamam", delegate (IUICommand command)
                {

                }));
                await
                                // call the ShowAsync() method to display the message dialog
                                messageDialog.ShowAsync();

            }
            CloseSplitPanel();
        }

        private void btnYoutube_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSplitPanel();
            Frame.Navigate(typeof(View.YoutubePage));
        }

        private void btnChat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSplitPanel();
            webView.Navigate(new Uri("https://tlk.io/teknoseyir"));
        }

        private void btnSettings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSplitPanel();
            Frame.Navigate(typeof(Settings));
        }

        private async void btnProfil_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                await ReadFile();
                webView.Navigate(new Uri("http://teknoseyir.com/u/" + tbxNotifiMain.Text));
            }
            catch (Exception)
            {
                var messageDialog = new MessageDialog("Kullanıcı adınızı uygulamaya kayıt etmemiş ya da giriş yapmamış olabilirsiniz. Lütfen ayarlardan kullanıcı adınızı kaydedin ve hesabınız ile giriş yaptığınızdan emin olun.");
                messageDialog.Commands.Add(new UICommand("Tamam", delegate (IUICommand command)
                {

                }));
                await
                                // call the ShowAsync() method to display the message dialog
                                messageDialog.ShowAsync();

            }
            CloseSplitPanel();
        }

        private void btnGoHome_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSplitPanel();
            webView.Navigate(new Uri("http://teknoseyir.com/"));
        }

        private void btnStore_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CloseSplitPanel();
            webView.Navigate(new Uri("https://teknoseyir.com/magaza"));
        }
        //listbox items end-----------------
    }
}
