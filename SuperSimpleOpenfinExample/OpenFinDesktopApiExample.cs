using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Openfin.Desktop;
using System.Runtime.InteropServices;
using System.IO;

namespace SuperSimpleOpenfinExample
{
    class OpenFinDesktopApiExample : DesktopStateListener
    {
        private DesktopConnection openFinConnection;

        public delegate void stringParamDelegate(string str);
        public delegate void appicationParamDelegate(Application application);

        public const string OPENFIN_HOST = "127.0.0.1";
        public const int OPENFIN_PORT = 9696;
        public const string OPENFIN_VERSION = "stable";

        public void Start()
        {
            openFinConnection = new DesktopConnection("c# Embed OpenFin Window", OPENFIN_HOST, OPENFIN_PORT);
            openFinConnection.connectToVersion(OPENFIN_VERSION, this, true, 9090);
        }
        private void createApplication(string name, string url, appicationParamDelegate callback)
        {
            //We will create a hidden frameless application
            var appOptions = new ApplicationOptions(name, name, url);

            appOptions.MainWindowOptions.AutoShow = true;

            var app = new Openfin.Desktop.Application(appOptions, openFinConnection);

            //Make sure the application is ready before we execute the callback
            onApplicationReady(app, (a, b, c) =>
            {
                callback(app);
            });

            app.run();
        }

        private void onApplicationReady(Application application, InterAppMessageHandler callback)
        {
            //The Application will send a ready message once its done bootstrappning.
            openFinConnection
                .getInterApplicationBus()
                .subscribe(application.getUuid(), "application:ready", callback);

        }

        public void onClosed()
        {
            Console.WriteLine("Closed");
        }

        public void onError(string reason)
        {
            Console.WriteLine(reason);
        }

        public void onMessage(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            //nothing of interest.
        }

        public void onOutgoingMessage(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            //nothing of interest.
        }

        public void subscribeToMessages(Application app)
        {
            var interApplicationBus = openFinConnection.getInterApplicationBus();

            interApplicationBus.subscribe(senderUuid: app.getUuid(), topic: "chart-click", listener: (sender, topic, message) =>  
            {
                Console.WriteLine(message);
            });
        }

        public void onReady()
        {
            Console.WriteLine("OpenFin is Ready");

            createApplication(name: "charts", url: "http://cdn.openfin.co/embed-web/chart.html", callback:(app) =>
            {
                Console.WriteLine("Application created");
                subscribeToMessages(app);
            });
        }
    }
}