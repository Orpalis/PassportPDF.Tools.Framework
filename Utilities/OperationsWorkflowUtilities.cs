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
using PassportPDF.Tools.Framework.Business;
using PassportPDF.Tools.Framework.Configuration;
using static PassportPDF.Model.PDFReduceParameters;

namespace PassportPDF.Tools.Framework.Utilities
{
    public static class OperationsWorkflowUtilities
    {
        public static OperationsWorkflow CreatePDFReductionWorkflow(ReduceActionConfiguration reduceActionConfiguration)
        {
            List<Operation> actionsToBePerformed = new List<Operation>
            {
                new Operation(Operation.OperationType.Load, reduceActionConfiguration.OutputVersion),
                new Operation(Operation.OperationType.Reduce, reduceActionConfiguration),
                new Operation(Operation.OperationType.Save)
            };

            return new OperationsWorkflow(actionsToBePerformed);
        }


        public static OperationsWorkflow CreatePDFOCRWorkflow(OCRActionConfiguration ocrActionConfiguration)
        {
            List<Operation> actionsToBePerformed = new List<Operation>
            {
                new Operation(Operation.OperationType.Load, OutputVersionEnum.PdfVersionRetainExisting),
                new Operation(Operation.OperationType.OCR, ocrActionConfiguration),
                new Operation(Operation.OperationType.Save)
            };

            return new OperationsWorkflow(actionsToBePerformed);
        }


        public static bool IsFileSizeReductionIntended(OperationsWorkflow operationsWorkflow)
        {
            foreach (var operation in operationsWorkflow.OperationsToBePerformed)
            {
                if (operation.Type == Operation.OperationType.Reduce)
                {
                    return true;
                }
            }

            return false;
        }
    }
}