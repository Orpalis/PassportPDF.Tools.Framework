/**********************************************************************
 * Project:                 PassportPDF.Tools.Framework
 * Authors:					- Evan Carrère.
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
    public sealed class ReduceActionConfiguration
    {
        public PDFReduceParameters.OutputVersionEnum OutputVersion { get; set; } = PDFReduceParameters.OutputVersionEnum.PdfVersion15;
        public PDFReduceParameters.ImageQualityEnum ImageQuality { get; set; } = PDFReduceParameters.ImageQualityEnum.ImageQualityMedium;
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
        public bool EnableJPEG2000 { get; set; } = true;
        public bool EnableJBIG2 { get; set; } = true;
        public bool EnableCharRepair { get; set; } = false;
        public PDFReduceParameters.ScannerSourceEnum ScannerSource { get; set; } = PDFReduceParameters.ScannerSourceEnum.PDFScannerSourceUnknown;
        public bool EnableMRC { get; set; } = false;

    }
}