using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using iTextSharp.text.pdf;
using iTextSharp.text.xml;
using iTextSharp.text.pdf.parser;
using Ionic.Zip;
using System.IO.Compression;
using System.Text.RegularExpressions;
using IniParser;
using IniParser.Model;
using iTextSharp.text;


namespace PDF_Analiser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (!Directory.Exists(@"bin\lib"))
            {
                MessageBox.Show(this, "\"bin\\lib\" is not found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!Directory.Exists(@"bin"))
            {
                MessageBox.Show(this, "\"bin\" is not found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog excelFile = new OpenFileDialog();

            excelFile.DefaultExt = ".xlsx";
            excelFile.Filter = "Microsoft Excel 2007 (*.xlsx)|*.xlsx|Excel File (*.xls)|*.xls";
            excelFile.FilterIndex = 1;
            excelFile.CheckFileExists = true;
            excelFile.ShowDialog();

            txt_FilePath.Text = excelFile.FileName;
        }

        private void btn_Generate_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"bin\lib"))
            {
                MessageBox.Show(this, "\"bin\\lib\" is not found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!Directory.Exists(@"bin"))
            {
                MessageBox.Show(this, "\"bin\" is not found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DateTime actualProcessSrat = DateTime.Now;
            richConsole.Clear();
            int error_Count = 0;
            int Chargable = 0;
            int nonChargable = 0;

            this.progressBar1.Visible = true;

            string fileName = txt_FilePath.Text;
            string log = @"log/"+ Application.ProductName + ".log";
            StreamWriter file = new StreamWriter(log, false);
            

            // Load INI file from path, Stream or TextReader.
            var parser = new FileIniDataParser();
            

            // is also the default comment string, you need to change it
            parser.Parser.Configuration.CommentString = "//";

            IniData data = parser.ReadFile(@"bin/PDFAnalyzer_Map.ini");


            // INI Sections Assign, Separater is mid dash "|"
            var iniBlankText = data["Non-Chargable"]["BlankPage"].Split('|');
            var iniWmText = data["Non-Chargable"]["Watermark"].Split('|');
            

            //Log Create
            if (!File.Exists(log))
            {
                File.Create(log);
                file.WriteLine("-------------------------------------------");
                file.WriteLine("     	  " + Application.ProductName + " (" + Application.ProductVersion + ")");
                file.WriteLine("-------------------------------------------");

                richConsole.AppendText("-------------------------------------------\n");
                richConsole.AppendText("    " + Application.ProductName + " (" + Application.ProductVersion + ")\n");
                richConsole.AppendText("-------------------------------------------\n");
                richConsole.ScrollToCaret();

                //file.Close();
            }
            else
            {
                file.Flush();
                file.WriteLine("-------------------------------------------");
                file.WriteLine("     	  " + Application.ProductName + " (" + Application.ProductVersion + ")");
                file.WriteLine("-------------------------------------------");

                richConsole.AppendText ("-------------------------------------------\n");
                richConsole.AppendText ("   " + Application.ProductName + " (" + Application.ProductVersion + ")\n");
                richConsole.AppendText ("-------------------------------------------\n");
                richConsole.ScrollToCaret();
                //file.Close();

            }


            /* Validate Excel */

            if (String.IsNullOrEmpty(fileName))
            {
                error_Count++;
                MessageBox.Show(this, "Invalid Path.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                file.WriteLine("[ERROR]: Invalid path.");
                file.WriteLine("------------------ END --------------------");
                file.Close();

                richConsole.AppendText("[ERROR]: Invalid path.\n");
                richConsole.AppendText("----------------- END -----------------");
                richConsole.ScrollToCaret();

                return;
            }

            if (!File.Exists(fileName))
            {
                error_Count++;
                MessageBox.Show(this, "Invalid Path.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                file.WriteLine("[ERROR]: Path \"" + fileName + "\" is not found.");
                file.WriteLine("------------------ END --------------------");
                file.Close();

                
                richConsole.AppendText("[ERROR]: Path \"" + fileName + "\" is not found.\n");
                richConsole.AppendText("----------------- END -----------------");
                richConsole.ScrollToCaret();

                return;
            }

            /* Start Process */
            var workbook = new XLWorkbook(fileName);
            var ws1 = workbook.Worksheet(1);

            //Remove header from template
            int skipCells = 1; // Skip top header names
            int totalRows = ws1.Range("B2:B1048576").CellsUsed().Count() + skipCells; //Modify cell range what you want to start loop

            //MessageBox.Show(this, totalRows.ToString(), "ROW_Count", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.progressBar1.Maximum = totalRows;

            progressLabel.Visible = true;
            richConsole.AppendText("[STATUS]: Please wait, until extracting PDF files...\n");
            richConsole.ScrollToCaret();
            DateTime startTime = DateTime.Now;
            progressLabel.Text = "Extracting PDF files...";

            /* docpub application run*/
            Process ExternalProcess = new Process();
            ExternalProcess.StartInfo.FileName = @"bin\docpubxsp_XPS.bat";
            ExternalProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ExternalProcess.Start();
            ExternalProcess.WaitForExit();

            if (ExternalProcess.HasExited)
            {

                // ... This is case-insensitive.
                string[] zipFiles = Directory.GetFiles(@"temp/", "*.zip");

       
                foreach (string zipfilename in zipFiles)
                {
                    string fl = zipfilename.Replace(@"temp/", "");
                    string filen = fl.Replace(".zip", "");
                    string actualFilePath = @"temp/" + filen;

                    richConsole.AppendText("[STATUS]: Extracting file: " + filen + "\n");
                    richConsole.ScrollToCaret();

                    try
                    {
                        using (var zip = Ionic.Zip.ZipFile.Read(zipfilename))
                        {
                            zip.ExtractAll(actualFilePath,
                            ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.HResult == -2146233088)
                        {
                            System.IO.Compression.ZipFile.ExtractToDirectory(zipfilename, actualFilePath);
                        }
                        
                    }

                    File.Delete(zipfilename);

                }

            }

            progressLabel.Visible = false;
            DateTime endTime = DateTime.Now;
            richConsole.AppendText("[STATUS]: PDF files extracted... (Process Time: " + (endTime - startTime) + ")\n");

            for (int i = (1 + skipCells); i <= totalRows; i++)
            {
                //Progessbar
                progressBar1.Value += 1;

                //Initialize row
                var row = ws1.Row(i);
                bool empty = row.IsEmpty();

                var cell = row.Cell(2);
                var cellSkip = row.Cell(1);
                string value = cell.GetValue<string>();
                //string value = valuepdf.Replace("-primary.pdf", "");
                string valueSkip = cellSkip.GetValue<string>();

                if (valueSkip == "The following pages are non-chargeable")
                {
                    break;
                }

                

                /*5-9*/
                if (!empty)
                {

                    richConsole.AppendText(value+"\n");
                    richConsole.ScrollToCaret();

                    string ppath = value;

                    var inputDirectory = Directory.EnumerateFiles(@"input\");
                    var pdfFile = inputDirectory.FirstOrDefault(PDFfile => PDFfile.Contains(ppath));
                    var pdfFileExists = pdfFile != null;
                   
                    if (pdfFileExists)
                    {
                        string getPath0 = pdfFile.Replace(@"input\", "");
                        string getPath1 = getPath0.Replace(".pdf", "");
                        string tempZipFolder = @"temp/" + getPath1;
                        bool additionalRemarks = false;
                        bool IsOCR = false;

                        //int TOCPage = 0;
                        List<int> FrontPageNumbers = new List<int>();                        
                        int frontMaxPage = 0;
                        int backMaxPage = 0;
                        int chkWMPage = 0;
                        int chkOCR = 0;

                        List<string> WMPages = new List<string>();
                        List<string> BlankPages = new List<string>();

                        PdfReader pdfReader = new PdfReader(pdfFile);
                        int numberOfPages = pdfReader.NumberOfPages;
                        Chargable = numberOfPages;
                        var Info = pdfReader.PdfVersion;

                        ws1.Cell(i, 6).DataType = XLDataType.Number;
                        ws1.Cell(i, 6).Value = (numberOfPages);

                        var xmlDoc = new XmlDocument();
                        //int isTOCFound = 0;
                        //const int blankPdfsize = 20;
                        var raf = new RandomAccessFileOrArray(pdfFile);


                        //if (File.Exists(tempZipFolder + @"/Documents/1/Structure/DocStruct.struct"))
                        //{

                        //    // content is your XML as string
                        //    xmlDoc.Load(tempZipFolder + @"/Documents/1/Structure/DocStruct.struct");

                        //    XmlNodeList elemList = xmlDoc.GetElementsByTagName("OutlineEntry");
                        //    for (int s = 0; s < elemList.Count; s++)
                        //    {

                        //        string DescriptionVal = elemList[s].Attributes["Description"].Value;

                        //        /*Table of Contents Checking*/                                
                        //        bool TOC = DescriptionVal.Contains("CONTENTS") || DescriptionVal.Contains("Table of Contents") || DescriptionVal.Contains("Contents");

                        //        /*Preface Checking*/
                        //        bool Preface = DescriptionVal.Contains("PREFACE") || DescriptionVal.Contains("Preface");

                        //        /*Foreward Checking*/
                        //        bool Foreward = DescriptionVal.Contains("FOREWORD") || DescriptionVal.Contains("Foreword");

                        //        /*Introduction Checking*/
                        //        bool Intro = DescriptionVal.Contains("INTRODUCTION") || DescriptionVal.Contains("Introduction");

                        //        if (TOC)
                        //        {
                                    
                        //            TOCPage = int.Parse(elemList[s + 1].Attributes["OutlineTarget"].Value.Replace("/FixedDocumentSequence.fdseq#", ""));
                        //            //FrontPageNumbers.Add(TOCPage - 1);

                        //            isTOCFound = TOCPage;

                        //        }

                        //    }

                        //    //if (FrontPageNumbers.Count > 0)
                        //    //{
                        //    //    frontMaxPage = FrontPageNumbers.Max();
                        //    //}
                        //    //else
                        //    //{
                        //    //    frontMaxPage = 0;
                        //    //}

                        //}

                        List<List<string>> groups = new List<List<string>>();
                        List<string> current = null;

                        /*Read Each Page*/
                        for (int j = 1; j <= numberOfPages ; j++)
                        {

                            foreach (var line in File.ReadAllLines(tempZipFolder + @"\Documents\1\Pages\" + (j) + ".fpage"))
                            {

                               
                                //if ((line.Contains("UnicodeString=\"NOTES ") || line.Contains("UnicodeString=\"Notes ") || line.Contains("UnicodeString=\"NOTES\"")) && current == null)
                                //{
                                //    nonChargable += 1;

                                //}
                                //else if ((line.Contains("UnicodeString=\"Intentionally Blank ") || line.Contains("UnicodeString=\"INTENTIONALLY BLANK ") || line.Contains("UnicodeString=\"INTENTIONALLY BLANK\"")) && current == null)
                                //{
                                //    nonChargable += 1;

                                //}

                                if ((line.Contains("&lt;FrontMatter&gt;")) && current == null)
                                {
                                    frontMaxPage = j - 1;
                                    
                                }

                                if ((line.Contains("&lt;/FrontMatter&gt;")) && current == null)
                                {
                                    backMaxPage = numberOfPages - j;

                                }

                                //Watermark
                                if ((line.Contains("&lt;WATERMARKED&gt;")) && current == null)
                                {
                                    WMPages.Add(j.ToString());
                                    additionalRemarks = true;

                                }
                                //BlankPages
                                if ((line.Contains("&lt;SA_BLANK_PAGE&gt;")) && current == null)
                                {
                                    nonChargable += 1;
                                    BlankPages.Add(j.ToString());

                                }

                                //Match m = Regex.Match(line, "<Path Data=\".*Z \"");
                                //if (m.Success && m.Length > 10440 && (!line.Contains("StrokeMiterLimit")) && current == null || (line.Contains("UnicodeString=\"Internal Use Only")))
                                //{

                                //    WMPages.Add(j.ToString());
                                //    additionalRemarks = true;
                                //}
                                //else
                                //{

                                //    //FindWatermark
                                //    foreach (String key in iniWmText)
                                //    {
                                //        if ((line.Contains("UnicodeString=\"" + key.ToString() + "\"")) && current == null)
                                //        {
                                //            WMPages.Add(j.ToString());
                                //            additionalRemarks = true;                                           
                                //        }
                                //    }

                                //}

                            }

                        }



                        //loop through each page and if the bs is larger than 20 than we know it is not blank.
                        //if it is less than 20 than we don't include that blank page.
                        //for (var page = frontMaxPage + 1; page < numberOfPages - backMaxPage; page++)
                        //{
                        //    if (frontMaxPage > 0)
                        //    {
                        //        string text = string.Empty;

                        //        text += PdfTextExtractor.GetTextFromPage(pdfReader, page);

                        //        // get the page content
                        //        byte[] bContent = pdfReader.GetPageContent(page, raf);
                        //        var bs = new MemoryStream();
                        //        bs.Write(bContent, 0, bContent.Length);

                                

                        //        ////FindBlanks
                        //        //foreach (String key in iniBlankText)
                        //        //{
                                    
                        //        //    if (text == key.ToString() || text == key.ToString() + " ")
                        //        //    {
                        //        //        nonChargable += 1;
                        //        //        BlankPages.Add(page.ToString());
                        //        //    }

                        //        //    if (string.IsNullOrEmpty(text) || text== "<FrontMatter>" || text == "</FrontMatter>" && bs.Length < 150)
                        //        //    {
                        //        //        chkOCR++;
                        //        //    }

                        //        //}

                        //        //if (string.IsNullOrEmpty(text) && bs.Length > blankPdfsize)
                        //        //{
                        //        //    nonChargable += 1;
                        //        //    BlankPages.Add(page.ToString());
                        //        //}

                        //    }
                        //}

                        //OCR identify
                        if (chkOCR>numberOfPages/2)
                        {
                            IsOCR = true;
                        }

                        //Chargeble Page Count                        
                        ws1.Cell(i, 7).DataType = XLDataType.Number;                        
                        ws1.Cell(i, 7).Value = ((numberOfPages) - (frontMaxPage + backMaxPage + nonChargable));                        
                        //nonChargable = 0;

                        //BlankPages                        
                        ws1.Cell(i, 12).DataType = XLDataType.Number;
                        ws1.Cell(i, 12).Value = (nonChargable);
                        ws1.Cell(i, 12).Style.Fill.BackgroundColor = XLColor.LightGray;
                        if (BlankPages.Count != 0)
                        {
                            error_Count++;
                            richConsole.AppendText("[INFO]: " + value + ", blank detected between bodymatter in page(s) " + string.Join(",", BlankPages.Distinct()) + "\n");
                            //richConsole.ScrollToCaret();
                            file.WriteLine("[INFO]: " + value + ", blank detected between bodymatter in page(s) " + string.Join(",", BlankPages.Distinct()));

                            ws1.Cell(i, 13).Value = ("Blank detected between bodymatter in page(s) " + string.Join(",", BlankPages.Distinct()) + ".");
                        }

                        nonChargable = 0;

                        //Additional Remarks     
                        ws1.Cell(i, 13).DataType = XLDataType.Text;
                        
                        if (BlankPages.Count != 0 && additionalRemarks)
                        {
                            ws1.Cell(i, 13).Value = ("1. blank detected between bodymatter in page(s) " + string.Join(",", BlankPages.Distinct())+".\n2. Please verify if this file included with watermark page(s)\n");
                            additionalRemarks = false;
                            IsOCR = false;
                            chkOCR = 0;
                        }
                        else if (chkOCR>0 && additionalRemarks)
                        {
                            ws1.Cell(i, 13).Value = ("1. This file may be Scanned PDF, please manually check this again.\n2. Please verify if this file included with watermark page(s)\n");
                            additionalRemarks = false;
                            IsOCR = false;
                            chkOCR = 0;
                        }
                        else if(IsOCR)
                        {
                            ws1.Cell(i, 13).Value = ("This file may be Scanned PDF, please manually check this again.\n");
                            IsOCR = false;
                            chkOCR = 0;
                        }
                        else if (BlankPages.Count == 0 && additionalRemarks)
                        {
                            ws1.Cell(i, 13).Value = ("Please verify if this file included with watermark page(s)\n");
                            additionalRemarks = false;
                        }

                        ws1.Cell(i, 13).Style.Fill.BackgroundColor = XLColor.LightGray;
                        additionalRemarks = false;


                        //check WaterMark Count
                        if (WMPages.Count!=0)
                        {
                            error_Count++;
                            richConsole.AppendText("[WARRNING]: " + value + ", watermarks found in page(s) " + string.Join(",", WMPages.Distinct()) + "\n");
                            //richConsole.ScrollToCaret();

                            file.WriteLine("[WARRNING]: " + value + ", watermarks found in page(s) " + string.Join(",", WMPages.Distinct()));

                            ws1.Cell(i, 8).DataType = XLDataType.Text;
                            ws1.Cell(i, 8).Value = ("YES");
                            ws1.Cell(i, 8).Style.Fill.BackgroundColor = XLColor.Yellow;
                            chkWMPage = 0;
                        }
                        else
                        {
                            chkWMPage = 0;
                        }



                        //nonChargableBeforeFront
                        ws1.Cell(i, 9).DataType = XLDataType.Number;
                        ws1.Cell(i, 9).Value = (frontMaxPage);
                        frontMaxPage = 0;

                        //nonChargableAfterback
                        ws1.Cell(i, 10).DataType = XLDataType.Number;
                        ws1.Cell(i, 10).Value = (backMaxPage);
                        backMaxPage = 0;

                        /*ReSet*/
                        //isTOCFound = 0;
                        //isPrefaceFound = 0;
                        //isForwardFound = 0;
                        //isIntroFound = 0;

                        ////If All Done
                        Directory.Delete(tempZipFolder,true);
                    }
                    else
                    {
                        ws1.Cell(i, 6).DataType = XLDataType.Text;
                        ws1.Cell(i, 6).Style.Fill.BackgroundColor = XLColor.Yellow;
                        ws1.Cell(i, 6).Value = ("NOT FOUND");
                    }

                }

            }

            /* End Process*/

            //BlankPages                        
            ws1.Cell(1, 12).DataType = XLDataType.Text;
            ws1.Cell(1, 12).Value = ("Blank Pages Count - Between Content");
            ws1.Cell(1, 12).Style.Fill.BackgroundColor = XLColor.LightGray;
            nonChargable = 0;

            //Additional Remarks                        
            ws1.Cell(1, 13).DataType = XLDataType.Text;
            ws1.Cell(1, 13).Value = ("Additional Remarks");
            ws1.Cell(1, 13).Style.Fill.BackgroundColor = XLColor.LightGray;

            DateTime actualProcessEnd = DateTime.Now;
            /*Log file Close*/
            richConsole.AppendText("[STATUS]: Total Process Time - " + (actualProcessEnd-actualProcessSrat).ToString() + "\n");
            richConsole.ScrollToCaret();
            file.WriteLine("------------------ END --------------------");
            file.Close();

            richConsole.AppendText("----------------- END -----------------");
            richConsole.ScrollToCaret();

            workbook.SaveAs(@"output/Catalogue v1" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");

            this.progressBar1.Visible = false;
            progressBar1.Value = 1;
            MessageBox.Show(this, "Precess Completed.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            /* Automatically open log file */
            if (error_Count > 0)
            {
                Process.Start("notepad.exe", log);
            }
        }

        private void tagPDF_Click(object sender, EventArgs e)
        {


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(@"bin\lib"))
            {
                MessageBox.Show(this, "\"bin\\lib\" is not found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!Directory.Exists(@"bin"))
            {
                MessageBox.Show(this, "\"bin\" is not found.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
