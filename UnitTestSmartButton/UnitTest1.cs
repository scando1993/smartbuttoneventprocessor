using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecieveEPHClient;
using SendTestClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestSmartButton
{
    [TestClass]
    public class UnitTest
    {

        [TestMethod]
        public void EnviarMensajesEventHub()
        {
            SendClient sclient = new SendClient();

            Console.WriteLine("Enviando eventos");
            sclient.MainAsync(MainClient.DummyData).GetAwaiter().GetResult();

            Assert.IsTrue(sclient.mensajesEnviados == MainClient.DummyData.Count, $"Se encontro un problema, mensajes enviados: {sclient.mensajesEnviados}.");
        }

        [TestMethod]
        public void ProcesarMensajesEventHub() {
            EventProcessor eProcessor = new EventProcessor();
            eProcessor.MainAsync(20000).GetAwaiter().GetResult();

            Assert.IsTrue(EventProcessor.events == MainClient.DummyData.Count, $"No se han procesado todos los mensajes. Eventos procesados: {EventProcessor.events}");
        }
    }
}
