﻿namespace Microsoft.ApplicationInsights
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Assert = Xunit.Assert;
    using Extensibility.Implementation;
    using TestFramework;

    /// <summary>
    /// Tests corresponding to TelemetryClientExtension methods.
    /// </summary>
    [TestClass]
    public class TelemetryClientExtensionAsyncTests
    {
        private TelemetryClient telemetryClient;
        private List<ITelemetry> sendItems;

        [TestInitialize]
        public void TestInitialize()
        {
            var configuration = new TelemetryConfiguration();
            this.sendItems = new List<ITelemetry>();
            configuration.TelemetryChannel = new StubTelemetryChannel { OnSend = item => this.sendItems.Add(item) };
            configuration.InstrumentationKey = Guid.NewGuid().ToString();
            configuration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            this.telemetryClient = new TelemetryClient(configuration);
            CallContextHelpers.SaveOperationContext(null);
        }

        /// <summary>
        /// Ensure that context being propagated via async/await.
        /// </summary>
        [TestMethod]
        public void ContextPropagatesThruAsyncAwait()
        {
            var task = this.TestAsync();
            task.Wait();
        }

        /// <summary>
        /// Actual async test method.
        /// </summary>
        /// <returns>Task to await.</returns>
        public async Task TestAsync()
        {
            using (var op = this.telemetryClient.StartOperation<RequestTelemetry>("request"))
            {
                var id1 = Thread.CurrentThread.ManagedThreadId;
                this.telemetryClient.TrackTrace("trace1");

                //HttpClient client = new HttpClient();
                await Task.Delay(100);//client.GetStringAsync("http://bing.com");

                var id2 = Thread.CurrentThread.ManagedThreadId;
                this.telemetryClient.TrackTrace("trace2");

                Assert.NotEqual(id1, id2);
            }

            Assert.Equal(3, this.sendItems.Count);
            var id = ((RequestTelemetry)this.sendItems[this.sendItems.Count - 1]).Id;
            Assert.False(string.IsNullOrEmpty(id));

            foreach (var item in this.sendItems)
            {
                if (item is TraceTelemetry)
                {
                    Assert.Equal(id, item.Context.Operation.ParentId);
                    Assert.Equal(id, item.Context.Operation.Id);
                }
                else
                {
                    Assert.Equal(id, ((RequestTelemetry)item).Id);
                    Assert.Equal(id, item.Context.Operation.Id);
                    Assert.Null(item.Context.Operation.ParentId);
                }
            }
        }

        /// <summary>
        /// Ensure that context being propagated via Begin/End.
        /// </summary>
        [TestMethod]
        public void ContextPropagatesThruBeginEnd()
        {
            var op = this.telemetryClient.StartOperation<RequestTelemetry>("request");
            var id1 = Thread.CurrentThread.ManagedThreadId;
            int id2 = 0;
            this.telemetryClient.TrackTrace("trace1");

            HttpWebRequest request = WebRequest.Create(new Uri("http://bing.com")) as HttpWebRequest;
            var result = request.BeginGetResponse(
                (r) =>
                    {
                        id2 = Thread.CurrentThread.ManagedThreadId;
                        this.telemetryClient.TrackTrace("trace2");

                        this.telemetryClient.StopOperation(op);

                        request.EndGetResponse(r);
                    },
                null);

            while (!result.IsCompleted)
            {
                Thread.Sleep(10);
            }

            Thread.Sleep(100);

            Assert.NotEqual(id1, id2);

            Assert.Equal(3, this.sendItems.Count);
            var id = ((RequestTelemetry)this.sendItems[this.sendItems.Count - 1]).Id;
            Assert.False(string.IsNullOrEmpty(id));

            foreach (var item in this.sendItems)
            {
                if (item is TraceTelemetry)
                {
                    Assert.Equal(id, item.Context.Operation.ParentId);
                    Assert.Equal(id, item.Context.Operation.Id);
                }
                else
                {
                    Assert.Equal(id, ((RequestTelemetry)item).Id);
                    Assert.Equal(id, item.Context.Operation.Id);
                    Assert.Null(item.Context.Operation.ParentId);

                }
            }
        }
    }
}
