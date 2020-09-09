using UnityEditor;
using UnityEngine;

namespace XLSReader.Example {
    public static class XLSReaderExample {
        [MenuItem("Tools/XLSReader/Test ReadWrite")]
        static void ReadWrite() {
            Excel xls = new Excel();
            ExcelTable table = new ExcelTable();
            table.TableName = "test";
            string outputPath = Application.dataPath + "/VavilichevGD/Tools/XLSReader/Example/Test2.xlsx";
            xls.Tables.Add(table);
            xls.Tables[0].SetValue(1, 1, "1");
            xls.Tables[0].SetValue(1, 2, "2");
            xls.Tables[0].SetValue(2, 1, "3");
            xls.Tables[0].SetValue(2, 2, "4");
            xls.ShowLog();
            ExcelHelper.SaveExcel(xls, outputPath);
        }

        [MenuItem("Tools/XLSReader/Test Read Test4")]
        static void Read() {
            string path = Application.dataPath + "/VavilichevGD/Tools/XLSReader/Example/Test4.xlsx";
            Excel xls = ExcelHelper.LoadExcel(path);
            xls.ShowLog();
        }

        [MenuItem("Tools/XLSReader/Test Read read5")]
        static void Read5() {
            string path = Application.dataPath + "/VavilichevGD/Tools/XLSReader//Example/Test5.xlsx";
            Excel xls = ExcelHelper.LoadExcel(path);
            xls.ShowLog();
        }
    }
}