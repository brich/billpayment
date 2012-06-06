using System;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Mail;

namespace BillPaymentForm
{
    public partial class BillPaymentForm : System.Web.UI.UserControl
    {

        // Set the Email Variables
        static string sEmailServer = "proxy1.HS.internal";
        static string sEmailServerUser = "admin@ability.hillsouth.com";
        static string sEmailServerPass = "MDCrjk94";

        static string sEmailFrom = "payments@allergysc.com";
        static string sEmailAdmin = "sharonposton@allergysc.com";
        static string sEmailAdmin2 = "jhatfield@allergysc.com";
        static string sEmailWebAdmin = "brad@hillsouth.com";
        static string sEmailSubjectToPayer = "Receipt from Allergy, Asthma and Sinus Center";
        static string sEmailSubjectToAdmin = "Payment from First Data Gateway";

        static string sEmailFont = "Lucida Grande, Lucida, Verdana, sans-serif";
        static string sEmailBkgdColor = "ececec";
        static string sEmailTopBarRadius = "6px 6px 0px 0px";
        static string sEmailTopBarColor = "34377D";
        static string sEmailTopBarFontColor = "ffffff";
        static string sEmailHeaderImage = "http://allergysc.com/media/6163/headerbkgd.jpg";
        static string sEmailBodyBkgdColor = "ffffff";
        static string sEmailBodyTitleSize = "18px";
        static string sEmailBodyTitleColor = "16195B";
        static string sEmailBodyTitlePayer = "Thank you for your payment";
        static string sEmailBodyTitleAdmin = "Payment from First Data Gateway";
        static string sEmailBodyFontSize = "13px";
        static string sEmailBodyFontColor = "34377D";
        static string sEmailBodySubTitleSize = "16px";
        static string sEmailBodySubTitlePayer = "Your Payment:";
        static string sEmailBodySubTitleAdmin = "Payment Information:";
        static string sEmailFooterRadius = "0px 0px 6px 6px";
        static string sEmailFooterFontSize = "12px";
        static string sEmailFooterFontSizeAddress = "10px";
        static string sEmailFooterMessagePayer = "You're receiving this email because you completed a payment via the AllergySC.com website. If you think that you have received this email by mistake or fraud, please <b><a href='http://allergysc.com/contactus.aspx' target='_blank'>contact Allergy, Asthma and Sinus Center immediately</a></b>.";
        static string sEmailFooterMessageAdmin = "You're receiving this email because you are an administrator for Allergy, Asthma and Sinus Center. If you think that you have received this email by mistake or fraud, please <b><a href='http://allergysc.com/contactus.aspx' target='_blank'>contact Allergy, Asthma and Sinus Center immediately</a></b>.";
        static string sEmailAddress = "800 East Cheves Street,<br />Suite 420<br />Florence, SC 29506<br />United States of America";

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void SubmitButton_Click(object sender, System.EventArgs e)
        {
            // Grab current Date and Time
            DateTime oPaymentDate = DateTime.Now;
            string sPaymentDate = oPaymentDate.ToString("M/d/yyyy hh:mm:ss tt");

            // Hide the Payment Panel
            Payment.Visible = false;
            // Show the Processing Panel
            Processing.Visible = true;

            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Ssl3;

            // Initialize Service Object
            FDGGWSApiOrderService.FDGGWSApiOrderService oFDGGWSApiOrderService = new FDGGWSApiOrderService.FDGGWSApiOrderService();

            // Set the WSDL URL
            oFDGGWSApiOrderService.Url = @"https://ws.firstdataglobalgateway.com/fdggwsapi/services/order.wsdl";

            // Get the Application Root Directory
            string appRoot = HttpRuntime.AppDomainAppPath;

            // Configure Client Certificate
            string certPath = "~/usercontrols/BillPaymentForm/WS1001304227._.1.pem";
            oFDGGWSApiOrderService.ClientCertificates.Add(X509Certificate.CreateFromCertFile(certPath.Replace("~/", appRoot)));

            // Set the Authentication Credentials
            NetworkCredential nc = new NetworkCredential("WS1001304227._.1", "jGmv8uJF");
            oFDGGWSApiOrderService.Credentials = nc;

            // Create Sale Transaction Request
            FDGGWSApiOrderService.FDGGWSApiOrderRequest oOrderRequest = new FDGGWSApiOrderService.FDGGWSApiOrderRequest();

            // Create a New Transaction
            FDGGWSApiOrderService.Transaction oTransaction = new FDGGWSApiOrderService.Transaction();

            // Create the Credit Card Type
            FDGGWSApiOrderService.CreditCardTxType oCreditCardTxType = new FDGGWSApiOrderService.CreditCardTxType();
            oCreditCardTxType.Type = FDGGWSApiOrderService.CreditCardTxTypeType.sale;

            // Create the Credit Card Data
            FDGGWSApiOrderService.CreditCardData oCreditCardData = new FDGGWSApiOrderService.CreditCardData();
            oCreditCardData.ItemsElementName = new FDGGWSApiOrderService.ItemsChoiceType[] { FDGGWSApiOrderService.ItemsChoiceType.CardNumber, FDGGWSApiOrderService.ItemsChoiceType.ExpMonth, FDGGWSApiOrderService.ItemsChoiceType.ExpYear, FDGGWSApiOrderService.ItemsChoiceType.CardCodeValue };
            oCreditCardData.Items = new string[] { txtCCNumber.Text, drpExpMonth.SelectedItem.Value, drpExpYear.SelectedItem.Value, txtCardCodeValue.Text };

            // Set the Credit Card Number to a variable for later usage
            string sCreditCardNumber = txtCCNumber.Text;
            string sCreditCardLast4 = "************" + sCreditCardNumber.Substring(12, 4);

            // Add the Credit Card Data to the Transaction
            oTransaction.Items = new object[] { oCreditCardTxType, oCreditCardData };

            // Create the Payment
            FDGGWSApiOrderService.Payment oPayment = new FDGGWSApiOrderService.Payment();
            oPayment.ChargeTotal = Convert.ToDecimal(txtChargeTotal.Text);

            // Set the Payment to a variable for later usage
            string sPayment = txtChargeTotal.Text;

            // Add the Payment to the Transaction
            oTransaction.Payment = oPayment;

            // Create the Transaction Details
            FDGGWSApiOrderService.TransactionDetails oTransactionDetails = new FDGGWSApiOrderService.TransactionDetails();
            string sCustomerLastName = txtCustomerLastName.Text;
            string sCustomerFirstName = txtCustomerFirstName.Text;
            string sCustomerID = txtCustomerID.Text;
            string sCustomerFull = sCustomerLastName + "," + sCustomerFirstName + "-" + sCustomerID;
            oTransactionDetails.UserID = sCustomerFull;

            // Add the Transaction Details to the Transaction
            oTransaction.TransactionDetails = oTransactionDetails;

            // Create the Billing Information
            FDGGWSApiOrderService.Billing oBilling = new FDGGWSApiOrderService.Billing();
            //oBilling.CustomerID = txtCustomerLastName.Text + "," + txtCustomerFirstName.Text + "-" + txtCustomerID.Text;
            oBilling.Name = txtName.Text;
            oBilling.Address1 = txtAddress.Text;
            oBilling.City = txtCity.Text;
            oBilling.State = txtState.Text;
            oBilling.Zip = txtZip.Text;
            oBilling.Country = "United States of America";
            oBilling.Email = txtEmail.Text;

            // Set the Billing Information to variables for later usage
            string sName = txtName.Text;
            string sAddress1 = txtAddress.Text;
            string sCity = txtCity.Text;
            string sState = txtState.Text;
            string sZip = txtZip.Text;
            string sCountry = "United States of America";
            string sEmail = txtEmail.Text;

            // Add the Billing to the Transaction
            oTransaction.Billing = oBilling;

            // Add the Transaction to the Order Request
            oOrderRequest.Item = oTransaction;

            // Get First Data's Response
            FDGGWSApiOrderService.FDGGWSApiOrderResponse oResponse = null;
            try
            {
                // Create the Response sequence
                oResponse = oFDGGWSApiOrderService.FDGGWSApiOrder(oOrderRequest);

                // Grab the Transaction Responses
                string sTransactionResult = oResponse.TransactionResult;
                string sTransactionTime = oResponse.TransactionTime;
                string sProcessorResponseMessage = oResponse.ProcessorResponseMessage;
                string sOrderID = oResponse.OrderId;

                // Grab the Tranaction Error Message
                string sErrorMessage = oResponse.ErrorMessage;
                string sErrorCode = null;
                if (!String.IsNullOrEmpty(sErrorMessage))
                {
                    sErrorCode = sErrorMessage.Substring(4, 6);
                }

                // If the transaction is Approved, show the success panel
                if (sTransactionResult == "APPROVED")
                {
                    Processing.Visible = false;
                    Success.Visible = true;

                    // Set the Reference Number Label
                    lblReferenceNumber.Text = sOrderID;
                    // Set the Payment Label
                    lblPayment.Text = sPayment;
                    // Set the Billing Information Labels
                    lblName.Text = sName;
                    lblAddress1.Text = sAddress1;
                    lblCity.Text = sCity;
                    lblState.Text = sState;
                    lblZip.Text = sZip;
                    lblCountry.Text = sCountry;
                    lblEmail.Text = sEmail;

                    // Create the Confirmation Email to the Payer Content
                    string sEmailContentPayer = "<tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" + 
						"<p style='padding:0; margin:0;'><b>Payment Date:</b></p>" + 
						"</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sPaymentDate + "</p>" + 
						"</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Name:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerFirstName + " " + sCustomerLastName + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Chart Number:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerID + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Reference #:</b></p>" + 
						"</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sOrderID + "</p>" + 
						"</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Total Payment:</b></p>" + 
						"</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>$" + sPayment + "</p>" + 
						"</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Credit Card Number:</b></p>" + 
						"</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCreditCardLast4 + "</p>" + 
						"</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Billed To:</b></p>" + 
						"</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sName + "<br />" + sAddress1 + "<br />" + sCity + ", " + sState + " " + sZip + "<br />" + sCountry + "</p>" + 
						"</td></tr>";

                    // Create the Admin Email
                    string sEmailContentAdmin = "<tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Payment Date:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sPaymentDate + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Name:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerFirstName + " " + sCustomerLastName + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Chart Number:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerID + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Information:<br /><i>As it is seen in gateway</i></b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerFull + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Reference #:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sOrderID + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Total Payment:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>$" + sPayment + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Credit Card Number:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCreditCardLast4 + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Billed To:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sName + "<br />" + sAddress1 + "<br />" + sCity + ", " + sState + " " + sZip + "<br />" + sCountry + "<br />" + sEmail + "</p>" +
                        "</td></tr>";

                    // Set up the SMTP Server
                    SmtpClient oSmtpServer = new SmtpClient(sEmailServer);
                    oSmtpServer.Credentials = new System.Net.NetworkCredential(sEmailServerUser, sEmailServerPass);

                    // Send the Confirmation Email to the Payer
                    MailMessage oMail = new MailMessage();
                    oMail.From = new MailAddress(sEmailFrom);
                    oMail.To.Add(sEmail);
                    oMail.Subject = sEmailSubjectToPayer;
                    oMail.IsBodyHtml = true;
                    oMail.Body = sEmailHeaderBeginning + sEmailStyles + sEmailHeaderEnding + sEmailBodyBeginning + sEmailBodyHeader + sEmailBodyPayer + sEmailContentPayer + sEmailFooterPayer;
                    oSmtpServer.Send(oMail);

                    // Send the Email to the Administrator
                    MailMessage oMail2 = new MailMessage();
                    oMail2.From = new MailAddress(sEmailFrom);
                    oMail2.To.Add(sEmailAdmin);
                    oMail2.To.Add(sEmailAdmin2);
                    oMail2.Subject = sEmailSubjectToAdmin;
                    oMail2.IsBodyHtml = true;
                    oMail2.Body = sEmailHeaderBeginning + sEmailStyles + sEmailHeaderEnding + sEmailBodyBeginning + sEmailBodyHeader + sEmailBodyAdmin + sEmailContentAdmin + sEmailFooterAdmin;
                    oSmtpServer.Send(oMail2);
                }
                else
                {
                    Processing.Visible = false;
                    Fail.Visible = true;

                    // Set up the back button
                    lblBackButton.Text = "<p class='bottom'><a onclick='goBack()' style='background-color: transparent; cursor:pointer;'><img src='/media/6156/backbutton.png' alt='Go Back' width='112' height='31' /></a></p>";

                    // Set the Error Message Label
                    if (sErrorCode == "000002")
                    {
                        lblErrorMessage.Text = "Please contact your financial institution. Your transaction will require a voice authorization.";
                    }
                    else if (sErrorCode == "000100")
                    {
                        lblErrorMessage.Text = "There has been an internal error with the payment gateway. Please try your payment transaction again at a later time.";
                    }
                    else if (sErrorCode == "002000")
                    {
                        lblErrorMessage.Text = "There has been an error with the payment gateway. This error could be one of the following: (1) The transaction was declined due to insufficient funds, (2) you tried to use an unsupported card type or (3) there was simply a general processing error. Please try your payment transaction again at a later time.";
                    }
                    else if (sErrorCode == "002302")
                    {
                        lblErrorMessage.Text = "The expiration date that you entered is invalid. Please return to the Payment form to correct the error and resubmit your transaction.";
                        // Show the back button
                        lblBackButton.Visible = true;
                    }
                    else if (sErrorCode == "002303")
                    {
                        lblErrorMessage.Text = "The credit card number that you provided is invalid. Please return to the Payment form to correct the error and resubmit your transaction.";
                        // Show the back button
                        lblBackButton.Visible = true;
                    }
                    else if (sErrorCode == "002304")
                    {
                        lblErrorMessage.Text = "The credit card that you provided has expired. Please return to the Payment form to enter in a new card and resubmit your transaction.";
                        // Show the back button
                        lblBackButton.Visible = true;
                    }
                    else
                    {
                        lblErrorMessage.Text = "Your transaction has been declined by our payment gateway. Please try your payment transaction again at a later time.";
                    }

                    // Set the Reference Number Label
                    lblReferenceNumber.Text = sOrderID;
                    // Set the Payment Label
                    lblPayment.Text = sPayment;
                    // Set the Billing Information Labels
                    lblName.Text = sName;
                    lblAddress1.Text = sAddress1;
                    lblCity.Text = sCity;
                    lblState.Text = sState;
                    lblZip.Text = sZip;
                    lblCountry.Text = sCountry;
                    lblEmail.Text = sEmail;

                    // Create the Web Admin Email
                    string sEmailContentWebAdmin = "<tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Payment Date:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sPaymentDate + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Error Code:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sErrorCode + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Error Message:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sErrorMessage + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Name:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerFirstName + " " + sCustomerLastName + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Chart Number:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerID + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Patient Information:<br /><i>As it is seen in gateway</i></b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCustomerFull + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Reference #:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sOrderID + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Total Payment:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>$" + sPayment + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Credit Card Number:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sCreditCardLast4 + "</p>" +
                        "</td></tr><tr style='border-collapse:collapse;'><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'><b>Billed To:</b></p>" +
                        "</td><td align='left' valign='top' style='border-collapse:collapse;'>" +
                        "<p style='padding:0; margin:0;'>" + sName + "<br />" + sAddress1 + "<br />" + sCity + ", " + sState + " " + sZip + "<br />" + sCountry + "<br />" + sEmail + "</p>" +
                        "</td></tr>";

                    // Set up the SMTP Server
                    SmtpClient oSmtpServer = new SmtpClient(sEmailServer);
                    oSmtpServer.Credentials = new System.Net.NetworkCredential(sEmailServerUser, sEmailServerPass);

                    // Send the Email to the Web Administrator
                    MailMessage oMail3 = new MailMessage();
                    oMail3.From = new MailAddress(sEmailFrom);
                    oMail3.To.Add(sEmailWebAdmin);
                    oMail3.Subject = sEmailSubjectToAdmin;
                    oMail3.IsBodyHtml = true;
                    oMail3.Body = sEmailHeaderBeginning + sEmailStyles + sEmailHeaderEnding + sEmailBodyBeginning + sEmailBodyHeader + sEmailBodyAdmin + sEmailContentWebAdmin + sEmailFooterAdmin;
                    oSmtpServer.Send(oMail3);
                }
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                // Catch the exception
            }
        }

        // Create the email template pieces
        static string sEmailHeaderBeginning = "<!DOCTYPE HTML PUBLIC '-//W3C//DTD XHTML 1.0 Transitional //EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html style='background-color:#ececec;margin-top:0;margin-bottom:0;margin-right:0;margin-left:0;padding-top:0;padding-bottom:0;padding-right:0;padding-left:0;' ><head><title></title><meta http-equiv='Content-Type' content='text/html; charset=utf-8' />";
        static string sEmailStyles = "<style type='text/css'>" + 
            "@media only screen and (max-device-width: 480px) { " + 
                "table[class=w0], td[class=w0] { width: 0 !important; }" + 
                "table[class=w10], td[class=w10], img[class=w10] { width:10px !important; }" + 
                "table[class=w15], td[class=w15], img[class=w15] { width:5px !important; }" + 
                "table[class=w30], td[class=w30], img[class=w30] { width:10px !important; }" + 
                "table[class=w60], td[class=w60], img[class=w60] { width:10px !important; }" +
                "table[class=w125], td[class=w125], img[class=w125] { width:80px !important; }" + 
                "table[class=w130], td[class=w130], img[class=w130] { width:55px !important; }" + 
                "table[class=w140], td[class=w140], img[class=w140] { width:90px !important; }" + 
                "table[class=w160], td[class=w160], img[class=w160] { width:180px !important; }" + 
                "table[class=w170], td[class=w170], img[class=w170] { width:100px !important; }" +
                "table[class=w180], td[class=w180], img[class=w180] { width:80px !important; }" +
                "table[class=w195], td[class=w195], img[class=w195] { width:80px !important; }" +
                "table[class=w220], td[class=w220], img[class=w220] { width:80px !important; }" +
                "table[class=w240], td[class=w240], img[class=w240] { width:180px !important; }" +
                "table[class=w255], td[class=w255], img[class=w255] { width:185px !important; }" +
                "table[class=w275], td[class=w275], img[class=w275] { width:135px !important; }" +
                "table[class=w280], td[class=w280], img[class=w280] { width:135px !important; }" +
                "table[class=w300], td[class=w300], img[class=w300] { width:140px !important; }" +
                "table[class=w325], td[class=w325], img[class=w325] { width:95px !important; }" +
                "table[class=w360], td[class=w360], img[class=w360] { width:140px !important; }" +
                "table[class=w410], td[class=w410], img[class=w410] { width:180px !important; }" +
                "table[class=w470], td[class=w470], img[class=w470] { width:200px !important; }" +
                "table[class=w580], td[class=w580], img[class=w580] { width:280px !important; }" +
                "table[class=w640], td[class=w640], img[class=w640] { width:300px !important; }" +
                "table[class=hide], td[class=hide], img[class=hide], p[class=hide], span[class=hide], .hide { display:none !important; }" +
                "table[class=h0], td[class=h0] { height: 0 !important; }" +
                "p[class=footer-content-left] { text-align: center !important; }" +
                "#headline p { font-size: 30px !important; }" +
            "}" +
            "#outlook a { padding: 0; }" +
            "body { width: 100% !important; }" +
            ".ReadMsgBody { width: 100%; }" +
            ".ExternalClass { width: 100%; display:block !important; }" +
            "html, body { background-color: #ececec; margin: 0; padding: 0; }" +
            "img { height: auto; line-height: 100%; outline: none; text-decoration: none; display: block; }" +
            "br, strong br, b br, em br, i br { line-height:100%; }" +
            "h1, h2, h3, h4, h5, h6 { line-height: 100% !important; -webkit-font-smoothing: antialiased; }" +
            "h1 a, h2 a, h3 a, h4 a, h5 a, h6 a { color: blue !important; }" +
            "h1 a:active, h2 a:active,  h3 a:active, h4 a:active, h5 a:active, h6 a:active { color: red !important; }" +
            "h1 a:visited, h2 a:visited,  h3 a:visited, h4 a:visited, h5 a:visited, h6 a:visited { color: purple !important; }" +
            "table td, table tr { border-collapse: collapse; }" +
            ".yshortcuts, .yshortcuts a, .yshortcuts a:link,.yshortcuts a:visited, .yshortcuts a:hover, .yshortcuts a span {" +
                "color: black; text-decoration: none !important; border-bottom: none !important; background: none !important;" +
            "}" +
            "code {" +
                "white-space: normal;" +
                "word-break: break-all;" +
            "}" +
            "#background-table { background-color: #ececec; }" +
            "#top-bar { border-radius:6px 6px 0px 0px; -moz-border-radius: 6px 6px 0px 0px; -webkit-border-radius:6px 6px 0px 0px; -webkit-font-smoothing: antialiased; background-color: #34377D; color: #ffffff; }" +
            "#top-bar a { font-weight: bold; color: #ffffff; text-decoration: none; }" +
            "#footer { border-radius:0px 0px 6px 6px; -moz-border-radius: 0px 0px 6px 6px; -webkit-border-radius:0px 0px 6px 6px; -webkit-font-smoothing: antialiased; }" +
            "body { font-family: Lucida Grande, Lucida, Verdana, sans-serif; }" +
            ".header-content, .footer-content-left, .footer-content-right { -webkit-text-size-adjust: none; -ms-text-size-adjust: none; }" +
            ".header-content { font-size: 12px; color: #ffffff; }" +
            ".header-content a { font-weight: bold; color: #ffffff; text-decoration: none; }" +
            "#headline p { color: #d9fffd; font-family: Helvetica Neue, Arial, Helvetica, Geneva, sans-serif; font-size: 36px; text-align: center; margin-top:0px; margin-bottom:30px; }" +
            "#headline p a { color: #d9fffd; text-decoration: none; }" +
            ".article-title { font-size: 18px; line-height:24px; color: #16195B; font-weight:bold; margin-top:0px; margin-bottom:18px; font-family: Lucida Grande, Lucida, Verdana, sans-serif; }" +
            ".article-title a { color: #16195B; text-decoration: none; }" +
            ".article-title.with-meta {margin-bottom: 0;}" +
            ".article-meta { font-size: 13px; line-height: 20px; color: #ccc; font-weight: bold; margin-top: 0; }" +
            ".article-content { font-size: 13px; line-height: 18px; color: #34377D; margin-top: 0px; margin-bottom: 18px; font-family: Lucida Grande, Lucida, Verdana, sans-serif; }" +
            ".article-content a { color: #00AEFF; font-weight:bold; text-decoration:none; }" +
            ".article-content img { max-width: 100% }" +
            ".article-content ol, .article-content ul { margin-top:0px; margin-bottom:18px; margin-left:19px; padding:0; }" +
            ".article-content li { font-size: 13px; line-height: 18px; color: #34377D; }" +
            ".article-content li a { color: #00AEFF; text-decoration:underline; }" +
            ".article-content p { margin-bottom: 15px; }" +
            ".footer-content-left { font-size: 12px; line-height: 15px; color: #ffffff; margin-top: 0px; margin-bottom: 15px; }" +
            ".footer-content-left a { color: #ffffff; font-weight: bold; text-decoration: none; }" +
            ".footer-content-right { font-size: 11px; line-height: 16px; color: #ffffff; margin-top: 0px; margin-bottom: 15px; }" +
            ".footer-content-right a { color: #ffffff; font-weight: bold; text-decoration: none; }" +
            "#footer { background-color: #34377D; color: #ffffff; }" +
            "#footer a { color: #ffffff; text-decoration: none; font-weight: bold; }" +
            "#permission-reminder { white-space: normal; }" +
            "#street-address { color: #ffffff; white-space: normal; }" +
            "</style>" +
            "<!--[if gte mso 9]>" +
            "<style _tmplitem='386' >" +
                ".article-content ol, .article-content ul {" +
                    "margin: 0 0 0 24px;" +
                    "padding: 0;" +
                    "list-style-position: inside;" +
                "}" +
            "</style>" +
            "<![endif]-->";
        static string sEmailHeaderEnding = "<meta name='robots' content='noindex,nofollow'></meta><meta property='og:title' content='Receipt from Allergy, Asthma and Sinus Center'></meta></head>";
        static string sEmailBodyBeginning = "<body style='width:100% !important;background-color:#ececec;margin-top:0;margin-bottom:0;margin-right:0;margin-left:0;padding-top:0;padding-bottom:0;padding-right:0;padding-left:0;font-family:" + sEmailFont + ";'>" + 
            "<table width='100%' cellpadding='0' cellspacing='0' border='0' id='background-table' style='background-color:#" + sEmailBkgdColor + ";' >" + 
	        "<tbody><tr style='border-collapse:collapse;'>" + 
		    "<td align='center' bgcolor='#" + sEmailBkgdColor + "' style='border-collapse:collapse;'>" + 
			"<table class='w640' width='640' cellpadding='0' cellspacing='0' border='0'>" + 
			"<tbody><tr style='border-collapse:collapse;' ><td class='w640' width='640' height='20' style='border-collapse:collapse;' ></td></tr>" + 
			"<tr style='border-collapse:collapse;'>" + 
			"<td class='w640' width='640' style='border-collapse:collapse;'>" + 
			"<table id='top-bar' class='w640' width='640' cellpadding='0' cellspacing='0' border='0' bgcolor='#" + sEmailTopBarColor + "' style='border-radius:" + sEmailTopBarRadius + ";-moz-border-radius:" + sEmailTopBarRadius + ";-webkit-border-radius:" + sEmailTopBarRadius + ";-webkit-font-smoothing:antialiased;background-color:#" + sEmailTopBarColor + ";color:#" + sEmailTopBarFontColor + ";'>" + 
			"<tbody><tr style='border-collapse:collapse;'>" +
			"<td class='w15' width='15' style='border-collapse:collapse;'></td>" + 
			"<td class='w325' width='350' valign='middle' align='left' style='border-collapse:collapse;'>" + 
			"<table class='w325' width='350' cellpadding='0' cellspacing='0' border='0'>" + 
			"<tbody><tr style='border-collapse:collapse;'><td class='w325' width='350' height='3' style='border-collapse:collapse;'></td></tr>" + 
			"</tbody></table><table class='w325' width='350' cellpadding='0' cellspacing='0' border='0'>" +
			"<tbody><tr style='border-collapse:collapse;'><td class='w325' width='350' height='3' style='border-collapse:collapse;'></td></tr>" +
			"</tbody></table></td><td class='w30' width='30' style='border-collapse:collapse;'></td>" + 
			"<td class='w255' width='255' valign='middle' align='right' style='border-collapse:collapse;'>" + 
			"<table class='w255' width='255' cellpadding='0' cellspacing='0' border='0'>" + 
			"<tbody><tr style='border-collapse:collapse;'><td class='w255' width='255' height='3' style='border-collapse:collapse;'></td></tr>" + 
			"</tbody></table><table cellpadding='0' cellspacing='0' border='0'><tbody><tr style='border-collapse:collapse;'></tr></tbody></table>" +
			"<table class='w255' width='255' cellpadding'0' cellspacing='0' border='0'>" + 
			"<tbody><tr style='border-collapse:collapse;'><td class='w255' width='255' height='3' style='border-collapse:collapse;' ></td></tr>" +
			"</tbody></table></td><td class='w15' width='15' style='border-collapse:collapse;'></td></tr></tbody></table></td></tr><tr style='border-collapse:collapse;'>";
        static string sEmailBodyHeader = "<td id='header' class='w640' width='640' align='center' bgcolor='#" + sEmailTopBarColor + "' style='border-collapse:collapse;'>" +
            "<div align='center' style='text-align:center;'><img id='customHeaderImage' src='" + sEmailHeaderImage + "' class='w640' border='0' align='top' style='display:inline;height:auto;line-height:100%;outline-style:none;text-decoration:none;' /></div>" +
            "</td>";
        static string sEmailBodyPayer = "</tr>" + 
			"<tr style='border-collapse:collapse;'><td class='w640' width='640' height='30' bgcolor='#" + sEmailBodyBkgdColor + "' style='border-collapse:collapse;'></td></tr>" + 
			"<tr id='simple-content-row' style='border-collapse:collapse;'><td class='w640' width='640' bgcolor='#" + sEmailBodyBkgdColor + "' style='border-collapse:collapse;'>" + 
			"<table class='w640' width='640' cellpadding='0' cellspacing='0' border='0'><tbody><tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td>" + 
			"<td class='w580' width='580' style='border-collapse:collapse;'><table class='w580' width='580' cellpadding='0' cellspacing='0' border='0'><tbody><tr style='border-collapse:collapse;'>" + 
			"<td class='w580' width='580' style='border-collapse:collapse;'>" + 
			"<p align='left' class='article-title' style='font-size:" + sEmailBodyTitleSize + ";line-height:24px;color:#" + sEmailBodyTitleColor + ";font-weight:bold;margin-top:0px;margin-bottom:18px;font-family:" + sEmailFont + ";'>" + 
            sEmailBodyTitlePayer + "</p>" + 
			"<div align='left' class='article-content' style='font-size:" + sEmailBodyFontSize + ";line-height:18px;color:#" + sEmailBodyFontColor + ";margin-top:0px;margin-bottom:18px;font-family:" + sEmailFont + ";'>" +
			"<p style='margin-bottom:15px;'><span style='font-size:" + sEmailBodySubTitleSize + ";'><strong>" + sEmailBodySubTitlePayer + "</strong></span></p>" + 
			"<table border='0' cellspacing='0' cellpadding='10'><tbody>";
        static string sEmailBodyAdmin = "</tr>" +
            "<tr style='border-collapse:collapse;'><td class='w640' width='640' height='30' bgcolor='#" + sEmailBodyBkgdColor + "' style='border-collapse:collapse;'></td></tr>" +
            "<tr id='simple-content-row' style='border-collapse:collapse;'><td class='w640' width='640' bgcolor='#" + sEmailBodyBkgdColor + "' style='border-collapse:collapse;'>" +
            "<table class='w640' width='640' cellpadding='0' cellspacing='0' border='0'><tbody><tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td>" +
            "<td class='w580' width='580' style='border-collapse:collapse;'><table class='w580' width='580' cellpadding='0' cellspacing='0' border='0'><tbody><tr style='border-collapse:collapse;'>" +
            "<td class='w580' width='580' style='border-collapse:collapse;'>" +
            "<p align='left' class='article-title' style='font-size:" + sEmailBodyTitleSize + ";line-height:24px;color:#" + sEmailBodyTitleColor + ";font-weight:bold;margin-top:0px;margin-bottom:18px;font-family:" + sEmailFont + ";'>" +
            sEmailBodyTitleAdmin + "</p>" +
            "<div align='left' class='article-content' style='font-size:" + sEmailBodyFontSize + ";line-height:18px;color:#" + sEmailBodyFontColor + ";margin-top:0px;margin-bottom:18px;font-family:" + sEmailFont + ";'>" +
            "<p style='margin-bottom:15px;'><span style='font-size:" + sEmailBodySubTitleSize + ";'><strong>" + sEmailBodySubTitleAdmin + "</strong></span></p>" +
            "<table border='0' cellspacing='0' cellpadding='10'><tbody>";
        static string sEmailFooterPayer = "</tbody></table></div></td></tr><tr style='border-collapse:collapse;' ><td class='w580' width='580' height='10' style='border-collapse:collapse;'></td></tr>" + 
			"</tbody></table></td><td class='w30' width='30' style='border-collapse:collapse;'></td></tr></tbody></table></td></tr>" + 
			"<tr style='border-collapse:collapse;'><td class='w640' width='640' height='15' bgcolor='#" + sEmailBodyBkgdColor + "' style='border-collapse:collapse;'></td></tr>" + 
            "<tr style='border-collapse:collapse;'><td class='w640' width='640' style='border-collapse:collapse;'>" + 
			"<table id='footer' class='w640' width='640' cellpadding='0' cellspacing='0' border='0' bgcolor='#" + sEmailTopBarColor + "' style='border-radius:" + sEmailFooterRadius + ";-moz-border-radius:" + sEmailFooterRadius + ";-webkit-border-radius:" + sEmailFooterRadius + ";-webkit-font-smoothing:antialiased;background-color:#" + sEmailTopBarColor + ";color:#" + sEmailTopBarFontColor + ";'>" + 
			"<tbody><tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td><td class='w580 h0' width='360' height='30' style='border-collapse:collapse;'></td>" + 
            "<td class='w0' width='60' style='border-collapse:collapse;'></td><td class='w0' width='160' style='border-collapse:collapse;'></td>" + 
            "<td class='w30' width='30' style='border-collapse:collapse;'></td></tr>" + 
			"<tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td><td class='w580' width='360' valign='top' style='border-collapse:collapse;'>" + 
			"<span class='hide'><p id='permission-reminder' align='left' class='footer-content-left' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:" + sEmailFooterFontSize + ";line-height:15px;color:#" + sEmailTopBarFontColor + ";margin-top:0px;margin-bottom:15px;white-space:normal;'><span>" + sEmailFooterMessagePayer + "</span></p></span>" + 
			"</td><td class='hide w0' width='60' style='border-collapse:collapse;'></td><td class='hide w0' width='160' valign='top' style='border-collapse:collapse;'>" + 
			"<p id='street-address' align='right' class='footer-content-right' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:" + sEmailFooterFontSizeAddress + ";line-height:16px;margin-top:0px;margin-bottom:15px;color:#" + sEmailTopBarFontColor + ";white-space:normal;'>" + sEmailAddress + "</p>" + 
		    "</td><td class='w30' width='30' style='border-collapse:collapse;'></td></tr>" + 
			"<tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td><td class='w580 h0' width='360' height='15' style='border-collapse:collapse;'></td>" + 
            "<td class='w0' width='60' style='border-collapse:collapse;'></td><td class='w0' width='160' style='border-collapse:collapse;'></td>" + 
            "<td class='w30' width='30' style='border-collapse:collapse;'></td></tr></tbody></table></td></tr>" + 
			"<tr style='border-collapse:collapse;'><td class='w640' width='640' height='60' style='border-collapse:collapse;'></td></tr></tbody></table></td></tr></tbody></table></body></html>";
        static string sEmailFooterAdmin = "</tbody></table></div></td></tr><tr style='border-collapse:collapse;' ><td class='w580' width='580' height='10' style='border-collapse:collapse;'></td></tr>" +
            "</tbody></table></td><td class='w30' width='30' style='border-collapse:collapse;'></td></tr></tbody></table></td></tr>" +
            "<tr style='border-collapse:collapse;'><td class='w640' width='640' height='15' bgcolor='#" + sEmailBodyBkgdColor + "' style='border-collapse:collapse;'></td></tr>" +
            "<tr style='border-collapse:collapse;'><td class='w640' width='640' style='border-collapse:collapse;'>" +
            "<table id='footer' class='w640' width='640' cellpadding='0' cellspacing='0' border='0' bgcolor='#" + sEmailTopBarColor + "' style='border-radius:" + sEmailFooterRadius + ";-moz-border-radius:" + sEmailFooterRadius + ";-webkit-border-radius:" + sEmailFooterRadius + ";-webkit-font-smoothing:antialiased;background-color:#" + sEmailTopBarColor + ";color:#" + sEmailTopBarFontColor + ";'>" +
            "<tbody><tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td><td class='w580 h0' width='360' height='30' style='border-collapse:collapse;'></td>" +
            "<td class='w0' width='60' style='border-collapse:collapse;'></td><td class='w0' width='160' style='border-collapse:collapse;'></td>" +
            "<td class='w30' width='30' style='border-collapse:collapse;'></td></tr>" +
            "<tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td><td class='w580' width='360' valign='top' style='border-collapse:collapse;'>" +
            "<span class='hide'><p id='permission-reminder' align='left' class='footer-content-left' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:" + sEmailFooterFontSize + ";line-height:15px;color:#" + sEmailTopBarFontColor + ";margin-top:0px;margin-bottom:15px;white-space:normal;'><span>" + sEmailFooterMessageAdmin + "</span></p></span>" +
            "</td><td class='hide w0' width='60' style='border-collapse:collapse;'></td><td class='hide w0' width='160' valign='top' style='border-collapse:collapse;'>" +
            "<p id='street-address' align='right' class='footer-content-right' style='-webkit-text-size-adjust:none;-ms-text-size-adjust:none;font-size:" + sEmailFooterFontSizeAddress + ";line-height:16px;margin-top:0px;margin-bottom:15px;color:#" + sEmailTopBarFontColor + ";white-space:normal;'>" + sEmailAddress + "</p>" +
            "</td><td class='w30' width='30' style='border-collapse:collapse;'></td></tr>" +
            "<tr style='border-collapse:collapse;'><td class='w30' width='30' style='border-collapse:collapse;'></td><td class='w580 h0' width='360' height='15' style='border-collapse:collapse;'></td>" +
            "<td class='w0' width='60' style='border-collapse:collapse;'></td><td class='w0' width='160' style='border-collapse:collapse;'></td>" +
            "<td class='w30' width='30' style='border-collapse:collapse;'></td></tr></tbody></table></td></tr>" +
            "<tr style='border-collapse:collapse;'><td class='w640' width='640' height='60' style='border-collapse:collapse;'></td></tr></tbody></table></td></tr></tbody></table></body></html>";

    }
}