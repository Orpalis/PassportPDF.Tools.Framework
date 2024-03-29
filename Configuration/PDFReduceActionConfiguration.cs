﻿/**********************************************************************
 * Project:                 PassportPDF.Tools.Framework
 * Authors:                 - Evan Carrère.
 *                          - Loïc Carrère.
 *
 * (C) Copyright 2018, ORPALIS.
 ** Licensed under the Apache License, Version 2.0 (the "License");
 ** you may not use this file except in compliance with the License.
 ** You may obtain a copy of the License at
 ** http://www.apache.org/licenses/LICENSE-2.0
 ** Unless required by applicable law or agreed to in writing, software
 ** distributed under the License is distributed on an "AS IS" BASIS,
 ** WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 ** See the License for the specific language governing permissions and
 ** limitations under the License.
 *
 **********************************************************************/

using PassportPDF.Model;

namespace PassportPDF.Tools.Framework.Configuration
{
    public sealed class PDFReduceActionConfiguration
    {
        public PdfVersion OutputVersion { get; set; } = PdfVersion.PdfVersion15;

        public ImageQuality ImageQuality { get; set; } = ImageQuality.ImageQualityMedium;

        public bool RecompressImages { get; set; } = true;

        public bool EnableColorDetection { get; set; } = true;

        public bool PackDocument { get; set; } = true;

        public bool PackFonts { get; set; } = true;

        public bool DownscaleImages { get; set; } = true;

        public int DownscaleResolution { get; set; } = 150;

        public bool FastWebView { get; set; } = false;

        public bool RemoveFormFields { get; set; } = false;

        public bool RemoveAnnotations { get; set; } = false;

        public bool RemoveBookmarks { get; set; } = false;

        public bool RemoveHyperlinks { get; set; } = false;

        public bool RemoveEmbeddedFiles { get; set; } = false;

        public bool RemoveBlankPages { get; set; } = false;

        public bool RemoveJavaScript { get; set; } = false;

        public bool EnableJPEG2000 { get; set; } = true;

        public bool EnableJBIG2 { get; set; } = true;

        public bool EnableCharRepair { get; set; } = false;

        public bool EnableMRC { get; set; } = false;

        public bool MRCPreserveSmoothing { get; set; } = true;

        public int MRCDownscaleResolution { get; set; } = 100;

        public bool RemovePageThumbnails { get; set; } = false;

        public bool RemoveMetadata { get; set; } = false;

        public bool RemoveEmbeddedFonts { get; set; } = false;

        public float JBIG2PMSThreshold { get; set; } = 0.75F;

        public bool RemovePagePieceInfo { get; set; } = true;
    }
}