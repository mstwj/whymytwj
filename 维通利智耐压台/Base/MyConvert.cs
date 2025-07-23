using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;

using Application = Microsoft.Office.Interop.Word.Application;
using Shape = Microsoft.Office.Interop.Word.Shape;


namespace 维通利智耐压台.Base
{
    public static class MyConvert
    {
       
        public static void ExportToWordUsingInteropWord(string filePath,
            string Operate, string RecordDateTimer, string ProductNumber,
            string Temperature, string ProductParts, string ProductTuhao,
            string Airpressure, string ProductName, string ProductType,
            string ProductStardVotil, string ProductStardPartial,
            string LevelV1, string LevelTime1, string LevelPc1, string LevelIsGood1,
            string LevelV2, string LevelTime2, string LevelPc2, string LevelIsGood2,
            string LevelV3, string LevelTime3, string LevelPc3, string LevelIsGood3,
            string Savepng)
        {
            string fileName = @"D:\NaiYa\TemplateDoc.doc";
            Application wordApp = new Application();

            //这里现在已经OK了..
            Document doc = null;

            try
            {
                doc = wordApp.Documents.Open(fileName);

                // 获取文档中的第一张图片（假设是第一个InlineShape）
                if (doc.InlineShapes.Count > 0)
                {
                    //string newImagePath = @"D:\naiyabaogao\vscontent.png"; // 新图片的路径
                    string newImagePath = Savepng; // 新图片的路径

                    InlineShape oldPic = doc.InlineShapes[1]; // 注意：索引从1开始，不是0

                    // 删除旧图片
                    oldPic.Select();
                    wordApp.Selection.Delete();

                    // 插入新图片，并设置其位置和大小与旧图片相同                
                    Microsoft.Office.Interop.Word.Range range = doc.Content;
                    range.Collapse(WdCollapseDirection.wdCollapseEnd);
                    InlineShape newPic = doc.InlineShapes.AddPicture(newImagePath, Range: range);
                }

                ReplaceAll(doc, "A001", Operate);
                ReplaceAll(doc, "A002", Temperature);
                ReplaceAll(doc, "A003", Airpressure);
                ReplaceAll(doc, "A004", RecordDateTimer);
                ReplaceAll(doc, "A005", ProductParts);
                ReplaceAll(doc, "A006", ProductName);
                ReplaceAll(doc, "A007", ProductNumber);
                ReplaceAll(doc, "A008", ProductTuhao);
                ReplaceAll(doc, "A009", ProductType);
                ReplaceAll(doc, "A010", ProductStardVotil);
                ReplaceAll(doc, "A011", ProductStardPartial);

                ReplaceAll(doc, "T001", LevelV1);
                ReplaceAll(doc, "T002", LevelTime1);
                ReplaceAll(doc, "T003", LevelPc1);
                ReplaceAll(doc, "T004", LevelIsGood1);
                ReplaceAll(doc, "T005", LevelV2);
                ReplaceAll(doc, "T006", LevelTime2);
                ReplaceAll(doc, "T007", LevelPc2);
                ReplaceAll(doc, "T008", LevelIsGood2);
                ReplaceAll(doc, "T009", LevelV3);
                ReplaceAll(doc, "T010", LevelTime3);
                ReplaceAll(doc, "T011", LevelPc3);
                ReplaceAll(doc, "T012", LevelIsGood3);

                // 保存修改后的文档
                doc.SaveAs(filePath);
                //doc.Save();
                doc.Close(); // 关闭文档时可以选择保存更改
                wordApp.Quit(); // 退出Word应用程序                

            }
            catch (Exception ex)
            {
                if (doc != null)
                {
                    doc.Close(); // 关闭文档时可以选择保存更改
                    wordApp.Quit(); // 退出Word应用程序              
                }
                //MessageBox.Show(ex.Message);
                //return;
                throw;
            }
        }


        /// <summary>
        /// 替换所有符合的text
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="text"></param>
        /// <param name="replaceText"></param>
        public static void ReplaceAll(Microsoft.Office.Interop.Word.Document doc, string text, string replaceText)
        {
            //story ranges 里面保存了不同类型的story range， 每个story range可以通过NextStoryRange来获取同一类型下所有的story range。
            foreach (Microsoft.Office.Interop.Word.Range storyRange in doc.StoryRanges)
            {
                Microsoft.Office.Interop.Word.Range range = storyRange;
                while (range != null)
                {
                    ReplaceAllText(range, text, replaceText);
                    range = range.NextStoryRange;
                }
            }
        }


        private static void ReplaceAllText(Microsoft.Office.Interop.Word.Range range, string text, string replaceText)
        {
            Find find = range.Find;
            find.ClearFormatting();
            find.Replacement.ClearFormatting();
            find.Text = text;
            find.Replacement.Text = replaceText;
            find.Forward = true;
            find.Wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
            find.Format = false;
            find.MatchCase = true;
            find.MatchWholeWord = false;
            find.MatchWildcards = false;
            find.MatchSoundsLike = false;
            find.MatchAllWordForms = false;
            try
            {
                //log.WriteLine($"替换所有【{text}】->【{replaceText}】!");
                find.Execute(find.Text, find.MatchCase, find.MatchWholeWord, find.MatchWildcards, find.MatchSoundsLike, find.MatchAllWordForms,
                    find.Forward, find.Wrap, find.Format, find.Replacement.Text, Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll, false, false, false, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常!");
                //log.WriteException(ex, "ReplaceAll");
            }

        }


    }

        /*
        public static void ExportToWordUsingOpenXml(string filePath,
            string Operate, string RecordDateTimer, string ProductNumber,
            string Temperature, string ProductParts, string ProductTuhao,
            string Airpressure, string ProductName, string ProductType,
            string ProductStardVotil, string ProductStardPartial,
            string LevelV1, string LevelTime1, string LevelPc1, string LevelIsGood1,
            string LevelV2, string LevelTime2, string LevelPc2, string LevelIsGood2,
            string LevelV3, string LevelTime3, string LevelPc3, string LevelIsGood3,
            string Savepng)
        {
            try
            {
                // 创建一个新的文档实例
                Document doc = new Document();

                // 创建一个构建器来操作文档
                DocumentBuilder builder = new DocumentBuilder(doc);

                string Row1 = "测试员:" + Operate;
                string Row2 = "测试时间:" + RecordDateTimer;
                string Row3 = "产品编号:" + ProductNumber;
                string Row4 = "环境温度(°C):" + Temperature;
                string Row5 = "施压部位:" + ProductParts;
                string Row6 = "图号:" + ProductTuhao;
                string Row7 = "大气压(Pa):" + Airpressure;
                string Row8 = "产品名称:" + ProductName;
                string Row9 = "规格型号:" + ProductType;
                string Row10 = "电压标准(Kv):" + ProductStardVotil;
                string Row11 = "局放标准(pC):" + ProductStardPartial;

                // 使用DocumentBuilder插入文字
                builder.InsertParagraph(); // 插入一个新段落
                builder.Font.Bold = true; // 设置字体为粗体
                builder.Font.Size = 16; // 设置字体大小为14磅
                builder.Writeln("试验报告"); // 写入文字            
                builder.Writeln();

                builder.InsertParagraph(); // 插入一个新段落
                builder.Font.Bold = false; // 设置字体为粗体
                builder.Font.Size = 12; // 设置字体大小为14磅
                builder.Writeln(Row1); // 写入文字
                builder.Writeln(Row2); // 写入文字
                builder.Writeln(Row3); // 写入文字
                builder.Writeln(Row4); // 写入文字
                builder.Writeln(Row5); // 写入文字
                builder.Writeln(Row6); // 写入文字
                builder.Writeln(Row7); // 写入文字
                builder.Writeln(Row8); // 写入文字
                builder.Writeln(Row8); // 写入文字
                builder.Writeln(Row10); // 写入文字
                builder.Writeln(Row11); // 写入文字
                builder.Writeln(); // 写入文字

                // 开始插入表格，参数为行数和列数
                Table table = builder.StartTable();

                // 添加行和单元格
                builder.InsertCell(); // 添加一个单元格
                builder.Write("施压阶段"); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write("施加电压(kV)"); // 向另一个单元格中写入内容            
                builder.InsertCell(); // 添加一个单元格
                builder.Write("持续时间(S)"); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write("局放量(pC)"); // 向另一个单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write("结果判断"); // 向另一个单元格中写入内容
                builder.EndRow(); // 结束当前行

                // 添加行和单元格
                builder.InsertCell(); // 添加一个单元格
                builder.Write("第一阶段"); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelV1); // 向另一个单元格中写入内容            
                builder.InsertCell(); // 添加一个单元格
                builder.Write(LevelTime1); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelPc1); // 向另一个单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelIsGood1); // 向另一个单元格中写入内容
                builder.EndRow(); // 结束当前行

                // 添加行和单元格
                builder.InsertCell(); // 添加一个单元格
                builder.Write("第二阶段"); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelV2); // 向另一个单元格中写入内容            
                builder.InsertCell(); // 添加一个单元格
                builder.Write(LevelTime2); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelPc2); // 向另一个单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelIsGood2); // 向另一个单元格中写入内容
                builder.EndRow(); // 结束当前行

                // 添加行和单元格
                builder.InsertCell(); // 添加一个单元格
                builder.Write("第三阶段"); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelV3); // 向另一个单元格中写入内容            
                builder.InsertCell(); // 添加一个单元格
                builder.Write(LevelTime3); // 向单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelPc3); // 向另一个单元格中写入内容
                builder.InsertCell(); // 添加另一个单元格
                builder.Write(LevelIsGood3); // 向另一个单元格中写入内容
                builder.EndRow(); // 结束当前行

                // 结束表格的插入
                builder.EndTable();
                builder.Writeln(); // 写入文字


                // 指定图片的路径
                string imagePath = Savepng;

                // 插入图片到文档中
                Shape shape = builder.InsertImage(imagePath);

                // 可选：设置图片的尺寸
                shape.Width = 100; // 设置宽度为200磅（大约是72点）
                shape.Height = 100; // 设置高度为200磅（大约是72点）
                shape.WrapType = WrapType.Inline; // 设置图片环绕方式为无环绕

                string date = DateTime.Now.ToString("HH_mm_ss");
                // 保存文档
                doc.Save("D:\\耐压报表\\" + date + ".docx");
                Thread.Sleep(1000);
                File.Delete(Savepng);
                MessageBox.Show("保存docx文件完成..");


            }
            catch(Exception ex)
            {
                MessageBox.Show("文件导出失败.."+ ex.Message);
                return;
            }


            /*
            using (WordprocessingDocument document = WordprocessingDocument.Create(filePath, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                Document doc = new Document();

                Body body = new Body();

                string Row1 = "测试员:" + Operate;
                string Row2 = "测试时间:" + RecordDateTimer;
                string Row3 = "产品编号:" + ProductNumber;
                string Row4 = "环境温度(°C):" + Temperature; 
                string Row5 = "施压部位:" + ProductParts;
                string Row6 = "图号:" + ProductTuhao;
                string Row7 = "大气压(Pa):" + Airpressure;
                string Row8 = "产品名称:" + ProductName;
                string Row9 = "规格型号:" + ProductType;
                string Row10 = "电压标准(Kv):" + ProductStardVotil;
                string Row11 = "局放标准(pC):"+ ProductStardPartial;

                // 添加段落
                Paragraph para = new Paragraph(new Run(new Text(Row1)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row2)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row3)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row4)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row5)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row6)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row7)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row8)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row9)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row10)));
                body.Append(para);
                para = new Paragraph(new Run(new Text(Row11)));
                body.Append(para);


                // 创建表格属性
                //TableProperties tableProperties = new TableProperties();
                //TableWidth tableWidth = new TableWidth() { Width = "10000", Type = TableWidthUnitValues.Dxa }; // 5000是100的50倍，即50000 EMUs (English Metric Units)
                //tableProperties.Append(tableWidth);

                // 创建表格的开始部分
                Table table = new Table();
                table.AppendChild(new TableProperties(
                     new TableWidth() { Width = "8000", Type = TableWidthUnitValues.Dxa },
                        new TableBorders(
                        new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 4 }, // 红色边框，宽度4pt
                        new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 4 },
                        new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 4 },
                        new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 4 },
                        new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 4 }, // 内水平边框
                        new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Color = "000000", Size = 4 }  // 内垂直边框
                    )
                ));

                //table.AppendChild(
                //new TableProperties(new TableWidth() { Width = "10000", Type = TableWidthUnitValues.Dxa })); // 设置表格宽度为5000 DXA（1 inch = 1440 DXA）

                // 创建表格的第一行和单元格
                TableRow row = new TableRow();
                TableCell cell1 = new TableCell(new Paragraph(new Run(new Text("施压阶段"))));
                TableCell cell2 = new TableCell(new Paragraph(new Run(new Text("施加电压(kV)"))));
                TableCell cell3 = new TableCell(new Paragraph(new Run(new Text("持续时间(S)"))));
                TableCell cell4 = new TableCell(new Paragraph(new Run(new Text("局放量(pC)"))));
                TableCell cell5 = new TableCell(new Paragraph(new Run(new Text("结果判断"))));

                row.Append(cell1);
                row.Append(cell2);
                row.Append(cell3);
                row.Append(cell4);
                row.Append(cell5);

                table.Append(row);

                // 创建表格的第二行和单元格
                row = new TableRow();
                cell1 = new TableCell(new Paragraph(new Run(new Text("第一阶段"))));
                cell2 = new TableCell(new Paragraph(new Run(new Text(LevelV1))));
                cell3 = new TableCell(new Paragraph(new Run(new Text(LevelTime1))));
                cell4 = new TableCell(new Paragraph(new Run(new Text(LevelPc1))));
                cell5 = new TableCell(new Paragraph(new Run(new Text(LevelIsGood1))));
                row.Append(cell1);
                row.Append(cell2);
                row.Append(cell3);
                row.Append(cell4);
                row.Append(cell5);
                table.Append(row);



                // 创建表格的第3行和单元格
                row = new TableRow();
                cell1 = new TableCell(new Paragraph(new Run(new Text("第二阶段"))));
                cell2 = new TableCell(new Paragraph(new Run(new Text(LevelV2))));
                cell3 = new TableCell(new Paragraph(new Run(new Text(LevelTime2))));
                cell4 = new TableCell(new Paragraph(new Run(new Text(LevelPc2))));
                cell5 = new TableCell(new Paragraph(new Run(new Text(LevelIsGood2))));
                row.Append(cell1);
                row.Append(cell2);
                row.Append(cell3);
                row.Append(cell4);
                row.Append(cell5);
                table.Append(row);


                // 创建表格的第3行和单元格
                row = new TableRow();
                cell1 = new TableCell(new Paragraph(new Run(new Text("第三阶段"))));
                cell2 = new TableCell(new Paragraph(new Run(new Text(LevelV3))));
                cell3 = new TableCell(new Paragraph(new Run(new Text(LevelTime3))));
                cell4 = new TableCell(new Paragraph(new Run(new Text(LevelPc3))));
                cell5 = new TableCell(new Paragraph(new Run(new Text(LevelIsGood3))));
                row.Append(cell1);
                row.Append(cell2);
                row.Append(cell3);
                row.Append(cell4);
                row.Append(cell5);
                table.Append(row);

                // 将表格添加到文档中
                body.Append(table);





                doc.Append(body);
                mainPart.Document = doc;
            }
            */     
    
}
