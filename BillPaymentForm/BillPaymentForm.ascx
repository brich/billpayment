<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillPaymentForm.ascx.cs" Inherits="BillPaymentForm.BillPaymentForm" %>

<script type="text/javascript" language="javascript">
    function goBack() {
        history.back();
    }
</script>

<asp:Panel runat="server" Visible="true" ID="Payment">

    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="Error" Headertext="<p class='bottom'>Please correct the following errors:</p>" />

    <div class="form-container">
	    <p>Please fill out this form completely. Uncompleted forms will be discarded.<br />
	    Fields marked with <span style="color: #ff0000;">*</span> are required.</p>
    </div>

    <div class="form-container">&nbsp;</div>
    <div class="form-container"><h2>Patient Information:</h2></div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Patient's First Name:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtCustomerFirstName" runat="server" ToolTip="Please enter the patient's first name." CssClass="special" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the patient's first name.</span></p>" ControlToValidate="txtCustomerFirstName" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Patient's Last Name:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtCustomerLastName" runat="server" ToolTip="Please enter the patient's last name." CssClass="special" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the patient's last name.</span></p>" ControlToValidate="txtCustomerLastName" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Patient's Chart Number:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtCustomerID" runat="server" ToolTip="Please enter the patient's chart number." CssClass="special" />
		    <asp:RegularExpressionValidator ID="RequiredExpressionValidator1" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter a valid patient chart number.</span></p>" ValidationExpression="([a-zA-Z]{0,3}\d{3,5})" ControlToValidate="txtCustomerID" Display="Dynamic" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the patient's chart number.</span></p>" ControlToValidate="txtCustomerID" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">&nbsp;</div>
    <div class="form-container"><h2>Payment Information:&nbsp;&nbsp;<img src="/media/1280/paymenttypes.png" alt="Payment Types" width="166" height="33" style="vertical-align: bottom;" /></h2></div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span> Charge Total - US$:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtChargeTotal" runat="server" ToolTip="Please enter the charge total." CssClass="special" />
		    <asp:RegularExpressionValidator ID="RequiredExpressionValidator2" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter a valid charge total.</span></p>" ValidationExpression="(\d*\.\d{2})" ControlToValidate="txtChargeTotal" Display="Dynamic" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter a charge total.</span></p>" ControlToValidate="txtChargeTotal" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Name on Card:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtName" runat="server" ToolTip="Please enter the name on the card." CssClass="special" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the name on the card.</span></p>" ControlToValidate="txtName" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Card Number:<br /><i>16 Digits Required</i></p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtCCNumber" runat="server" ToolTip="Please enter the card number." CssClass="special" />
		    <asp:RegularExpressionValidator ID="RequiredExpressionValidator3" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter a valid card number.</span></p>" ValidationExpression="(\d{16})" ControlToValidate="txtCCNumber" Display="Dynamic" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the card number.</span></p>" ControlToValidate="txtCCNumber" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
            <p class="bottom"><span style="color: #ff0000;">* </span>Expiration Date:</p>
	    </div>
        <div class="form-element">
	        <div class="form-element-left">
                <asp:DropDownList ID="drpExpMonth" runat="server" ToolTip="Please select the expiration date month." CssClass="special">
                    <asp:ListItem Value="0" Text="Select Month" Selected="True" />
                    <asp:ListItem Value="01" Text="January - 01" />
                    <asp:ListItem Value="02" Text="February - 02" />
                    <asp:ListItem Value="03" Text="March - 03" />
                    <asp:ListItem Value="04" Text="April - 04" />
                    <asp:ListItem Value="05" Text="May - 05" />
                    <asp:ListItem Value="06" Text="June - 06" />
                    <asp:ListItem Value="07" Text="July - 07" />
                    <asp:ListItem Value="08" Text="August - 08" />
                    <asp:ListItem Value="09" Text="September - 09" />
                    <asp:ListItem Value="10" Text="October - 10" />
                    <asp:ListItem Value="11" Text="November - 11" />
                    <asp:ListItem Value="12" Text="December - 12" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" InitialValue="0" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please select the expiration date month.</span></p>" ControlToValidate="drpExpMonth" Display="Dynamic" />
	        </div>
            <div class="form-element-right">
                <asp:DropDownList ID="drpExpYear" runat="server" ToolTip="Please select the expiration date year." CssClass="special">
                    <asp:ListItem Value="0" Text="Select Year" Selected="True" />
                    <asp:ListItem Value="12" Text="2012" />
                    <asp:ListItem Value="13" Text="2013" />
                    <asp:ListItem Value="14" Text="2014" />
                    <asp:ListItem Value="15" Text="2015" />
                    <asp:ListItem Value="16" Text="2016" />
                    <asp:ListItem Value="17" Text="2017" />
                    <asp:ListItem Value="18" Text="2018" />
                    <asp:ListItem Value="19" Text="2019" />
                    <asp:ListItem Value="20" Text="2020" />
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" InitialValue="0" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please select the expiration date year.</span></p>" ControlToValidate="drpExpYear" Display="Dynamic" />
            </div>
        </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Card Security Code:<br /><i><a href="/miscellaneous/CSCCode.aspx" onclick="return hs.htmlExpand(this, { objectType: 'iframe' })">What's This?</a></i></p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtCardCodeValue" runat="server" ToolTip="Please enter the card security code." CssClass="special" />
		    <asp:RegularExpressionValidator ID="RequiredExpressionValidator6" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter a valid card security code.</span></p>" ValidationExpression="(\d{3,4})" ControlToValidate="txtCardCodeValue" Display="Dynamic" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the card security code.</span></p>" ControlToValidate="txtCardCodeValue" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">&nbsp;</div>
    <div class="form-container"><h2>Billing Information:</h2></div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom">Email Address:<br /><i>if you don't include an email address, you will not receive a receipt</i></p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtEmail" runat="server" ToolTip="Please enter the email address." CssClass="special" />
		    <asp:RegularExpressionValidator ID="RequiredExpressionValidator8" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter a valid email address.</span></p>" ValidationExpression="([_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4}))" ControlToValidate="txtEmail" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Address:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtAddress" runat="server" ToolTip="Please enter the address." CssClass="special" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the address.</span></p>" ControlToValidate="txtAddress" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>City:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtCity" runat="server" ToolTip="Please enter the city." CssClass="special" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the city.</span></p>" ControlToValidate="txtCity" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>State:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtState" runat="server" ToolTip="Please enter the state." CssClass="special" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the state.</span></p>" ControlToValidate="txtState" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">
	    <div class="form-label">
		    <p class="bottom"><span style="color: #ff0000;">* </span>Zip Code:</p>
	    </div>
	    <div class="form-element">
		    <asp:TextBox ID="txtZip" runat="server" ToolTip="Please enter the zip code." CssClass="special" />
		    <asp:RegularExpressionValidator ID="RequiredExpressionValidator7" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter a valid zip code.</span></p>" ValidationExpression="(\d{5}(-\d{4})?)" ControlToValidate="txtZip" Display="Dynamic" />
		    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<p class='bottom'><span style='color: #ff0000;'>Please enter the zip code.</span></p>" ControlToValidate="txtZip" Display="Dynamic" />
	    </div>
    </div>

    <div class="form-container">&nbsp;</div>
    <div class="form-container">
	    <p>By clicking submit below, you have agreed to pay the above amount.<br />
        <strong>Please note:</strong> do not click on the Submit button more than once, it could cause multiple payments. Also, there is a two minute timeout placed on submitting this form. If your transaction fails, please wait for at least two minutes before resubmitting the form.</p>
    </div>

    <div class="form-container">&nbsp;</div>
    <div class="form-container">
        <div class="form-label">
            <asp:Button ID="btnSubmit" runat="server" Text=" " ToolTip="Click to submit." OnClick="SubmitButton_Click" CssClass="button" />
        </div>
    </div>

    <div class="form-container">
        <asp:Label runat="server" ID="lblTransactionResult" />
    </div>

</asp:Panel>

<asp:Panel runat="server" Visible="false" ID="Processing">

    <div class="form-container">
        <div class="form-loading">
            <img src="/media/6144/blockpreloader.gif" alt="" width="60" height="60" />
        </div>
        <div class="form-loading">
            <h2>Please wait while we process your payment</h2>
        </div>
    </div>

</asp:Panel>

<asp:Panel runat="server" Visible="false" ID="Success">

    <p>Thank you for using Allergy, Asthma and Sinus Center's Payment Gateway.</p>
    <h2 style="color:#ffffff; padding:7px 10px 5px 10px; background-color:#34377D; border:2px solid #A4E4EB;">YOUR TRANSACTION HAS BEEN APPROVED!</h2>
    <p class="bottom">&nbsp;</p>
    <table border="0" cellspacing="0" cellpadding="10">
        <tbody>
            <tr>
                <td align="left" valign="top">
                    <p class="bottom"><strong>Reference #:</strong></p>
                </td>
                <td align="left" valign="top">
                    <p class="bottom"><asp:Label runat="server" ID="lblReferenceNumber" /></p>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <p class="bottom"><strong>Total Payment:</strong></p>
                </td>
                <td align="left" valign="top">
                    <p class="bottom">$ <asp:Label runat="server" ID="lblPayment" /></p>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <p class="bottom"><strong>Billed To:</strong></p>
                </td>
                <td align="left" valign="top">
                    <p class="bottom"><asp:Label runat="server" ID="lblName" /><br />
                    <asp:Label runat="server" ID="lblAddress1" /><br />
                    <asp:Label runat="server" ID="lblCity" />,&nbsp;<asp:Label runat="server" ID="lblState" />&nbsp;<asp:Label runat="server" ID="lblZip" /><br />
                    <asp:Label runat="server" ID="lblCountry" /><br />
                    <asp:Label runat="server" ID="lblEmail" /></p>
                </td>
            </tr>
        </tbody>
    </table>
    <p class="bottom">&nbsp;</p>
    <p>An email confirmation has been sent to the address that you provided. Please <a href="/contactus.aspx" title="Contact Us">Contact Us</a> if you have any questions about your transaction.</p>

</asp:Panel>

<asp:Panel runat="server" Visible="false" ID="Fail">

    <p>Thank you for using Allergy, Asthma and Sinus Center's Payment Gateway.</p>
    <h2 style="color:#ff0000; padding:7px 10px 5px 10px; background-color:#FFD6D6; border:2px solid #ff0000;">YOUR TRANSACTION HAS BEEN DECLINED!</h2>
    <p class="bottom">&nbsp;</p>
    <p class="bottom"><strong><asp:Label runat="server" ID="lblErrorMessage" /></strong></p>
    <asp:Label runat="server" ID="lblBackButton" Visible="false" />
    <p class="bottom">&nbsp;</p>
    <p>If you have any questions about your transaction, please feel free to <a href="/contactus.aspx" title="Contact Us">Contact Us</a>.</p>

</asp:Panel>