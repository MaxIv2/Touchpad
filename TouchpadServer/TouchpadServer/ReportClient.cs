using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.Diagnostics;

namespace TouchpadServer {
    sealed class ReportClient {
        private const string authUri = "https://cryptic-lowlands-58542.herokuapp.com/api/user/authenticate";
        private const string addActionUri = "https://cryptic-lowlands-58542.herokuapp.com/api/action/addAction";
        private const string getUri = "https://cryptic-lowlands-58542.herokuapp.com/api/user/profile";
        private const string authenticationMsg = "{{\"username\":\"{0}\",\"password\":\"{1}\"}}";
        private JavaScriptSerializer serializer;
        private Queue<Command> commands;
        private HttpClient client;
        private static ReportClient instance;
        private bool authenticated;

        public static ReportClient Instance {
            get {
                if (instance == null) {
                    instance = new ReportClient();
                }
                return instance;
            }
        }

        private ReportClient() {
            this.serializer = new JavaScriptSerializer();
            this.client = new HttpClient();
            this.commands = new Queue<Command>();
            this.authenticated = false;
        }

        public void EnqueueCommand(Command c) {
            if (!Properties.Settings.Default.SendToServer || !authenticated)
                return;
            lock (commands) {
                commands.Enqueue(c);
            }
            if (commands.Count >= 50) {
                SendCommandsToServer();
            }
        }

        private async Task SendCommandsToServer() {
            string json = null;
            lock (commands) {
                json = "{\"items\":" + serializer.Serialize(commands) + "}";
                commands.Clear();
            }
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            try {
                response = await client.PostAsync(addActionUri, content);
                string result = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(result);
            }
            catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
            finally {
                content.Dispose();
                if (response != null)
                    response.Dispose();
            }
        }

        public async Task<bool> Authenticate(string user, string pass) {
            this.authenticated = false;
            this.client.DefaultRequestHeaders.Authorization = null;
            string authMsg = String.Format(authenticationMsg, user, pass);
            StringContent content = new StringContent(authMsg, Encoding.UTF8, "application/json");
            HttpResponseMessage response = null;
            try {
                response = await client.PostAsync(authUri, content);
                string result = await response.Content.ReadAsStringAsync();
                Dictionary<object, object> dict = serializer.Deserialize<Dictionary<object, object>>(result);
                if ((bool)dict["success"]) {
                    string[] split = ((string)dict["token"]).Split(' ');
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(split[0], split[1]);
                    this.authenticated = true;
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
                return false;
            }
            finally {
                content.Dispose();
                if (response != null)
                    response.Dispose();
            }
        }

        public async Task<string> GetStuff() {
            while (!authenticated) { }
            HttpResponseMessage response = await client.GetAsync(getUri);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
