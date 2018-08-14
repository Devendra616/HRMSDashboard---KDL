<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginPg.aspx.cs" Inherits="HRMSDashboard.LoginPg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NMDC HRMS Dashboard</title>
    <link rel="shortcut icon" href="~/Images/favicon.ico" type="image/x-icon" />
    <script src="Scripts/jquery-1.10.2.min.js"></script>   
    <script src="Scripts/jquery-ui-1.12.1.min.js"></script>
    <link href="Scripts/jquery-ui.min.css" rel="stylesheet" />
    <script src="Scripts/ResponsiveSlides.js"></script>
</head>
<body>
    <script type="text/javascript">
        function clearError() {
            $("#FailureText").prop("text", "");
        }
        $(function () {
            $("#DOB").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd/M/y",
                yearRange: "c-60:c",
                minDate: "-60y", maxDate: "-20y" //from last 60 years to last 20 years
            });
            $(".rslides").responsiveSlides({
                pager: false,               
                speed: 700,
                pause: true,
                maxwidth: "400px"
            });
            $(".rslides").responsiveSlides();
        });
    </script>
    <style>
        /*slider */
        .rslides {
            position: relative;
            list-style: none;
            overflow: hidden;
            width: 100%;
            padding: 0;
            margin: 0;
        }

            .rslides li {
                -webkit-backface-visibility: hidden;
                position: absolute;
                display: none;
                width: 100%;
                left: 0;
                top: 0;
            }

                .rslides li:first-child {
                    position: relative;
                    display: block;
                    float: left;
                }

            .rslides img {
                display: block;
                height: auto;
                float: left;
                width: 100%;
                border: 0;
            }
        /* cellpadding */
        .login > th, td {
            padding: 4px;
        }

        .centered {
            text-align: center;
        }

        .righted {
            text-align: right;
        }

        .login {
            border-collapse: collapse;
            border-spacing: 0;
            height: 300px;
            width: 99%;
        }
        /* cellspacing="0" */
    </style>

    <form id="form1" runat="server">
        <div>
            <table class="login">
                <tr>
                    <td>
                        <ul class="rslides">
                            <li><a href="#">
                                <img src="Images/01.jpg" alt="Fines " />
                            </a></li>
                            <li><a href="#">
                                <img src="Images/02.jpg" alt="Road to hill top" />
                            </a></li>
                            <li><a href="#">
                                <img src="Images/03.jpg" alt="Dumper" />
                            </a></li>
                            <li><a href="#">
                                <img src="Images/04.jpg" alt="11C" />
                            </a></li>
                            <li><a href="#">
                                <img src="Images/05.jpg" alt="Blue Dust" />
                            </a></li>
                            <li><a href="#">
                                <img src="Images/06.jpg" alt="Shovel" />
                            </a></li>
                            <li><a href="#">
                                <img src="Images/07.jpg" alt="11C" />
                            </a></li>
                            <li><a href="#">
                                <img src="Images/08.jpg" alt="Shovel in operation" /></a></li>
                            <li><a href="#">
                                <img src="Images/09.jpg" alt="Dumper - Shovel" /></a></li>
                            <li><a href="#">
                                <img src="Images/10.jpg" alt="11C View Point" /></a></li>
                            <li><a href="#">
                                <img src="Images/11.jpg" alt="11C View Point" /></a></li>
                        </ul>
                    </td>
                    <td>
                        <table style="height: 100%; width: 100%;">
                            <tr>
                                <td colspan="2" style="color: White; background-color: #990000; font-size: 1.9em; font-weight: bold;">Employee HRMS Dashboard</td>
                            </tr>
                            <tr>
                                <td class="righted">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Token No</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="UserName" runat="server" Font-Size="0.8em" Width="113px" ToolTip="Enter Employee Id" onmouseout="javascript:clearError();" OnTextChanged="checkCategory" AutoPostBack="True"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserIDRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User ID is required." SetFocusOnError="True" ToolTip="User ID is required." ValidationGroup="Login" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="righted">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" Font-Size="0.8em" TextMode="Password" Width="113px" onmouseout="javascript:clearError();" TabIndex="1"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="righted" rowspan="2">
                                    <asp:Label ID="DOBLabel" runat="server" AssociatedControlID="DOB">DOB(dd/MON/yy) : <br /> as per records &nbsp;</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="DOB" runat="server" Font-Size="0.8em" Width="113px" ToolTip="Select DOB" onmouseout="javascript:clearError();" TabIndex="2"></asp:TextBox>
                                    <div id="calenderDOB"></div>
                                    <asp:RequiredFieldValidator ID="DOBReqd" runat="server" ErrorMessage="DOB is required." SetFocusOnError="True" ToolTip="DOB is required." ValidationGroup="Login" ForeColor="Red" Display="Dynamic" ControlToValidate="DOB"></asp:RequiredFieldValidator>
                                </td>
                            </tr>

                            <tr>
                                <td class="centered" colspan="2" style="color: Red;">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr>
                                <td class="centered" colspan="2">
                                    <asp:Button ID="LoginButton" runat="server" BackColor="White" BorderColor="#CC9966" BorderStyle="Solid" BorderWidth="1px" CommandName="Login" Font-Names="Verdana" Font-Size="0.8em" ForeColor="#990000" Text="Log In" ValidationGroup="Login" Height="23px" Width="73px" OnClick="LoginButton_Click" TabIndex="3" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td class="righted">
                        <img alt="NMDC Ltd" src="Images/nmdclogo.png" /></td>
                </tr>
            </table>
        </div>
    </form>

    <footer>
        <marquee direction="left" speed="normal" behavior="loop" style="background: #ffeb95; color: #28282a;">Enter your HRMS Credentials for login, if any discripancy in data please contact Personal department(Phone No:6747) for correction!</marquee>
        <p>&copy; <%: DateTime.Now.Year %> C&IT NMDC Kirandul - (For better experience use Google Chrome or Mozilla Firefox)</p>
    </footer>
    <div>
        <label style="font-family: Mangal; font-size: medium">
            <pre style="color: red; font-size: large">
                उपयोगकर्ताओं के लिए निर्देश: <br />
                • कृपया अपना टोकन नंबर अंकित करें।
                • यदि आप अधिशासी संवर्ग के है तो एचआरएमएस पासवर्ड अंकित करें। 
                • यदि आप कामगार संवर्ग के है तो अपनी जन्‍मतिथि अंकित करें।
                • यदि लागइन के पश्‍चात उपलब्‍ध डाटा में किसी भी तरह की कोई विसंगति पाई जाती है तो कृपया कार्मिक विभाग (टेलिफोन नंबर 6747) से संपर्क करें।  
           </pre>
        </label>
    </div>
</body>
</html>
