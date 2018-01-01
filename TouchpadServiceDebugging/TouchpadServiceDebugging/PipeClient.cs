using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Pipes;

namespace TouchpadServiceDebugging {
    
    class PipeClient : IDisposable {
        private NamedPipeClientStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private bool disposed;

        public PipeClient(string pipeName) {
            this.stream = new NamedPipeClientStream(pipeName);
            this.writer = new StreamWriter(stream);
            this.reader = new StreamReader(stream);
            this.disposed = false;
        }

        public void Connect() {
            this.stream.Connect();
        }

        public void SendData(string data) {
            writer.Write(data);
            writer.Flush();
        }

        public string ReceiveData() {
            return reader.ReadLine();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) {
            if (!this.disposed) {
                if (disposing) {
                    this.stream.Dispose();
                    this.reader.Dispose();
                    this.writer.Dispose();
                }
                this.disposed = true;
            }
        }

    }
}
