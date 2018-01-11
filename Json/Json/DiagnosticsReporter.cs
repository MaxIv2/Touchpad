using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Script.Serialization;

namespace Json {
    class DiagnosticsReporter {
        private HttpClient client;
        private string serverUri;
        private JavaScriptSerializer serializer;
        private Queue<Item> items;

        public DiagnosticsReporter(string serverUri) {
            client = new HttpClient();
            this.serverUri = serverUri;
            serializer = new JavaScriptSerializer();
            items = new Queue<Item>();
        }

        public void AddItem(Item item) {
            items.Enqueue(item);
            if (items.Count > 10) {
                Task t = new Task(HandleActions);
                t.Start();
            }
        }

        public async void HandleActions() {
            string jsons = "\"items\":" + serializer.Serialize(items);
            StringContent content = new StringContent(jsons, Encoding.ASCII, "application/json");
            HttpResponseMessage r =  await client.PostAsync(this.serverUri, content);
            await r.Content.ReadAsStringAsync();
        }
    }
}
