//-----------------------------------------------------------------------
// <copyright file="EngagementPageOverlay.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Azure.Engagement.Overlay
{
  /// <summary>
  /// Helper class used to replace Windows's Page class.
  /// Automatically adds the Engagement overlay in the current page to host Reach in-app campaigns UI.
  /// It inherits from <see cref="EngagementPage"/> which automatically starts a new Engagement activity when displayed.
  /// </summary>
  public class EngagementPageOverlay : EngagementPage
  {
    /// <summary>
    /// Height of the banner in the page.
    /// </summary>
    protected const int BannerHeight = 80;

    /// <summary>
    /// Overlay container's name.
    /// </summary>
    protected const string OverlayContainerName = "engagementGrid";

    /// <summary>
    /// Banner webview's name.
    /// </summary>
    protected const string BannerWebviewName = "engagement_notification_content";

    /// <summary>
    /// Interstitial webview's name.
    /// </summary>
    protected const string InterstitialWebviewName = "engagement_announcement_content";

    /// <summary>
    /// Engagement component's name assigned to the overlay.
    /// </summary>
    protected const string EngagementComponentName = "Overlay";

    /// <summary>
    /// Initializes a new instance of the <see cref="EngagementPageOverlay" /> class.
    /// </summary>
    public EngagementPageOverlay()
    {
      // Initialize the webview for notifications.
      Banner = new WebView();
      Banner.Name = BannerWebviewName;
      Banner.Height = BannerHeight;
      Banner.HorizontalAlignment = HorizontalAlignment.Stretch;
      Banner.VerticalAlignment = VerticalAlignment.Top;
      Banner.Visibility = Visibility.Collapsed;

      // Initialize the webview for text\Web announcements and Polls.
      Interstitial = new WebView();
      Interstitial.Name = InterstitialWebviewName;
      Interstitial.HorizontalAlignment = HorizontalAlignment.Stretch;
      Interstitial.VerticalAlignment = VerticalAlignment.Stretch;
      Interstitial.Visibility = Visibility.Collapsed;

      // Initialize the webviews' container.
      Container = new Grid();
      Container.Name = OverlayContainerName;

      // Add webviews to the container but leave the first index to the page's content.
      Container.Children.Insert(1, Banner);
      Container.Children.Insert(2, Interstitial);
    }

    /// <summary>
    /// Gets the <see cref="WebView"/> element hosting the in-app banner (notifications).
    /// </summary>
    protected WebView Banner
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the <see cref="WebView"/> element hosting the in-app interstitial view (text\web announcements and polls).
    /// </summary>
    protected WebView Interstitial
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the <see cref="Grid"/> element hosting the overlay.
    /// </summary>
    protected Grid Container
    {
      get;
      private set;
    }

    /// <summary>
    /// Initializes the overlay and send an activity to the Engagement backend when the page is displayed.
    /// </summary>
    /// <param name="e">Navigation event argument.</param>
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      try
      {
        UIElement content;

        // Get any existing Engagement container. This may happen if the page has been restored from cache.
        var existingContainer = (Grid)this.FindName(OverlayContainerName);

        // Add the overlay container to the page if it's not already there.
        if (existingContainer == null)
        {
          content = this.Content;

          // Replace the content of the page with the container then attach the previous content to the container.
          this.Content = Container;
          Container.Children.Insert(0, content);
        }
        else if (existingContainer != Container)
        {
          // It's not our overlay container.
          EngagementLogger.Trace.Error(EngagementComponentName, "The element named '" + OverlayContainerName + "' from this page conflicts with the Engagement overlay container.");
        }
      }
      catch
      {
        EngagementLogger.Trace.Error(EngagementComponentName, "Can't add the overlay dynamically to the page, you should consider adding the Engagement webviews manually.");
      }

      // The base implementation will call the start activity API.
      base.OnNavigatedTo(e);
    }
  }
}
