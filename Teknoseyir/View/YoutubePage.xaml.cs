using Teknoseyir.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.NetworkInformation;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Teknoseyir.Model;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Teknoseyir.View
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class YoutubePage : Page
    {
        
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        
        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public YoutubePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        /// 
        //private void YoutubeChannelChanger()
        //{
        //    if (pivotChannelSelector.SelectedItem == pivotTeknoseyir)
        //    {
        //        username = "teknoseyir";
        //    }
        //    else if(pivotChannelSelector.SelectedItem == pivotOtoseyir)
        //    {
        //        username = "otoseyir";
        //    }
        //    else if(pivotChannelSelector.SelectedItem == pivotGamende)
        //    {
        //        username = "gamende";
        //    }
        //}
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            //try
            //{
            //    if (NetworkInterface.GetIsNetworkAvailable())
            //    {
            //        //The number of videos you would like to get in one request(from 1 to 50)
            //        int max_results = 50;

            //        ////Channel Videos
            //        ChannelVideos.Visibility = Visibility.Collapsed;
            //        ChannelProgress.Visibility = Visibility.Visible;

            //        //Here is the Id of the Channel
            //        //string YoutubeChannel = "UCFtEEv80fQVKkD4h1PF-Xqw";
            //        ////If you can't get the Channel Id, use the UserName to get it via this method
            //        ////UserName
            //        string userName = "UCZCl64NLRytf0ckqC-r3gzQ";


            //        string YoutubeChannel = await GetChannelId(userName);

            //        var channelVideos = await GetChannelVideos(YoutubeChannel, max_results);
            //        ChannelVideos.ItemsSource = channelVideos;

            //        ChannelVideos.Visibility = Visibility.Visible;
            //        ChannelProgress.Visibility = Visibility.Collapsed;
            //        /////////

            //        ////Playlist Videos
            //        PlaylistVideos.Visibility = Visibility.Collapsed;
            //        PlaylistProgress.Visibility = Visibility.Visible;

            //        //Here is the ID of the Playlist
            //        string YoutubePlaylist = "PLb6panBBCFEIkd3kFB3RePVcCqd9XxFJk";
            //        var playlistVideos = await GetPlaylistVideos(YoutubePlaylist, max_results);
            //        PlaylistVideos.ItemsSource = playlistVideos;

            //        PlaylistVideos.Visibility = Visibility.Visible;
            //        PlaylistProgress.Visibility = Visibility.Collapsed;
            //        /////////
            //    }
            //    else
            //    {
            //        MessageDialog msg = new MessageDialog("İnternet bağlantınızda problem var. Lütfen internet bağlantınızı kontrol edin ve tekrar deneyin.");
            //        await msg.ShowAsync();
            //    }
            //}
            //catch { }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion



        //Youtub Data API Credentials
        YouTubeService youtubeService = new YouTubeService(
                new BaseClientService.Initializer
                {
                    ApiKey = "AIzaSyB8ThUidDGJnPengSGVJ725LsM9TrtJQPU",
                    ApplicationName = "TeknoYT"
                });


        //Get Channel Videos
        public async Task<List<YoutubeVideo>> GetChannelVideos(string channelId, int maxResults)
        {
            var channelItemsListRequest = youtubeService.Search.List("snippet");
            channelItemsListRequest.ChannelId = channelId;
            channelItemsListRequest.Type = "video";
            channelItemsListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            channelItemsListRequest.MaxResults = maxResults;
            channelItemsListRequest.PageToken = "";

            var channelItemsListResponse = await channelItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> channelVideos = new List<YoutubeVideo>();

            foreach (var channelItem in channelItemsListResponse.Items)
            {
                channelVideos.Add(
                    new YoutubeVideo
                    {
                        Id = channelItem.Id.VideoId,
                        Title = channelItem.Snippet.Title,
                        Description = channelItem.Snippet.Description,
                        PubDate = channelItem.Snippet.PublishedAt,
                        Thumbnail = channelItem.Snippet.Thumbnails.Medium.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + channelItem.Id.VideoId
                    });
            }

            return channelVideos;
        }

        //Get Channel Videos With Pagination
        public async Task<List<YoutubeVideo>> GetChannelVideosWithPagination(string channelId, int maxResults)
        {
            var channelItemsListRequest = youtubeService.Search.List("snippet");
            channelItemsListRequest.ChannelId = channelId;
            channelItemsListRequest.Type = "video";
            channelItemsListRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            channelItemsListRequest.MaxResults = maxResults;
            channelItemsListRequest.PageToken = "";

            var channelItemsListResponse = await channelItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> channelVideos = new List<YoutubeVideo>();
            var nextPageToken = "";
            int i = 0;
            while (i < 3)
            {
                foreach (var channelItem in channelItemsListResponse.Items)
                {
                    channelVideos.Add(
                        new YoutubeVideo
                        {
                            Id = channelItem.Id.VideoId,
                            Title = channelItem.Snippet.Title,
                            Description = channelItem.Snippet.Description,
                            PubDate = channelItem.Snippet.PublishedAt,
                            Thumbnail = channelItem.Snippet.Thumbnails.Medium.Url,
                            YoutubeLink = "https://www.youtube.com/watch?v=" + channelItem.Id.VideoId
                        });
                }

                i++;
                nextPageToken = channelItemsListResponse.NextPageToken;
            }

            return channelVideos;
        }

        //Get Playlist Videos
        public async Task<List<YoutubeVideo>> GetPlaylistVideos(string playlistId, int maxResults)
        {
            var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
            playlistItemsListRequest.PlaylistId = playlistId;
            playlistItemsListRequest.MaxResults = maxResults;
            playlistItemsListRequest.PageToken = "";

            var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();
            List<YoutubeVideo> playlistVideos = new List<YoutubeVideo>();

            foreach (var playlistItem in playlistItemsListResponse.Items)
            {
                playlistVideos.Add(
                    new YoutubeVideo
                    {
                        Id = playlistItem.Snippet.ResourceId.VideoId,
                        Title = playlistItem.Snippet.Title,
                        Description = playlistItem.Snippet.Description,
                        PubDate = playlistItem.Snippet.PublishedAt,
                        Thumbnail = playlistItem.Snippet.Thumbnails.Medium.Url,
                        YoutubeLink = "https://www.youtube.com/watch?v=" + playlistItem.Snippet.ResourceId.VideoId
                    });
            }

            return playlistVideos;
        }

        //Get Channel Id
        public async Task<string> GetChannelId(string userName)
        {
            var channelIdRequest = youtubeService.Channels.List("id");
            channelIdRequest.ForUsername = userName;

            var channelIdResponse = await channelIdRequest.ExecuteAsync();

            return channelIdResponse.Items.FirstOrDefault().Id;
        }


        //After selecting a video, navigate to the VideoPage
        private void Videos_ItemClick(object sender, ItemClickEventArgs e)
        {
            YoutubeVideo video = e.ClickedItem as YoutubeVideo;
            if (video != null)
                Frame.Navigate(typeof(VideoPage), video);
        }

        private async void pivotChannelSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (pivotChannelSelector.SelectedItem == pivotTeknoseyir)
            {
                try
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        //The number of videos you would like to get in one request(from 1 to 50)
                        int max_results = 50;

                        ////Channel Videos
                        ChannelVideos.Visibility = Visibility.Collapsed;
                        ChannelProgress.Visibility = Visibility.Visible;

                        string userName = "teknoseyir";
                        string YoutubeChannel = await GetChannelId(userName);

                        var channelVideos = await GetChannelVideos(YoutubeChannel, max_results);
                        ChannelVideos.ItemsSource = channelVideos;

                        ChannelVideos.Visibility = Visibility.Visible;
                        ChannelProgress.Visibility = Visibility.Collapsed;

                    }
                    else
                    {
                        MessageDialog msg = new MessageDialog("İnternet bağlantınızda problem var. Lütfen internet bağlantınızı kontrol edin ve tekrar deneyin.");
                        await msg.ShowAsync();
                    }
                }
                catch
                {
                    MessageDialog msg = new MessageDialog("Can't load new channel.");
                    await msg.ShowAsync();
                }
            }
            else if (pivotChannelSelector.SelectedItem == pivotOtoseyir)
            {
                //YoutubeChannel = "UCsBqTbGrvtDZYLXt73vkYvw";
                try
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        //The number of videos you would like to get in one request(from 1 to 50)
                        int max_results = 50;

                        ////Channel Videos
                        ChannelVideos.Visibility = Visibility.Collapsed;
                        ChannelProgress.Visibility = Visibility.Visible;

                        string YoutubeChannel = "UCsBqTbGrvtDZYLXt73vkYvw";

                        var channelVideos = await GetChannelVideos(YoutubeChannel, max_results);
                        ChannelVideosOtoseyir.ItemsSource = channelVideos;

                        ChannelVideos.Visibility = Visibility.Visible;
                        ChannelProgress.Visibility = Visibility.Collapsed;

                    }
                    else
                    {
                        MessageDialog msg = new MessageDialog("İnternet bağlantınızda problem var. Lütfen internet bağlantınızı kontrol edin ve tekrar deneyin.");
                        await msg.ShowAsync();
                    }
                }
                catch
                {
                    MessageDialog msg = new MessageDialog("Can't load new channel.");
                    await msg.ShowAsync();
                }
            }
            else if (pivotChannelSelector.SelectedItem == pivotGamende)
            {
                //YoutubeChannel = "UCZCl64NLRytf0ckqC-r3gzQ";
                try
                {
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        //The number of videos you would like to get in one request(from 1 to 50)
                        int max_results = 50;

                        ////Channel Videos
                        ChannelVideos.Visibility = Visibility.Collapsed;
                        ChannelProgress.Visibility = Visibility.Visible;

                        string YoutubeChannel = "UCZCl64NLRytf0ckqC-r3gzQ";

                        var channelVideos = await GetChannelVideos(YoutubeChannel, max_results);
                        ChannelVideosGamende.ItemsSource = channelVideos;

                        ChannelVideos.Visibility = Visibility.Visible;
                        ChannelProgress.Visibility = Visibility.Collapsed;

                    }
                    else
                    {
                        MessageDialog msg = new MessageDialog("İnternet bağlantınızda problem var. Lütfen internet bağlantınızı kontrol edin ve tekrar deneyin.");
                        await msg.ShowAsync();
                    }
                }
                catch
                {
                    MessageDialog msg = new MessageDialog("Can't load new channel.");
                    await msg.ShowAsync();
                }
            }
            else
            {
                MessageDialog msg = new MessageDialog("Cant get ID");
                await msg.ShowAsync();
            }

            
        }
    }
}
