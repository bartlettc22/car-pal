using System.Windows;
using System.Windows.Navigation;
using car_pal.Models;
using car_pal.ViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace car_pal
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        // The static ViewModel, to be used across the application.
        private static MainViewModel _viewModel;
        public static MainViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            LoadViewModelFromDB();
            RootFrame.DataContext = ViewModel;
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (e.IsApplicationInstancePreserved)
            {
                //LoadViewModelFromAppState();
            }
            else
            {
                LoadViewModel();
                RootFrame.DataContext = ViewModel;
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            SaveViewModel();
            //SaveViewModelToIsolatedStorage();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            //SaveViewModelToIsolatedStorage();
        }

        private void LoadViewModelFromDB()
        {
            // load the view model from isolated storage

            // Delete local storage
            //SettingsPage.appSettings.Clear();
            //SettingsPage.appSettings.Save();

            // Create the database if it does not exist.
            using (DatabaseContext db = new DatabaseContext(DatabaseContext.DBConnectionString))
            {
                //db.DeleteDatabase();
                if (db.DatabaseExists() == false)
                {
                    // Create the local database.
                    db.CreateDatabase();
                }
            }

            // Create the ViewModel object.
            ViewModel = new MainViewModel();

            // Query the local database and load observable collections.
            ViewModel.LoadCollectionsFromDatabase();
        }

        private void SaveViewModel()
        {
            //PhoneApplicationService.Current.State["MainViewModel"] = ViewModel;
            //IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            //settings["ViewModel"] = ViewModel;
        }

        private void LoadViewModel()
        {
            /*IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            if (settings.Contains("ViewModel"))
            {
                ViewModel = settings["ViewModel"] as MainViewModel;
            }
            else
            {
                
            }*/

            LoadViewModelFromDB();

            /*if (PhoneApplicationService.Current.State.ContainsKey("MainViewModel"))
            {
                ViewModel = PhoneApplicationService.Current.State["MainViewModel"] as MainViewModel;
            }
            else
            {
                //LoadViewModelFromIsolatedStorage();
            }*/
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {

        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {

        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}