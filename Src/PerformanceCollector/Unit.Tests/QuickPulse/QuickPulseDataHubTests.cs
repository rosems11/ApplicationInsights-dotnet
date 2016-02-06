﻿namespace Unit.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights.Extensibility.PerfCounterCollector.Implementation.QuickPulse;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    
    [TestClass]
    public class QuickPulseDataHubTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            QuickPulseDataHub.ResetInstance();
        }

        [TestMethod]
        public void QuickPulseDataHubLocksInSampleCorrectly()
        {
            // ARRANGE
            QuickPulseDataHub.Instance.CurrentDataSampleReference.AIRequestCount = 5;

            // ACT
            var completedSample = QuickPulseDataHub.Instance.CompleteCurrentDataSample();

            // ASSERT
            Assert.AreEqual(5, completedSample.AIRequestCount);
            Assert.AreEqual(0, QuickPulseDataHub.Instance.CurrentDataSampleReference.AIRequestCount);

            Assert.AreSame(completedSample, QuickPulseDataHub.Instance.CompletedDataSample);
            Assert.AreNotSame(completedSample, QuickPulseDataHub.Instance.CurrentDataSampleReference);

            Assert.AreNotSame(QuickPulseDataHub.Instance.CompletedDataSample, QuickPulseDataHub.Instance.CurrentDataSampleReference);
        }

        [TestMethod]
        public void QuickPulseDataHubLocksInSampleCorrectlyMultithreaded()
        {
            // ARRANGE
            int taskCount = 1000;
            var writeTasks = new List<Task>(taskCount);
            var pause = TimeSpan.FromMilliseconds(10);

            for (int i = 0; i < taskCount; i++)
            {
                var task = new Task(() =>
                {
                    Interlocked.Increment(ref QuickPulseDataHub.Instance.CurrentDataSampleReference.AIRequestCount);

                    // sleep to increase the probability of sample completion happening right now
                    Thread.Sleep(pause);

                    Interlocked.Increment(ref QuickPulseDataHub.Instance.CurrentDataSampleReference.AIDependencyCallCount);
                });

                writeTasks.Add(task);
            }

            var completionTask = new Task(() =>
            {
                // sleep to increase the probability of more write tasks being between the two writes
                Thread.Sleep(TimeSpan.FromTicks(pause.Ticks / 2));

                QuickPulseDataHub.Instance.CompleteCurrentDataSample();
            });

            // shuffle the completion task into the middle of the pile to have it fire roughly halfway through
            writeTasks.Insert(writeTasks.Count / 2, completionTask);

            // ACT
            var sample1 = QuickPulseDataHub.Instance.CurrentDataSampleReference;

            var result = Parallel.For(0, writeTasks.Count, new ParallelOptions() { MaxDegreeOfParallelism = taskCount }, i => writeTasks[i].RunSynchronously());

            while (!result.IsCompleted)
            {
            }

            var sample2 = QuickPulseDataHub.Instance.CurrentDataSampleReference;

            // ASSERT
            // we expect some "telemetry items" to get "sprayed" over the two neighboring samples
            Assert.IsTrue(sample1.AIRequestCount > sample1.AIDependencyCallCount);
            Assert.IsTrue(sample2.AIRequestCount < sample2.AIDependencyCallCount);

            // overall numbers should match exactly
            Assert.AreEqual(taskCount, sample1.AIRequestCount + sample2.AIRequestCount);
            Assert.AreEqual(taskCount, sample1.AIDependencyCallCount + sample2.AIDependencyCallCount);
        }
    }
}
