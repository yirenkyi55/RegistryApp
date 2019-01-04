using RegistryLibrary.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistryAppUI.Printer
{
    public class FilesDocument : PrintDocument
    {
        private int PageNumber { get; set; } //Indicates the current page
        private int Offset { get; set; } //used to access the list of files
        private List<IncomingFileModel> _files { get; set; } //The files to be printed.
        private float lineHeight;//Used to move down one step.

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

            string mainTitle = "TECHIMAN MUNICIPAL ASSEMBLY";
            string addressTitle1 = "P.O.Box TM 1234. Techiman";
            string addressTitle2 = "REGISTRY DEPARTMENT";
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

            PrintPageHeader(e);

            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(0, 122, 204));
            using (Font pageFont = new Font("Arial", 8))
            {
                //Print all headers/titles
                //float lineHeight = pageFont.GetHeight(e.Graphics);
                float y = lineHeight;
                float fontHeight = pageFont.GetHeight(e.Graphics);

                //set the title for the pages
                string No = "  ";
                string regNo = "REG";
                string receivedDate = "REC DATE";
                string personSent = "SENT BY";
                string fileDate = "FILE DATE";
                string refNo = "REF NUMBER";
                string subject = "SUBJECT";
                string from = "FROM";
                string to = "DEPARTMENT";
                string remarks = "REMARKS";

                //Set all xpositions
                float xNo = e.MarginBounds.Left;
                float xRegNo = xNo + e.Graphics.MeasureString(No, pageFont).Width + 3;
                float xReceivedDate = xRegNo + e.Graphics.MeasureString(regNo, pageFont).Width + 8;
                float xPersonSent = xReceivedDate + e.Graphics.MeasureString(receivedDate, pageFont).Width + 8;
                float xFileDate = xPersonSent + e.Graphics.MeasureString(personSent, pageFont).Width + 22;
                float xRefNo = xFileDate + e.Graphics.MeasureString(fileDate, pageFont).Width + 8;
                float xSubject = xRefNo + e.Graphics.MeasureString(refNo, pageFont).Width + 8;
                float xFrom = xSubject + e.Graphics.MeasureString(subject, pageFont).Width + 22;
                float xTo = xFrom + e.Graphics.MeasureString(from, pageFont).Width + 22;
                float xRemarks = xTo + e.Graphics.MeasureString(to, pageFont).Width + 8;

                //print all headers
                //e.Graphics.DrawString(No, pageFont, blueBrush, xNo, y);
                e.Graphics.DrawString(regNo, pageFont, blueBrush, xRegNo, y);
                e.Graphics.DrawString(receivedDate, pageFont, blueBrush, xReceivedDate, y);
                e.Graphics.DrawString(personSent, pageFont, blueBrush, xPersonSent, y);
                e.Graphics.DrawString(fileDate, pageFont, blueBrush, xFileDate, y);
                e.Graphics.DrawString(refNo, pageFont, blueBrush, xRefNo, y);
                e.Graphics.DrawString(subject, pageFont, blueBrush, xSubject, y);
                e.Graphics.DrawString(from, pageFont, blueBrush, xFrom, y);
                e.Graphics.DrawString(to, pageFont, blueBrush, xTo, y);
                e.Graphics.DrawString(remarks, pageFont, blueBrush, xRemarks, y);

                //Move to the next line
                y += fontHeight;

                //Draw a line
                //e.Graphics.DrawLine(new Pen(blackBrush, 1), e.MarginBounds.Left, y, e.MarginBounds.Right, y);
                y += fontHeight + 8;
                //Print all the body

                //Increase the page number by one
                PageNumber += 1; //First page

                while (((y+lineHeight)<e.MarginBounds.Bottom) && Offset<= _files.Count-1)
                {
                    //Define rectangles for appropriate fields
                    List<int> allRectHeights = new List<int>();//For all rectangle heights

                    //Rectangle for the person sent
                    RectangleF personSentRect = new RectangleF();
                    float personSentRectWidth = xFileDate - xPersonSent - 2;
                    personSentRect.Location = new Point((int)xPersonSent, (int)y);                    
                    personSentRect.Size = new Size((int)personSentRectWidth, (int)e.Graphics.MeasureString(_files[Offset].PersonSent, pageFont, (int)personSentRectWidth, StringFormat.GenericTypographic).Width);
                    int personSentHeight = (int)e.Graphics.MeasureString(_files[Offset].PersonSent, pageFont, (int)personSentRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(personSentHeight);

                    //Rectangle for Reference Number
                    RectangleF refRect = new RectangleF();
                    float refRectangleWidth = xSubject - xRefNo ;
                    refRect.Location = new PointF((int)xRefNo, (int)y);
                    refRect.Size = new Size((int)refRectangleWidth,(int)e.Graphics.MeasureString(_files[Offset].ReferenceNumber,pageFont,(int)refRectangleWidth,StringFormat.GenericTypographic).Width);
                    int refHeight = (int)e.Graphics.MeasureString(_files[Offset].ReferenceNumber, pageFont, (int)refRectangleWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(refHeight);

                    //Rectangle for Subject
                    RectangleF subjectRect = new RectangleF();
                    float subjectRectWidth = xFrom - xSubject ;
                    subjectRect.Location = new Point((int)xSubject, (int)y);
                    subjectRect.Size = new Size((int)subjectRectWidth, (int)e.Graphics.MeasureString(_files[Offset].Subject, pageFont, (int)subjectRectWidth, StringFormat.GenericTypographic).Width);
                    int subjectHeight = (int)e.Graphics.MeasureString(_files[Offset].Subject, pageFont, (int)subjectRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(subjectHeight);

                    //Rectangle for Department from                    
                    RectangleF fromRect = new RectangleF();
                    float fromRectWidth = xTo - xFrom ;
                    fromRect.Location = new Point((int)xFrom, (int)y);
                    fromRect.Size = new Size((int)fromRectWidth, (int)e.Graphics.MeasureString(_files[Offset].DepartmentSent, pageFont, (int)fromRectWidth, StringFormat.GenericTypographic).Width);
                    int fromHeight = (int)e.Graphics.MeasureString(_files[Offset].DepartmentSent, pageFont, (int)fromRectWidth, StringFormat.GenericTypographic).Height;
                    allRectHeights.Add(fromHeight);

                    //Rectangle for Department To
                    RectangleF toRect = new RectangleF();
                    float toRectWidth = xRemarks - xTo ;
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
                   // e.Graphics.DrawString((Offset + 1+".").ToString(), pageFont, blackBrush, xNo, y);
                    e.Graphics.DrawString(_files[Offset].RegistryNumber.ToString(),pageFont,blackBrush,xRegNo,y);
                    e.Graphics.DrawString(_files[Offset].DateReceived.ToString("dd/MM/yyyy"), pageFont, blackBrush, xReceivedDate, y);
                    e.Graphics.DrawString(_files[Offset].PersonSent, pageFont, blackBrush, personSentRect);
                    e.Graphics.DrawString(_files[Offset].DateOfLetter.ToString("dd/MM/yyyy"), pageFont, blackBrush, xFileDate, y);
                    e.Graphics.DrawString(_files[Offset].ReferenceNumber, pageFont, blackBrush, refRect);
                    e.Graphics.DrawString(_files[Offset].Subject, pageFont, blackBrush, subjectRect);
                    e.Graphics.DrawString(_files[Offset].DepartmentSent, pageFont, blackBrush, fromRect);
                    e.Graphics.DrawString(_files[Offset].Department.DepartmentName, pageFont, blackBrush, toRect);
                    e.Graphics.DrawString(_files[Offset]?.Remarks ?? "", pageFont, blackBrush, remarksRect);

                    //Increase the offset by one
                    Offset += 1;

                    //Find the maximum height
                    int maxHeight = allRectHeights.Max();

                    y += maxHeight + 8;
                }

                if (Offset<_files.Count-1)
                {
                    //There are still pages to be print
                    e.HasMorePages = true;
                }
                else
                {
                    Offset = 0;
                }

                lineHeight += y;
            }
        }
    }
}
