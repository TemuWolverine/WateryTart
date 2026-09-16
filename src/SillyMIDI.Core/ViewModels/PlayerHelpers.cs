using SillyMIDI.Core.ViewModels.Players;
using System;
using System.Collections.Generic;
using SillyMIDI.MusicAssistant.Models;

namespace SillyMIDI.Core.ViewModels
{
    public static class PlayerHelpers
    {
        public static QualityTier DetermineQuality(Streamdetails? streamDetails)
        {
            if (streamDetails == null || streamDetails.AudioFormat == null || string.IsNullOrEmpty(streamDetails.AudioFormat.ContentType))
                return QualityTier.LOW;
            if (streamDetails.AudioFormat.BitDepth > 16 || streamDetails.AudioFormat.SampleRate > 48000)
                return QualityTier.HIRES;
            else if (PlayerHelpers.IsContentTypeLossless(streamDetails.AudioFormat.ContentType))
                return QualityTier.HQ;
            else
                return QualityTier.LOW;
        }
        public static bool IsContentTypeLossless(string contentType)
        {
            return LosslessContentTypes.Contains(contentType);
        }

        public static readonly HashSet<string> LosslessContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "FLAC",
            "AIFF",
            "WAV",
            "ALAC",
            "WAVPACK",
            "TAK",
            "APE",
            "TRUEHD",
            "DSD_LSBF",
            "DSD_MSBF",
            "DSD_LSBF_PLANAR",
            "DSD_MSBF_PLANAR",
            "RA_144"
        };
    }
}