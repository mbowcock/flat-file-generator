using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace FlatFile
{
    class FlatFile<T>
    {
        public void writeFile(List<T> records, string filePath)
        {
            // Make sure there are records to write
            if (records.Count > 0)
            {
                Type t = records[0].GetType();
                PropertyInfo[] pi = t.GetProperties();

                using (StreamWriter writer = File.CreateText(filePath))
                {
                    foreach (T record in records)
                    {
                        string dataRow = string.Empty;

                        foreach (PropertyInfo p in pi)
                        {
                            object[] attributes = p.GetCustomAttributes(typeof(FlatFileAttribute), false);
                            FlatFileAttribute ffa = null;
                            int start = 0, length = 0;

                            if (attributes.Length > 0)
                            {
                                ffa = (FlatFileAttribute)attributes[0];
                            }

                            if (ffa != null)
                            {
                                start = ffa.startPosition;
                                length = ffa.fieldLength;
                            }

                            string outputString = p.GetValue(record, null).ToString();
                            if (outputString.Length < length)
                            {
                                outputString = outputString.PadRight(length);
                            }

                            if (outputString.Length == length)
                            {
                                if (dataRow.Length < start)
                                {
                                    dataRow += outputString;
                                }
                                else
                                {
                                    // current field falls inside the middle of the existing output string.
                                    // split based on start position and then concatenate
                                    string leftSide = string.Empty, rightSide = string.Empty;
                                    leftSide = dataRow.Substring(0, start - 1);
                                    rightSide = dataRow.Substring(start, dataRow.Length);
                                    dataRow = leftSide + outputString + rightSide;
                                }
                            }
                        }

                        // write to file
                        writer.WriteLine(dataRow);
                    }
                }
            }
        }
    }

}
