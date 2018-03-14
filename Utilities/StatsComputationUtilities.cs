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

using System;

namespace PassportPDF.Tools.Framework.Utilities
{
    public static class StatsComputationUtilities
    {
        public static double ComputeReductionRatio(double inputSize, double outputSize)
        {
            double percent = (inputSize > 0 ? (double)outputSize / inputSize * 100 : 100);

            return Math.Round(percent, 2);
        }


        public static double ComputeReductionPercentage(double inputSize, double outputSize)
        {
            double percent = inputSize > 0 ? Math.Round(outputSize / inputSize * 100, 2) : 100;
            if (percent == 100 && inputSize > 0)
            {
                percent = Math.Round(outputSize / inputSize * 100, 4);
            }
            return percent;
        }


        public static double ComputeSavedSpaceRatio(double inputSize, double outputSize)
        {
            return Math.Round(100 - (inputSize > 0 ? outputSize / inputSize * 100 : 100), 2);
        }
    }
}
