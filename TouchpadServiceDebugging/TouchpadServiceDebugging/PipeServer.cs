using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;

namespace TouchpadServiceDebugging {
    class PipeServer : IDisposable{
        private NamedPipeServerStream stream;
        private StreamWriter writer;
        private StreamReader reader;
        private bool disposed;

        public PipeServer(string pipeName) {
            this.stream = new NamedPipeServerStream(pipeName);
            this.reader = new StreamReader(stream);
            this.writer = new StreamWriter(stream);
        }

        public void WaitForConncetion() {
            this.stream.WaitForConnection();
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
