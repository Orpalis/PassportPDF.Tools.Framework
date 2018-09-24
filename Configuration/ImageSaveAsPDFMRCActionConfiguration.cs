/**********************************************************************
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
    public sealed class ImageSaveAsPDFMRCActionConfiguration
    {
        public ImageSaveAsPDFMRCParameters.ConformanceEnum Conformance { get; set; } = ImageSaveAsPDFMRCParameters.ConformanceEnum.PDF15;

        public ImageSaveAsPDFMRCParameters.ColorImageCompressionEnum ColorImageCompression { get; set; } = ImageSaveAsPDFMRCParameters.ColorImageCompressionEnum.JPEG;

        public ImageSaveAsPDFMRCParameters.BitonalImageCompressionEnum BitonalImageCompression { get; set; } = ImageSaveAsPDFMRCParameters.BitonalImageCompressionEnum.JBIG2;

        public int ImageQuality { get; set; } = 55;

        public bool DownscaleImages { get; set; } = true;

        public int DownscaleResolution { get; set; } = 150;

        public bool PreserveSmoothing { get; set; } = true;

        public bool FastWebView { get; set; } = false;
    }
}