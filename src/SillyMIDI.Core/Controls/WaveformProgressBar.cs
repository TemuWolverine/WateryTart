using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace SillyMIDI.Core.Controls
{
    public class WaveformProgressBar : Control
    {
        public static readonly StyledProperty<IReadOnlyList<double>?> PeaksProperty = AvaloniaProperty.Register<WaveformProgressBar, IReadOnlyList<double>?>(nameof(Peaks));
        public static readonly StyledProperty<IBrush?> PlayedBrushProperty = AvaloniaProperty.Register<WaveformProgressBar, IBrush?>(nameof(PlayedBrush));
        public static readonly StyledProperty<double> ProgressProperty = AvaloniaProperty.Register<WaveformProgressBar, double>(nameof(Progress), defaultValue: 0.0);
        public static readonly StyledProperty<int> ResolutionProperty = AvaloniaProperty.Register<WaveformProgressBar, int>(nameof(Resolution), defaultValue: 0);
        public static readonly StyledProperty<IBrush?> UnplayedBrushProperty = AvaloniaProperty.Register<WaveformProgressBar, IBrush?>(nameof(UnplayedBrush));

        public IReadOnlyList<double>? Peaks
        {
            get => GetValue(PeaksProperty);
            set => SetValue(PeaksProperty, value);
        }

        public IBrush? PlayedBrush
        {
            get => GetValue(PlayedBrushProperty);
            set => SetValue(PlayedBrushProperty, value);
        }

        /// <summary>
        /// Playback progress from 0.0 to 1.0.
        /// </summary>
        public double Progress
        {
            get => GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        /// <summary>
        /// Waveform resolution level.
        ///
        /// 0 = full resolution
        /// 1 = 1 bar per 2 samples
        /// 2 = 1 bar per 4 samples
        /// 3 = 1 bar per 8 samples
        /// etc.
        /// </summary>
        public int Resolution
        {
            get => GetValue(ResolutionProperty);
            set => SetValue(ResolutionProperty, value);
        }

        public IBrush? UnplayedBrush
        {
            get => GetValue(UnplayedBrushProperty);
            set => SetValue(UnplayedBrushProperty, value);
        }

        static WaveformProgressBar()
        {
            AffectsRender<WaveformProgressBar>(PeaksProperty, ProgressProperty, ResolutionProperty, PlayedBrushProperty, UnplayedBrushProperty);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (Peaks == null || Peaks.Count == 0)
                return;

            double width = Bounds.Width;
            double height = Bounds.Height;

            if (width <= 0 || height <= 0)
                return;

            double centreY = height / 2.0;

            double progress = Math.Clamp(Progress / 100, 0.0, 1.0);
            double progressX = width * progress;

            int resolution = Math.Max(0, Resolution);

            // Resolution 0 = 1 sample per bar
            // Resolution 1 = 2 samples per bar
            // Resolution 2 = 4 samples per bar
            // etc.
            int stride = 1 << resolution;

            int barCount = (Peaks.Count + stride - 1) / stride;

            // No gaps between bars.
            double barWidth = width / barCount;

            for (int bar = 0; bar < barCount; bar++)
            {
                int start = bar * stride;
                int end = Math.Min(start + stride, Peaks.Count);

                // Preserve transient peaks when reducing resolution.
                double peak = 0.0;

                for (int i = start; i < end; i++)
                {
                    peak = Math.Max(peak, Peaks[i]);
                }

                peak = Math.Clamp(peak, 0.0, 1.0);

                // Boost quieter sections visually.
                peak = Math.Sqrt(peak);

                double barHeight = Math.Max(2.0, peak * height);
                double x = bar * barWidth;
                double y = centreY - (barHeight / 2.0);

                var brush = x < progressX ? PlayedBrush : UnplayedBrush;

                if (brush == null)
                    continue;

                context.FillRectangle(brush, new Rect(x, y, barWidth, barHeight));
            }
        }
    }
}