using System.Windows;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;

namespace TRWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var cultureInfo = new CultureInfo("ru-RU");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            base.OnStartup(e);
        }

        Mutex mutex;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Run one copy app
            string mutName = "TRWpf";
            mutex = new Mutex(true, mutName, out bool create);
            if (!create)
                this.Shutdown();            
        }
    }
}
