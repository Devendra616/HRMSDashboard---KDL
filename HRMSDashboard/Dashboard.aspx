<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="HRMSDashboard.Dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard Page</title>
    <style type="text/css">   
       
        legend{
            color:#2874A6;
            font-style:italic;
            font-size:larger;
            font-weight:bold;
            background-color:#FAD7A0;
        }      

        .rowstyle1
        {
            text-align: center;
        }

        *{margin:0;padding:0}

        .profileImageCss {
           position:absolute;
           right:55px;
           top:5px;
           width:100px;
           height:100px;
           background-color:#C4B7B5;
           margin:0px 0px 0px 0px;        
        }
          .profileImageCss:hover{
            width:200px;
            height:200px;
        }
		
		
		.txtSelectEmp {
           position:absolute;
           right:200px;
           text-align:center;
           top:35px;
           width:60px;
           height:20px;
           background-color:#b6ff00;
           color:#ed0c0c;
           margin:0px 0px 0px 0px;
        }
      
       .btnUploadCSS {
           position:absolute;
           right:55px;
           top:100px;

           width:100px;
           height:20px;
           background-color:#C4B7B5;
           color:darkred;
           margin:0px 0px 0px 0px;
       }
       .logoutCss {
           position:absolute;
           right:5px;
           top:5px;
           width:50px;
           height:60px;
           background-color:#C4B7B5;
           color:darkred;
           margin:0px 0px 0px 0px;
       }
        .footer {
          position: absolute;
          right: 0;
          width:100%;
          height:10px;
          bottom: 0;
          left: 0;
          padding: 1rem;
          color:#339933;
          font-weight:bold;
          background-color: #C4B7B5;
          text-align: center;
        }
        #lblHeader {
           font-family:Lucida Handwriting, Helvetica, sans-serif;
           font-size:xx-large;            
           text-decoration:underline;
           width:100%;
           height:60px;
           text-align: center;
           position:relative;
           top:10px;
           left:120px;          
        }   


        .Initial
            {
              display: block;
              padding: 7px 18px 7px 18px;
              float: left;             
              background-color:#E8E581;
              color: #086507;
              font-weight: bold;
            }
            .Initial:hover
            {
              color: white;            
             background-color:#BDB952;
            }
            .Clicked
            {
              float: left;
              display: block;            
              background-color:#BDB952;
              padding: 7px 18px 7px 18px;            
              font-weight: bold;
              color: #1A766D;
            }   
            .pension{                
                color:red;
                font-size: 18px;
                font-style:italic;
            }

    </style>
    <script type="text/javascript">
        function chooseFile() {
            document.getElementById("FileUpload").click();
            return false; //disable autopostback
        }
    </script>
</head>
<body>
    <form id="dashboardFrm" runat="server">
        
        <asp:Table runat="server" Width="100%" >
                <asp:TableHeaderRow Height="70px" BackColor="#C4B7B5">
                    <asp:TableCell Width="10%" ><img src="Images/nmdclogo.png" alt="logo"  style="position:absolute;left:5px;width:100px;height:60px; top:5px;margin:0px 0 0 0px"/></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Justify" Width="75%"><asp:Label ID="lblHeader" runat="server" Font-Bold="True" Font-Underline="True" Font-Overline="True" ForeColor="#339933">HRMS EMPLOYEE DASHBOARD</asp:Label></asp:TableCell>
                    <asp:TableCell HorizontalAlign="Right" Width="10%" >                        
                        <asp:TextBox ID="txtSelectEmployee" runat="server" BorderWidth="1" AutoPostBack="True" ToolTip="Enter Employee ID" Visible="False" OnTextChanged="txtSelectEmployee_Changed" CssClass="txtSelectEmp"></asp:TextBox>                       
                        <asp:ImageButton ID="ProfileImage" runat="server" AlternateText="Profile Image" CssClass="profileImageCss" ToolTip="Upload Profile Pic" OnClientClick="chooseFile(); return false;" Enabled="true"  />                        
                        <asp:Button ID="btnUpload" runat="server" Text="Upload Photo" onclick="btnUpload_Click" CssClass="btnUploadCSS" />
                        <asp:FileUpload ID="FileUpload"  runat="server" style="height: 0px; overflow: hidden"/>
                    </asp:TableCell >
                    <asp:TableCell HorizontalAlign="Right" Width="5%"><asp:ImageButton ID="Btnlogout" runat="server" AlternateText="Logout Image" CssClass="logoutCss" ToolTip="Logout"  OnClick="btnLogout_Click"  ImageUrl="~/Images/logout.png" /></asp:TableCell>
                </asp:TableHeaderRow>
            </asp:Table>     
         <asp:HiddenField runat="server" ID="HFCurrTabIndex" /> <!--********************************** -->
    <div style="position:relative;width:100%;">    
        <table style="width: 100%;" border="1">
            <col style="width:43%"/>
            <col style="width:57%"/>
            <tr id="tr_tab">
                <td colspan="2">
                    <asp:Button Text="Basic Details" BorderStyle="None" ID="Tab1" CssClass="Initial" runat="server" OnClick="Tab1_Click" />
                    <asp:Button Text="Leave/TA Claims" BorderStyle="None" ID="Tab2" CssClass="Initial" runat="server" OnClick="Tab2_Click" />  
                    <asp:Button Text="Pension/Nominee Details" BorderStyle="None" ID="Tab3" CssClass="Initial" runat="server" OnClick="Tab3_Click" />  
                    <asp:MultiView ID="MainView" runat="server">
                    <asp:View ID="View1" runat="server">    
                                         

            <tr id="tr_1">
                <td style="background-position: center center; table-layout: auto; background-image: url('Images/basic.png'); background-attachment: scroll; background-repeat: no-repeat;">
                <fieldset><legend>Basic Details</legend>
                    <asp:Table ID="t_basic" runat="server">                       
                     <asp:TableRow>
                            <asp:TableCell >Name</asp:TableCell>
                            <asp:TableCell ColumnSpan="3" >
                                <asp:Label ID="lblName" runat="server" ></asp:Label>
                            </asp:TableCell>                            
                        </asp:TableRow>                        
                        <asp:TableRow>
                            <asp:TableCell Width="20%">Gender</asp:TableCell>
                            <asp:TableCell Width="30%"><asp:Label ID="lblGender" runat="server" Text=""></asp:Label></asp:TableCell>
                            <asp:TableCell Width="20%">DOB</asp:TableCell>
                            <asp:TableCell Width="30%"><asp:Label ID="lblDOB" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell Width="20%">Caste</asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="lblCaste" runat="server" Text=""></asp:Label></asp:TableCell><asp:TableCell Width="20%">Religion</asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="lblReligion" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell>Mother Name</asp:TableCell><asp:TableCell><asp:Label ID="lblMother" runat="server" Text="" /></asp:TableCell><asp:TableCell>Father Name</asp:TableCell><asp:TableCell><asp:Label ID="lblFather" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell>Maritial Status</asp:TableCell><asp:TableCell><asp:Label ID="lblMaritial" runat="server" Text="" /></asp:TableCell><asp:TableCell>Spouse Name</asp:TableCell><asp:TableCell><asp:Label ID="lblSpouse" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell>Dependents</asp:TableCell><asp:TableCell ColumnSpan="3">
                                <asp:ListBox ID="lstDependents" runat="server" Enabled="False" Width="100%">
                                </asp:ListBox>
                            </asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell Width="20%">Hometown</asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="lblHometown" runat="server" Text=""></asp:Label></asp:TableCell><asp:TableCell Width="20%">Qualification</asp:TableCell><asp:TableCell Width="30%"><asp:Label ID="lblQualification" runat="server" Text="" /></asp:TableCell></asp:TableRow>
					
                    </asp:Table>
                </fieldset>
                </td>
                <td style="background-position: center center; table-layout: auto; background-image: url('Images/office.png'); background-attachment: scroll; background-repeat: no-repeat;">


                <fieldset><legend>Office Details</legend><asp:Table ID="t_office" runat="server" Width="100%">

                     <asp:TableRow>
                        <asp:TableCell Width="20%">Employee ID</asp:TableCell><asp:TableCell><asp:Label ID="lblEmpID" runat="server" Text="" /></asp:TableCell><asp:TableCell>Category</asp:TableCell><asp:TableCell><asp:Label ID="lblCategory" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>


                         <asp:TableCell>Designation</asp:TableCell><asp:TableCell ColumnSpan="3"><asp:Label ID="lblDesignation" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>
                        <asp:TableCell>Department</asp:TableCell><asp:TableCell><asp:Label ID="lblDepartment" runat="server" Text="" /></asp:TableCell><asp:TableCell>Grade</asp:TableCell><asp:TableCell><asp:Label ID="lblGrade" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>



                         <asp:TableCell>Section</asp:TableCell><asp:TableCell><asp:Label ID="lblSection" runat="server" Text="" /></asp:TableCell><asp:TableCell>Project</asp:TableCell><asp:TableCell><asp:Label ID="lblProject" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow >

                         <asp:TableCell>Basic</asp:TableCell><asp:TableCell><asp:Label ID="lblBasic" runat="server" Text="" /></asp:TableCell><asp:TableCell>DA</asp:TableCell><asp:TableCell><asp:Label ID="lblDA" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>
                        <asp:TableCell>PFA</asp:TableCell><asp:TableCell><asp:Label ID="lblFPA" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>
                           <asp:TableCell>Mobile No.</asp:TableCell><asp:TableCell><asp:Label ID="lblMobileNo" runat="server" Text="" /></asp:TableCell><asp:TableCell>DOJ Project</asp:TableCell><asp:TableCell><asp:Label ID="lblDOJProj" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>                            


                            <asp:TableCell>First Appointment</asp:TableCell><asp:TableCell><asp:Label ID="lblFirstAppointment" runat="server" Text="" /></asp:TableCell><asp:TableCell>DOJ NMDC</asp:TableCell><asp:TableCell><asp:Label ID="lblDOJNMDC" runat="server" Text="" /></asp:TableCell></asp:TableRow><asp:TableRow>                     



                            <asp:TableCell>Years Of Service</asp:TableCell><asp:TableCell ><asp:Label ID="lblYrService" runat="server" Text="" /></asp:TableCell><asp:TableCell ForeColor="#FF0000">Retirement Date</asp:TableCell><asp:TableCell ForeColor="#FF0000"><asp:Label ID="lblRetirementDt" runat="server" Text="" /></asp:TableCell></asp:TableRow></asp:Table></fieldset> 
							</td>
            </tr>
        <tr id="tr_2">

                <td style="background-position: center center; table-layout: auto; background-image: url('Images/leaves.png'); background-attachment: scroll; background-repeat: no-repeat;">             
                    <fieldset><legend>Leave Details</legend><asp:Table ID="t_leaves" runat="server" Width="100%" >

                    <asp:TableHeaderRow>
                      <asp:TableHeaderCell Width="33%" >TYPE</asp:TableHeaderCell><asp:TableHeaderCell Width="33%">USED</asp:TableHeaderCell><asp:TableHeaderCell>BALANCE</asp:TableHeaderCell></asp:TableHeaderRow></asp:Table></fieldset>


					  </td>
					   <td style="background-position: center center; table-layout: auto; background-image: url('Images/pf.png'); background-attachment: scroll; background-repeat: no-repeat; width:100%;">             
                    <fieldset><legend>PF Details</legend><asp:Table ID="t_pF" runat="server" Width="100%" >

                        <asp:TableHeaderRow Font-Size="Small">                          


                         
                            <asp:TableHeaderCell Width="20%">VPF BALANCE</asp:TableHeaderCell><asp:TableHeaderCell Width="20%">CPF BALANCE</asp:TableHeaderCell><asp:TableHeaderCell Width="20%">NMDC BALANCE</asp:TableHeaderCell><asp:TableHeaderCell Width="20%">LOAN BALANCE</asp:TableHeaderCell><asp:TableHeaderCell >NET BALANCE</asp:TableHeaderCell></asp:TableHeaderRow></asp:Table></fieldset> 
							
							
							</td>
					  
					  
					  </tr>
            <tr id="tr_3">
                
            </tr>
            <tr id="tr_4">
                <td colspan="2" style="background-position: center center; table-layout: auto; background-image: url('Images/transfer.jpg'); background-attachment: scroll; background-repeat: no-repeat;width:100%;">             
                    <fieldset><legend>Transfer Details</legend>
                        <asp:Table ID="t_transfer" runat="server" Width="100%" >
                        <asp:TableHeaderRow class="rowstyle1" Font-Size="Small">
                            <asp:TableHeaderCell Width="12%" >TRANSFER NO</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="12.5%" >TRANSFER DATE</asp:TableHeaderCell>
                            <asp:TableHeaderCell >JOIN DATE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="12%">FROM PROJECT</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="12%">TO PROJECT</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="25%">DESIGNATION </asp:TableHeaderCell>
                           
                       </asp:TableHeaderRow>
                        </asp:Table>
                    </fieldset>
                </td>
            </tr>
             <tr id="tr_5">
                <td colspan="2" style="background-position: center center; table-layout: auto; background-image: url('Images/work_exp.jpg'); background-attachment: scroll; background-repeat: no-repeat;width:100%;">             
                    <fieldset><legend>Work Experiences</legend>
                        <asp:Table ID="t_workExp" runat="server" Width="100%" >
                        <asp:TableHeaderRow class="rowstyle1" Font-Size="Small">
                            <asp:TableHeaderCell Width="10%">COMPANY</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="25%">DESIGNATION</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="5%">GRADE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="10%">START DATE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="10%">END DATE</asp:TableHeaderCell>                          
                            <asp:TableHeaderCell >UNIT</asp:TableHeaderCell>                           
                           
                       </asp:TableHeaderRow>
                        </asp:Table>
                    </fieldset>
                </td>
            </tr>

                    </asp:View> 
                    <asp:View ID="View2" runat="server">   
                        <tr id="tr_6" >

                          <td colspan="2" style="background-position: center center; table-layout: auto; background-image: url('Images/leaveStatus.png'); background-attachment: scroll; background-repeat: no-repeat;width:50%;">
                            <fieldset><legend>Leave Claims</legend><asp:Table ID="t_lvClaims" runat="server" Width="100%" >
                             <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="10%">REQ. NO</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="10%">TYPE</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="35%">DATES</asp:TableHeaderCell>
                                <asp:TableHeaderCell >REASON</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="15%">STATUS</asp:TableHeaderCell>
                             </asp:TableHeaderRow>
                             </asp:Table>
                            </fieldset>
                        </td>
                        </tr>
                <tr id="tr_7">
                <td colspan="2" style="background-position: center center; table-layout: auto; background-image: url('Images/lta.png'); background-attachment: scroll; background-repeat: no-repeat;width:100%;">             
                    <fieldset><legend>TA Details</legend>
                        <asp:Table ID="t_ta" runat="server" Width="100%" >
                        <asp:TableHeaderRow class="rowstyle1" Font-Size="Small">
                            <asp:TableHeaderCell Width="12%" >BILL NO</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="12.5%" >BILL DATE</asp:TableHeaderCell>
                            <asp:TableHeaderCell >PURPOSE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="12%">START PLACE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="12%">END PLACE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="9%">AMT APPLIED</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="9%">AMT APPROVED</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="8%">STATUS</asp:TableHeaderCell>
                       </asp:TableHeaderRow>
                        </asp:Table>
                    </fieldset>
                </td>
            </tr>
            </asp:View>
            <asp:View ID="View3" runat="server">                   
            <tr id="tr_8">
                <td colspan="2" style="background-position: center center; table-layout: auto; background-image: url('Images/pension.png'); background-attachment: scroll; background-repeat: no-repeat;width:100%;">             
                    <fieldset><legend>Pension Details</legend>
                        <asp:Table ID="tblPension" runat="server" Width="100%" >
                        <asp:TableHeaderRow class="rowstyle1" Font-Size="Small">
                            <asp:TableHeaderCell Width="15%"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="7%" >FROM DATE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="7%" >TO DATE</asp:TableHeaderCell>
                            <asp:TableHeaderCell >OPENING BALANCE</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="15%">YEARLY CONTRIBUTION</asp:TableHeaderCell>  
                            <asp:TableHeaderCell Width="10%">INTEREST ACCRURED</asp:TableHeaderCell>                         
                            <asp:TableHeaderCell Width="10%">FUND MGMT. CHARGES</asp:TableHeaderCell>                         
                            <asp:TableHeaderCell Width="10%">SERVICE TAX</asp:TableHeaderCell>                         
                            <asp:TableHeaderCell Width="12%">CLOSING BALANCE</asp:TableHeaderCell>                         
                       </asp:TableHeaderRow>
                        </asp:Table>
                    </fieldset>
                </td>
            </tr>
            <tr id="tr_9">
                <td colspan="2" style="background-position: center center; table-layout: auto; background-image: url('Images/nominee.png'); background-attachment: scroll; background-repeat: no-repeat;width:100%;">             
                    <fieldset><legend>Nominee Details</legend>
                        <asp:Table ID="tblNominees" runat="server" Width="100%" >
                        <asp:TableHeaderRow class="rowstyle1" Font-Size="Small">
                         
                            <asp:TableHeaderCell Width="40%" >SCHEME</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="20%" >NOMINEE</asp:TableHeaderCell>
                            <asp:TableHeaderCell >RELATION</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="15%" >PERCENTAGE SHARE</asp:TableHeaderCell>                           
                       </asp:TableHeaderRow>
                        </asp:Table>
                    </fieldset>
                </td>
            </tr>
                    </asp:View>
                    </asp:MultiView>                
                </td>
            </tr>  
                     
        </table>
    </div>
     
    
       </form>
</body>
</html>