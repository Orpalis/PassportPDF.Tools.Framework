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

using System.Collections.Generic;
using PassportPDF.Model;
using PassportPDF.Tools.Framework.Business;
using PassportPDF.Tools.Framework.Configuration;

namespace PassportPDF.Tools.Framework.Utilities
{
    public static class OperationsWorkflowUtilities
    {
        public static OperationsWorkflow CreatePDFReductionWorkflow(PDFReduceActionConfiguration reduceActionConfiguration)
        {
            List<Operation> actionsToBePerformed = new List<Operation>
            {
                new Operation(Operation.OperationType.LoadPDF, reduceActionConfiguration.OutputVersion),
                new Operation(Operation.OperationType.ReducePDF, reduceActionConfiguration),
                new Operation(Operation.OperationType.SavePDF)
            };

            return new OperationsWorkflow(actionsToBePerformed);
        }


        public static OperationsWorkflow CreatePDFOCRWorkflow(PDFOCRActionConfiguration ocrActionConfiguration)
        {
            List<Operation> actionsToBePerformed = new List<Operation>
            {
                new Operation(Operation.OperationType.LoadPDF, PDFReduceParameters.OutputVersionEnum.PdfVersionRetainExisting),
                new Operation(Operation.OperationType.OCRPDF, ocrActionConfiguration),
                new Operation(Operation.OperationType.SavePDF)
            };

            return new OperationsWorkflow(actionsToBePerformed);
        }


        public static OperationsWorkflow CreateImageToPDFMRCWorkflow(ImageSaveAsPDFMRCActionConfiguration saveAsPdfMrcActionConfiguration)
        {
            List<Operation> actionsToBePerformed = new List<Operation>
            {
                new Operation(Operation.OperationType.LoadImage),
                new Operation(Operation.OperationType.SaveImageAsPDFMRC, saveAsPdfMrcActionConfiguration)
            };

            return new OperationsWorkflow(actionsToBePerformed);
        }


        public static bool IsFileSizeReductionIntended(OperationsWorkflow operationsWorkflow)
        {
            foreach (var operation in operationsWorkflow.OperationsToBePerformed)
            {
                if (operation.Type == Operation.OperationType.ReducePDF)
                {
                    return true;
                }
            }

            return false;
        }
    }
}