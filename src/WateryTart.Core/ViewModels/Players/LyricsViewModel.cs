using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ReactiveUI;

namespace WateryTart.Core.ViewModels.Players
{
    public class LyricLine : ReactiveObject
    {
        private bool _isActive;
        public TimeSpan Time { get; init; }
        public string Text { get; init; } = string.Empty;
        public bool IsActive { get => _isActive; set => this.RaiseAndSetIfChanged(ref _isActive, value); }
    }

    public class LyricsViewModel : ReactiveObject
    {
        public ObservableCollection<LyricLine> Lines { get; } = new();

        private int _currentLineIndex = -1;
        public int CurrentLineIndex { get => _currentLineIndex; private set => this.RaiseAndSetIfChanged(ref _currentLineIndex, value); }

        private static readonly Regex _timeTagRx = new(@"\[(\d+):(\d+(?:\.\d+)?)\]", RegexOptions.Compiled);
        private string? _lastLoadedContent;

        /// <summary>
        /// Loads LRC-style content. Accepts multiple time tags per line, e.g. "[00:01.00][00:02.00]Text".
        /// Deduplicates by (Time, Text) combination and skips if already loaded.
        /// </summary>
        public void LoadFromLrc(string lrcContent)
        {
            // Skip if already loaded with the same content
            if (_lastLoadedContent == lrcContent)
                return;

            _lastLoadedContent = lrcContent;
           // Lines.Clear();
            if (string.IsNullOrWhiteSpace(lrcContent)) { CurrentLineIndex = -1; return; }

            var lines = lrcContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var parsed = new HashSet<(TimeSpan Time, string Text)>();

            foreach (var line in lines)
            {
                var matches = _timeTagRx.Matches(line);
                if (matches.Count == 0)
                    continue;

                var text = _timeTagRx.Replace(line, "").Trim();
                if (string.IsNullOrWhiteSpace(text))
                    continue;

                foreach (Match m in matches)
                {
                    if (!int.TryParse(m.Groups[1].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var minutes))
                        continue;
                    if (!double.TryParse(m.Groups[2].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var seconds))
                        continue;

                    var ts = TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
                    parsed.Add((ts, text));
                }
            }

            // Add unique entries sorted by time
            foreach (var (time, text) in parsed.OrderBy(x => x.Time))
                Lines.Add(new LyricLine { Time = time, Text = text });

            CurrentLineIndex = -1;
        }

        public int FindCurrentIndex(TimeSpan position)
        {
            int lo = 0, hi = Lines.Count - 1, result = -1;
            while (lo <= hi)
            {
                var mid = (lo + hi) >> 1;
                if (Lines[mid].Time <= position) { result = mid; lo = mid + 1; } else hi = mid - 1;
            }

            return result;
        }

        public void UpdateActiveLine(TimeSpan position)
        {
            var idx = FindCurrentIndex(position);
            if (idx == CurrentLineIndex)
                return;

            if (CurrentLineIndex >= 0 && CurrentLineIndex < Lines.Count)
                Lines[CurrentLineIndex].IsActive = false;

            CurrentLineIndex = idx;

            if (idx >= 0 && idx < Lines.Count)
                Lines[idx].IsActive = true;
        }

        public void Clear()
        {
            _lastLoadedContent = null;
            Lines.Clear();
            CurrentLineIndex = -1;
        }
    }
}
