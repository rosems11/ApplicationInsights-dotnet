﻿namespace Microsoft.ApplicationInsights.Metrics
{
    using System;
    using System.Collections.Generic;

    /// <summary>@ToDo: Complete documentation before stable release. {218}</summary>
    public sealed class MetricConfigurationForMeasurement : MetricConfiguration
    {
        /// <summary>Creates a new instance of <c>MetricConfigurationForMeasurement</c>.</summary>
        /// <param name="seriesCountLimit">How many data time series a metric can contain as a maximum.
        /// Once this limit is reached, calls to <c>TryTrackValue(..)</c>, <c>TryGetDataSeries(..)</c> and similar
        /// that would normally result in new series will return <c>false</c>.</param>
        /// <param name="valuesPerDimensionLimit">How many different values each of the dimensions of a metric can
        /// have as a maximum.
        /// Once this limit is reached, calls to <c>TryTrackValue(..)</c>, <c>TryGetDataSeries(..)</c> and similar
        /// that would normally result in new series will return <c>false</c>.</param>
        /// <param name="seriesConfig">The configuration for how each series of this metric should be aggregated.</param>
        public MetricConfigurationForMeasurement(int seriesCountLimit, int valuesPerDimensionLimit, MetricSeriesConfigurationForMeasurement seriesConfig)
            : base(seriesCountLimit, valuesPerDimensionLimit, seriesConfig)
        {
        }

        /// <summary>Creates a new instance of <c>MetricConfiguration</c>.</summary>
        /// <param name="seriesCountLimit">How many data time series a metric can contain as a maximum.
        /// Once this limit is reached, calls to <c>TryTrackValue(..)</c>, <c>TryGetDataSeries(..)</c> and similar
        /// that would normally result in new series will return <c>false</c>.</param>
        /// <param name="valuesPerDimensionLimits">How many different values each of the dimensions of a metric can
        /// have as a maximum. If this enumeration contains less elements than the number of supported dimensions,
        /// then the last specified element is replicated for subsequent dimensions. If this enumeration contains
        /// too many elements, superflous elements are ignored.
        /// Once this limit is reached, calls to <c>TryTrackValue(..)</c>, <c>TryGetDataSeries(..)</c> and similar
        /// that would normally result in new series will return <c>false</c>.</param>
        /// <param name="seriesConfig">The configuration for how each series of this metric should be aggregated.</param>
        public MetricConfigurationForMeasurement(int seriesCountLimit, IEnumerable<int> valuesPerDimensionLimits, MetricSeriesConfigurationForMeasurement seriesConfig)
            : base(seriesCountLimit, valuesPerDimensionLimits, seriesConfig)
        {
        }
    }
}