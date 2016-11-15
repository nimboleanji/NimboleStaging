using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIMBOLE.Common
{
    public class ImportExport
    {
        public static Dictionary<string, string> GetSheetValues(WorkbookPart wbPart, Worksheet wsSheet, int rowIndex, Dictionary<string, int> labels)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            string[] colLabels = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            foreach (KeyValuePair<string, int> labelPair in labels)
            {
                Cell cell = wsSheet.Descendants<Cell>().Where(c => c.CellReference == colLabels[labelPair.Value] + rowIndex.ToString().Trim()).FirstOrDefault();
                string value = "";
                if (cell != null)
                {
                    value = cell.InnerText;
                    if (cell.DataType != null)
                    {
                        if (cell.DataType == CellValues.SharedString)
                        {
                            value = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault().SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                        }
                    }
                }
                values.Add(labelPair.Key, value);
            }
            return values;
        }
        public static Dictionary<string, int> SetSheetLabels(WorkbookPart wbPart, Worksheet wsSheet, int rowIndex, Dictionary<string, int> labels)
        {
            Dictionary<string, int> results = new Dictionary<string, int>();
            string[] colLabels = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

            int colLabelIndex = 0;
            foreach (string strFindLabel in labels.Keys)
            {
                Cell cell = wsSheet.Descendants<Cell>().Where(c => c.CellReference == colLabels[colLabelIndex] + rowIndex.ToString().Trim()).FirstOrDefault();
                string value = "";
                if (cell != null)
                {
                    value = cell.InnerText;
                    if (cell.DataType != null)
                    {
                        if (cell.DataType == CellValues.SharedString)
                        {
                            value = wbPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault()
                                .SharedStringTable
                         .ElementAt(int.Parse(value)).InnerText;
                        }
                    }
                }
                if (value.Equals(strFindLabel))
                {
                    results.Add(value, colLabelIndex);
                }
                else
                {
                    //Clearing the results dictionary and adding the value
                    results.Clear();
                    results.Add("Invalid", colLabelIndex);
                    break;
                }
                colLabelIndex++;

            }
            return results;
        }
    }
}
