using RegistryLibrary.Abstracts;
using RegistryLibrary.Data;
using RegistryLibrary.Infrastructure;
using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegistryAppUI.Printer
{
    public class FilesDocument : PrintDocument
    {
        RegistryInfoData registryData = new RegistryInfoData();
        private int PageNumber { get; set; } //Indicates the current page
        private int Offset { get; set; } //used to access the list of files
        private List<IncomingFileModel> _files { get; set; } //The files to be printed.
        private float lineHeight;//Used to move down one step.
        float footerBegins;
        bool printPageHeader = true;

        public FilesDocument(List<IncomingFileModel> files)
        {
            _files = files;

        }


        private void PrintPageHeader(PrintPageEventArgs e)
        {
            float leftMargin = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;

            float pageWidth = e.MarginBounds.Width;
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 122, 204));
            var info = registryData.GetRegistryInfoPrint();
            string mainTitle = info?.MunicipalName.ToUpper() ?? "Municipal Name";
            string addressTitle1 = info?.Address.ToUpper() ?? "Municipal Address";
            string addressTitle2 = info?.RegistryName.ToUpper() ?? "Registry Name";
            string addressTitle3 = "ALL INCOMING FILES AS AT " + DateTime.Today.ToString("dd/MM/yyyy");

            //Determine the font to use
            using (Font mainTitleFont = new Font("Century Gothic", 14, FontStyle.Bold))
            {
                using (Font subTitleFont = new Font("Century Gothic", 12, FontStyle.Bold))
                {
                    //Measure all title width
                    float mainTitleWidth = e.Graphics.MeasureString(mainTitle, mainTitleFont).Width;
                    float addressTitle1Width = e.Graphics.MeasureString(addressTitle1, subTitleFont).Width;
                    float addressTitle2Width = e.Graphics.MeasureString(addressTitle2, subTitleFont).Width;
                    float addressTitle3Width = e.Graphics.MeasureString(addressTitle3, subTitleFont).Width;

                    //Calculates all xpositions(to center text) of the text on the paper
                    float xMainTitle = leftMargin + (pageWidth - mainTitleWidth) / 2;
                    float xAddressTitle1 = leftMargin + (pageWidth - addressTitle1Width) / 2;
                    float xAddressTitle2 = leftMargin + (pageWidth - addressTitle2Width) / 2;
                    float xAddressTitle3 = leftMargin + (pageWidth - addressTitle3Width) / 2;

                    List<float> titleWidths = new List<float>()
                    {
                        mainTitleWidth,addressTitle1Width,addressTitle2Width,addressTitle3Width
                    };

                    List<float> xtitleWidths = new List<float>()
                    {
                        xMainTitle,xAddressTitle1,xAddressTitle2,xAddressTitle3
                    };

                    float longestTitleWidth = titleWidths.Max();
                    float longestxTitle = xtitleWidths.Max();
                    //Draw the  various logo

                    string coatPath = Application.StartupPath + @"\Image\" + "coat.png";
                    Bitmap bmpCoat = new Bitmap(coatPath); /*Properties.Resources.coatPic;*/
                    float imageWidth = bmpCoat.Width;
                    float xImage =( leftMargin +  (pageWidth - longestTitleWidth) / 2)-(imageWidth+15);
                    e.Graphics.DrawImage(bmpCoat, xImage, y);

                    Image logoImage = null;

                    if (info.PicData!=null)
                    {
                         logoImage = info.PicData.BinaryToImage();
                    }

                    if (logoImage==null)
                    {
                        string logoPath = Application.StartupPath + @"\Image\" + "reg.png";
                        var bmpLogo = new Bitmap(logoPath); /*Properties.Resources.regPic;*/
                        float logoWidth = bmpLogo.Width;
                        float xLongEnd = longestTitleWidth + (pageWidth - longestTitleWidth) / 2;
                        float xLogo = xLongEnd + (logoWidth + 15);
                        e.Graphics.DrawImage(bmpLogo, xLogo, y);
                    }
                    else
                    {
                        float logoWidth = logoImage.Width;
                        float xLongEnd = longestTitleWidth + (pageWidth - longestTitleWidth) / 2;
                        float xLogo = xLongEnd + (logoWidth + 15);
                        e.Graphics.DrawImage(logoImage, xLogo, y);
                    }

                    //Draw all headers on the page and increse the line height
                    e.Graphics.DrawString(mainTitle, mainTitleFont, blueBrush, xMainTitle, y);
                    lineHeight = mainTitleFont.GetHeight(e.Graphics);
                    //Move down equivalent spacing of one line
                    y += lineHeight;


                    e.Graphics.DrawString(addressTitle1, subTitleFont, blueBrush, xAddressTitle1, y);
                    y += lineHeight;

                    e.Graphics.DrawString(addressTitle2, subTitleFont, blueBrush, xAddressTitle2, y);
                    y += lineHeight;

                    e.Graphics.DrawString(addressTitle3, subTitleFont, blueBrush, xAddressTitle3, y);
                    y += lineHeight;

                    //Print a line
                    e.Graphics.DrawLine(new Pen(blueBrush, 1), leftMargin, y, e.MarginBounds.Right, y);
                    y += lineHeight + 8;

                    lineHeight = y;//Sets the line height to the last spacing
                }
            }

        }


        public void PrintFilesPage(PrintPageEventArgs e)
        {

            if (printPageHeader)
            {
                PrintPageHeader(e); 
            }

            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 122, 204));
            using (Font pageFont = new Font("Arial", 8))
            {
                //Print all headers/titles
                //float lineHeight = pageFont.GetHeight(e.Graphics);
                float y = lineHeight;
                float fontHeight = pageFont.GetHeight(e.Graphics);

                //set the title for the pages
                string No = "    ";
                string regNo = "REG NUMBER";
                string receivedDate = "REC DATE";
                string personSent = "SENT BY";
               // string fileDate = "FILE DATE";
                string refNo = "REF NUMBER";
                string subject = "SUBJECT";
                string from = "FROM";
                string to = "DEPARTMENT";
                string remarks = "REMARKS";
                bool stopReading = false;

                //Set all xpositions
                float xNo = e.MarginBounds.Left;
                float xRegNo = xNo + e.Graphics.MeasureString(No, pageFont).Width + 22;
                float xReceivedDate = xRegNo + e.Graphics.MeasureString(regNo, pageFont).Width + 9;
                float xPersonSent = xReceivedDate + e.Graphics.MeasureString(receivedDate, pageFont).Width + 9;
                //float xFileDate = xPersonSent + e.Graphics.MeasureString(personSent, pageFont).Width + 22;
                float xRefNo = xPersonSent + e.Graphics.MeasureString(personSent, pageFont).Width + 9;
                float xSubject = xRefNo + e.Graphics.MeasureString(refNo, pageFont).Width + 9;
                float xFrom = xSubject + e.Graphics.MeasureString(subject, pageFont).Width + 22;
                float xTo = xFrom + e.Graphics.MeasureString(from, pageFont).Width + 22;
                float xRemarks = xTo + e.Graphics.MeasureString(to, pageFont).Width + 9;
                footerBegins = xTo;

                //print all headers
                //e.Graphics.DrawString(No, pageFont, blueBrush, xNo, y);
                e.Graphics.DrawString(regNo, pageFont, blueBrush, xRegNo, y);
                e.Graphics.DrawString(receivedDate, pageFont, blueBrush, xReceivedDate, y);
                e.Graphics.DrawString(personSent, pageFont, blueBrush, xPersonSent, y);
                //e.Graphics.DrawString(fileDate, pageFont, blueBrush, xFileDate, y);
                e.Graphics.DrawString(refNo, pageFont, blueBrush, xRefNo, y);
                e.Graphics.DrawString(subject, pageFont, blueBrush, xSubject, y);
                e.Graphics.DrawString(from, pageFont, blueBrush, xFrom, y);
                e.Graphics.DrawString(to, pageFont, blueBrush, xTo, y);
                e.Graphics.DrawString(remarks, pageFont, blueBrush, xRemarks, y);

                ////Move to the next line
                //y += fontHeight;

                ////Print a line
                //e.Graphics.DrawLine(new Pen(blueBrush, 1), e.MarginBounds.Left, y, e.MarginBounds.Right, y);


                //Draw a line
                //e.Graphics.DrawLine(new Pen(blackBrush, 1), e.MarginBounds.Left, y, e.MarginBounds.Right, y);
                y += fontHeight + 8;
                //Print all the body

                //Increase the page number by one
                PageNumber += 1; //First page

                while (((y + lineHeight) < e.MarginBounds.Bottom) && Offset <= _files.Count - 1)
                {
                    //Define rectangles for appropriate fields
                    List<int> allRectHeights = new List<int>();//For all rectangle heights

                    //Rectangle for the  Number
                    RectangleF noRect = new RectangleF();
                    float noRectWidth = xRegNo-xNo;
                    noRect.Location = new Point((int)xNo, (int)y);
                    noRect.Size = new Size((int)noRectWidth, (int)e.Graphics.MeasureString((Offset + 1 + ". ").ToString(), pageFont, (int)noRectWidth, StringFormat.GenericTypographic).Width);
                    int noHeight = (int)e.Graphics.MeasureString((Offset + 1+ ". ").ToString(), pageFont, (int)noRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(noHeight);

                    //Rectangle for the Registry Number
                    RectangleF regNumberRect = new RectangleF();
                    float regNumberRectWidth = xReceivedDate - xRegNo;
                    regNumberRect.Location = new Point((int)xRegNo, (int)y);
                    regNumberRect.Size = new Size((int)regNumberRectWidth, (int)e.Graphics.MeasureString("TMA-REG-" + _files[Offset].RegistryNumber.ToString("D3"), pageFont, (int)regNumberRectWidth, StringFormat.GenericTypographic).Width);
                    int regNumberHeight = (int)e.Graphics.MeasureString("TMA-REG-" + _files[Offset].RegistryNumber.ToString("D3"), pageFont, (int)regNumberRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(regNumberHeight);

                    //Rectangle for the person sent
                    RectangleF personSentRect = new RectangleF();
                    float personSentRectWidth = xRefNo - xPersonSent - 2;
                    personSentRect.Location = new Point((int)xPersonSent, (int)y);
                    personSentRect.Size = new Size((int)personSentRectWidth, (int)e.Graphics.MeasureString(_files[Offset].PersonSent, pageFont, (int)personSentRectWidth, StringFormat.GenericTypographic).Width);
                    int personSentHeight = (int)e.Graphics.MeasureString(_files[Offset].PersonSent, pageFont, (int)personSentRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(personSentHeight);

                    //Rectangle for Reference Number
                    RectangleF refRect = new RectangleF();
                    float refRectangleWidth = xSubject - xRefNo;
                    refRect.Location = new PointF((int)xRefNo, (int)y);
                    refRect.Size = new Size((int)refRectangleWidth, (int)e.Graphics.MeasureString(_files[Offset].ReferenceNumber, pageFont, (int)refRectangleWidth, StringFormat.GenericTypographic).Width);
                    int refHeight = (int)e.Graphics.MeasureString(_files[Offset].ReferenceNumber, pageFont, (int)refRectangleWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(refHeight);

                    //Rectangle for Subject
                    RectangleF subjectRect = new RectangleF();
                    float subjectRectWidth = xFrom - xSubject;
                    subjectRect.Location = new Point((int)xSubject, (int)y);
                    subjectRect.Size = new Size((int)subjectRectWidth, (int)e.Graphics.MeasureString(_files[Offset].Subject, pageFont, (int)subjectRectWidth, StringFormat.GenericTypographic).Width);
                    int subjectHeight = (int)e.Graphics.MeasureString(_files[Offset].Subject, pageFont, (int)subjectRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(subjectHeight);

                    //Rectangle for Department from                    
                    RectangleF fromRect = new RectangleF();
                    float fromRectWidth = xTo - xFrom;
                    fromRect.Location = new Point((int)xFrom, (int)y);
                    fromRect.Size = new Size((int)fromRectWidth, (int)e.Graphics.MeasureString(_files[Offset].DepartmentSent, pageFont, (int)fromRectWidth, StringFormat.GenericTypographic).Width);
                    int fromHeight = (int)e.Graphics.MeasureString(_files[Offset].DepartmentSent, pageFont, (int)fromRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(fromHeight);

                    //Rectangle for Department To
                    RectangleF toRect = new RectangleF();
                    float toRectWidth = xRemarks - xTo;
                    toRect.Location = new Point((int)xTo, (int)y);
                    toRect.Size = new Size((int)toRectWidth, (int)e.Graphics.MeasureString(_files[Offset].Department.DepartmentName, pageFont, (int)toRectWidth, StringFormat.GenericTypographic).Width);
                    int toHeight = (int)e.Graphics.MeasureString(_files[Offset].Department.DepartmentName, pageFont, (int)toRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(toHeight);

                    //Rectangle for remarks
                    RectangleF remarksRect = new RectangleF();
                    float remarksRectWidth = e.MarginBounds.Right - xRemarks;
                    remarksRect.Location = new Point((int)xRemarks, (int)y);
                    remarksRect.Size = new Size((int)remarksRectWidth, (int)e.Graphics.MeasureString(_files[Offset].Remarks, pageFont, (int)remarksRectWidth, StringFormat.GenericTypographic).Width);
                    int remarksHeight = (int)e.Graphics.MeasureString(_files[Offset].Remarks, pageFont, (int)remarksRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(remarksHeight);

                    //draw records on the paper
                     e.Graphics.DrawString((Offset + 1 + ". ").ToString(), pageFont, blackBrush, noRect);
                    e.Graphics.DrawString("TMA-REG-"+_files[Offset].RegistryNumber.ToString("D3"), pageFont, blackBrush, regNumberRect);
                    e.Graphics.DrawString(_files[Offset].DateReceived.ToString("dd/MM/yyyy"), pageFont, blackBrush, xReceivedDate, y);
                    e.Graphics.DrawString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_files[Offset].PersonSent), pageFont, blackBrush, personSentRect);
                   //e.Graphics.DrawString(_files[Offset].DateOfLetter.ToString("dd/MM/yyyy"), pageFont, blackBrush, xFileDate, y);
                    e.Graphics.DrawString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_files[Offset].ReferenceNumber), pageFont, blackBrush, refRect);
                    e.Graphics.DrawString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_files[Offset].Subject), pageFont, blackBrush, subjectRect);
                    e.Graphics.DrawString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_files[Offset].DepartmentSent), pageFont, blackBrush, fromRect);
                    e.Graphics.DrawString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_files[Offset].Department.DepartmentName), pageFont, blackBrush, toRect);
                    e.Graphics.DrawString(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_files[Offset]?.Remarks ?? ""), pageFont, blackBrush, remarksRect);

                    //Increase the offset by one
                    Offset += 1;

                   
                    //Find the maximum height
                    int maxHeight = allRectHeights.Max();

                    y += maxHeight +2;
                    //Print a line
                    e.Graphics.DrawLine(new Pen(blueBrush, 1), e.MarginBounds.Left, y, e.MarginBounds.Right, y);

                    y += fontHeight ;

                   

                }

                if (Offset < _files.Count - 1)
                {
                    //There are still pages to be print
                    printPageHeader = false;
                    e.HasMorePages = true;
                }
                else
                {
                    Offset = 0;
                    stopReading = true;
                }

                lineHeight = y;
                if (stopReading)
                {
                    lineHeight -= fontHeight;
                    //We can print the footer of the page...
                    PrintFooter(e);
                }
            }
        }

        private void PrintFooter(PrintPageEventArgs e)
        {
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 122, 204));
            float y = lineHeight;
            //y += 4;
           // e.Graphics.DrawLine(new Pen(blueBrush, 1), e.MarginBounds.Left, y, e.MarginBounds.Right, y);
   
            //e.Graphics.DrawLine(new Pen(blueBrush, 1), e.MarginBounds.Left, y, e.MarginBounds.Right, y);
            using (Font footerFont = new Font("Arial",8))
            {
                float fontHeight = footerFont.GetHeight(e.Graphics);
                y += fontHeight;
              float  xFooter = footerBegins;
                e.Graphics.DrawString($"Total Files Printed: {_files.Count()}", footerFont, blueBrush, xFooter, y);
            }
        }
    }
}
