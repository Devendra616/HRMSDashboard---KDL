using System;

using System.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;
using System.IO;
using System.Diagnostics;
using System.Web.UI;

namespace HRMSDashboard
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string strcon = null;
        String CurUser = null;
        
        private enum TabIndex
        {
            DEFAULT = 0,
            ONE = 1,
            TWO = 2                          
            // you can as many as you want here
        }
        private void SetSelectedTab(TabIndex tabIndex)
        {
            HFCurrTabIndex.Value = ((int)tabIndex).ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CurUser = Session["user"].ToString();
            txtSelectEmployee.Focus();
            if (!IsPostBack) {                             
                
                if (CurUser != null)
                {
                    if (CurUser == "ADMIN" || CurUser == "admin")
                    {
                        ProfileImage.ImageUrl = "Uploads/default.png";
                        txtSelectEmployee.Visible = true;
                    }
                    else
                    {                      
                        loadProfilePic(CurUser);
                        txtSelectEmployee.Text = CurUser;
                        fetchData(txtSelectEmployee.Text);
                    }
                    Tab1.CssClass = "Clicked";
                    MainView.ActiveViewIndex = 0;
                    SetSelectedTab(TabIndex.DEFAULT);
                }
                else
                {
                    Response.Write("Unauthorised Access ...<br>");                    
                    Response.Redirect("~/LoginPg.aspx"); //direct landing to the page
                }
            }
            else
            {
                fetchData(txtSelectEmployee.Text);
            } 

        }

        protected void Tab1_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Clicked";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 0;
            SetSelectedTab(TabIndex.DEFAULT);
        }

        protected void Tab2_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Clicked";
            Tab3.CssClass = "Initial";
            MainView.ActiveViewIndex = 1;
            SetSelectedTab(TabIndex.ONE);
        }

        protected void Tab3_Click(object sender, EventArgs e)
        {
            Tab1.CssClass = "Initial";
            Tab2.CssClass = "Initial";
            Tab3.CssClass = "Clicked";
            MainView.ActiveViewIndex = 2;
            SetSelectedTab(TabIndex.TWO);
        }

        private void loadProfilePic(String user)
        {
            user = user.ToUpper();
            if (isFileExist(".jpg"))
            {
                ProfileImage.ImageUrl = "Uploads/profile_" + user + ".jpg";
            }
            else if (isFileExist(".jpeg"))
            {
                ProfileImage.ImageUrl = "Uploads/profile_" + user + ".jpeg";
            }
            else
            {
                try
                {
                    ProfileImage.ImageUrl = "ImageHandler.ashx?ID=" + user;
                }
                catch(Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "Unable to get profile pic...");
                    ProfileImage.ImageUrl = "Uploads/default.png";
                }
                
                // ProfileImage.ImageUrl = "~/Uploads/default.png";
            }
        }

        protected void txtSelectEmployee_Changed(object sender, EventArgs e)
        {
            // this.CurUser = txtSelectEmployee.Text;
            //  fetchData(CurUser);
            try
          {
                txtSelectEmployee.Text = txtSelectEmployee.Text.ToUpper();
                loadProfilePic(txtSelectEmployee.Text);
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "Unable to get profile pic...");
            }
           
        }

        protected void fetchData(string empId)
        {
            string empUnit ="01";
            empId = empId.ToUpper();
            OracleConnection con = null;
            string getUnit = " select unitcode from employee where empid = '" +empId +"'";

            string qBasic = " select x_sect_desc section,catgdesc, gradecode,doj_currgrade, doj_nmdc, doj_unit, x_proj_desc, dob, firstappmnt_date, present_mobile_res, empid, DECODE(SHORTNAME,NULL,FIRSTNAME,SHORTNAME)  name," +
                            " gender, fathername, mothername, spousename, maritalstat, retirementdate retirementDt, deptname department, round(months_between(sysdate,firstappmnt_Date)/12,2) yrsofservice, religcode religion, castecatg caste, hometown, empqualif, " +
                            " firstappmnt_desigdesc , " +
                            " (select DESIGSHORT desigdesc from designation desig join employee emp on (emp.desigcode = desig.desigcode and desig.isactive = 'Y') where emp.empid = '" + empId + "') presentDesig " +
                            " from employee emp join department dept on (dept.deptcode = emp.deptcode and dept.isactive = 'Y')  " +
                            " LEFT join fam06_sect on(c_sect_code = sectcode and fam06_sect.isactive = 'Y')  LEFT join payscale on(payscale.scaleid = emp.scaleid and payscale.isactive = 'Y') " +
                            " LEFT join fam05_proj on(fam05_proj.c_proj_code = projcode) LEFT join  discipline on(discipline.discipcode = emp.discipcode) " +
                            " join empcatg on(empcatg.catgcode = emp.catgcode and empcatg.isactive = 'Y') where empid = '" + empId + "' and empstatus = 'A' and SUBSTR(emp.EMPID,1,1) BETWEEN 'A' AND 'Z' and  emp.CATGCODE NOT IN ('ADV','CISF')";


            /*   string qBasic = " select x_sect_desc section,catgdesc, gradecode,doj_currgrade, doj_nmdc, doj_unit, x_proj_desc, dob, firstappmnt_date, present_mobile_res, empid, rtrim(concat(concat(concat(concat(title, ''), firstname), ' '), lastname)) Name," +
                               " gender, fathername, mothername, spousename, maritalstat, retirementdate retirementDt, deptname department, round(months_between(sysdate,firstappmnt_Date)/12,2) yrsofservice, religcode religion, castecatg caste, hometown, empqualif, " +
                               " firstappmnt_desigdesc , " +
                               " (select desigdesc from designation desig join employee emp on (emp.desigcode = desig.desigcode and desig.isactive = 'Y') where emp.empid = '" + empId + "') presentDesig " +
                               " from employee emp join department dept on (dept.deptcode = emp.deptcode and dept.isactive = 'Y')  " +
                               " join fam06_sect on(c_sect_code = sectcode and fam06_sect.isactive = 'Y')  join payscale on(payscale.scaleid = emp.scaleid and payscale.isactive = 'Y') " +
                               " join fam05_proj on(fam05_proj.c_proj_code = projcode) join discipline on(discipline.discipcode = emp.discipcode) " +
                               " join empcatg on(empcatg.catgcode = emp.catgcode and empcatg.isactive = 'Y') where empid = '" + empId + "' and empstatus = 'A'";
              */
           
            string qDependents = " select d.dependentname || ' (' ||d.relation|| ') ' Dependents,d.fulldependency fulldependency from empdependents d join employee emp on (d.empid = emp.empid and emp.empstatus = 'A') where d.empid ='" + empId + "' and d.isactive = 'Y' and d.isdependent = 'Y' order by d.gender";
                        
            
            /*string qTransfers = " select  promotionno,entrydate,joineddate,newdesigcode,ndg.desigshort desig,oldunitcode ,newunitcode  , decode(substr(promotionno,1,2),'TR','TRANSFER'),promotiondate, ou.unitshort ou, nu.unitshort nu " +
                                " from emppromotions e,designation ndg,units ou, units nu " +
                                " where empcode = '" + empId + "' and substr(promotionno,1,2) IN ('TR') and ou.unitcode = e.oldunitcode and nu.unitcode = e.newunitcode" +
                                " and ndg.desigcode = e.newdesigcode  order by promotiondate desc"; */
            string qTransfers = " select  promotionno,TRFORDERDATE  entrydate,JOINORDERDATE,JOINEDDATE,DECODE(TO_CHAR(joINORDERdate,'DD/MM/YYYY'),NULL,joineddate ,joinORDERDate) JOINDT ,"+
                                " ndg.desigshort desig, decode(substr(promotionno, 1, 2), 'TR', 'TRANSFER', 'PR', 'PROMOTION') promotype, promotiondate, ou.unitshort ou, nu.unitshort nu "+
                                " from emppromotions e,designation ndg, units ou, units nu "+
                                " where empcode = '" + empId + "' and substr(promotionno, 1, 2) IN('TR') and ou.unitcode = e.oldunitcode " +
                                " and nu.unitcode = e.newunitcode and ndg.desigcode = e.newdesigcode and trfstatus = 'T' order by promotiondate ";
            string qExperiences = " select nvl(compname,decode(exptype,'T','Others','N','NMDC','O','Govt PSU/Others')) compname,nvl(desg.desigshort,'') designation," +
                                 " nvl(startdate, '') startdate,nvl(enddate, '') enddate, NVL(EXP_IN_YEARS, '0') || 'Yrs ' || NVL(EXP_IN_MONTHS, '0') || ' Mths ' || NVL(EXP_IN_DAYS, '0') || ' Days' experience," +
                                 " nvl(unitname, '') unit,nvl(gradecode, '') grade " +
                                  " from  empworkexp, designation desg where empid = '" + empId + "' and empworkexp.desigcode = desg.desigcode(+) " +
                                  " order by startdate";
            /*string qPension =   " SELECT p.fdt fdt,p.tdt tdt,SUM(p.PA) pa, " +
                                " (select sum(temp.pa) FROM PENSIONDATAT temp where temp.tno = p.tno group by tno) totalPension " +
                                " from PENSIONDATAT p  WHERE p.TNO = '" + empId + "' AND substr(p.finyr, 1, 4) = SUBSTR(getfinyear(sysdate), 1, 4) " +
                                " group by TNO,fdt,tdt order by fdt "; */
            string qPension = " SELECT 'WITH INTEREST' PENSDESC,  EMPCODE,DECODE(SHORTNAME, NULL, FIRSTNAME, SHORTNAME) NAME, "+
                               " FROMDATE, TODATE, LIC_ID, OPBAL, PENSION_CONT_YR, INTEREST_ACCRURED, FUNDMGMTCHRGS, SERVTAX, CLOSBAL "+
                               " FROM HRMSFAS.PAY_NMDC_PENSION_SUMM a, Employee E,DESIGNATION D, units u,department de "+
                               " WHERE A.EMPCODE = E.EMPID AND(d.desigcode(+) = e.desigcode)   and E.unitcode = U.unitcode(+) "+
                               " and de.deptcode(+) = e.deptcode "+
                               " AND A.EMPCODE ='"+empId +"' AND FYEARCODE =getfinyear(sysdate)  "+
                               " UNION "+
                               " SELECT 'WITHOUT INTEREST' PENSDESC,  EMPCODE,DECODE(SHORTNAME, NULL, FIRSTNAME, SHORTNAME) NAME,TO_DATE(SAL_PROC_YEARMONTH, 'YYYYMM') FROMDATE,"+
                               " LAST_DAY(TO_DATE(SAL_PROC_YEARMONTH, 'YYYYMM')), LIC_ID, NULL OPBAL, SAL_PENSION_AMT PENSION_CONT_YR, NULL INTEREST_ACCRURED, "+
                               " NULL  FUNDMGMTCHRGS, NULL SERVTAX, NULL  CLOSBAL "+
                               " FROM HRMSFAS.PAY_NMDC_PENSION_DATA a, Employee E,DESIGNATION D, units u,department de "+
                               " WHERE A.EMPCODE = E.EMPID AND(d.desigcode(+) = e.desigcode)   and E.unitcode = U.unitcode(+) "+
                               " and de.deptcode(+) = e.deptcode AND A.EMPCODE ='"+empId +"' AND FYEARCODE =getfinyear(sysdate)  AND "+ 
                               " SAL_PROC_YEARMONTH >  (SELECT DISTINCT MAX(TO_CHAR(TODATE, 'YYYYMM')) FROM HRMSFAS.PAY_NMDC_PENSION_SUMM a, Employee E, DESIGNATION D, units u, department de "+
                               " WHERE A.EMPCODE = E.EMPID AND(d.desigcode(+) = e.desigcode)   and E.unitcode = U.unitcode(+) and de.deptcode(+) = e.deptcode "+
                               " AND A.EMPCODE ='"+empId +"' AND FYEARCODE =getfinyear(sysdate) ) ORDER BY EMPCODE ";


            string qNominee = " SELECT SCHEMEDESC,NOMINEE,A.RELATION RELATION,PERCENTAGE " +
                               " FROM EMPNOMINEES A,BENEFITSCHEME S, EMPLOYEE E WHERE A.SCHEMECODE = S.SCHEMECODE AND E.EMPID = A.EMPID AND A.EMPID ='" + empId + "' " +
                               " order by a.SCHEMECODE";
            String symbolicLink = "";
          
            try
            {
                strcon = ConfigurationManager.ConnectionStrings["HrmsConnection"].ConnectionString;
                if (strcon == null || string.IsNullOrEmpty(strcon))
                {
                    throw new Exception("Couldn't find connection string in web.config.");
                }

                con = new OracleConnection(strcon);
                con.Open();
                using (con)
                {
                    //--------GET UNIT CODE ----------------------------------
                    OracleCommand cmd = new OracleCommand(getUnit, con);
                    OracleDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        empUnit = Convert.ToString(dr["unitcode"]);
                    }

                    switch (empUnit)
                    {
                        case "01": symbolicLink = ""; break;
                        case "03": symbolicLink = "@bchlho"; break;
                        case "04": symbolicLink = "@kdlho"; break;
                        case "05": symbolicLink = "@dnmho"; break;
                        default: symbolicLink = ""; break;
                    }


                    //---------- BASIC & OFFICE DETAILS ------------------------------------------
                    cmd = new OracleCommand(qBasic, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        lblName.Text = Convert.ToString(dr["Name"]);
                        //ViewState["Name"] = lblName.Text;
                        lblDOB.Text = Convert.ToDateTime(dr["dob"]).ToString("dd/MM/yyyy");
                        //ViewState["DOB"] = lblDOB.Text;
                        lblCaste.Text = Convert.ToString(dr["caste"]);
                        lblReligion.Text = Convert.ToString(dr["religion"]);
                        lblFather.Text = Convert.ToString(dr["fathername"]);
                        //ViewState["FatherName"] = lblFather.Text;
                        lblMother.Text = Convert.ToString(dr["mothername"]);
                        //ViewState["MotherName"] = lblMother.Text;
                        lblSpouse.Text = Convert.ToString(dr["spousename"]);
                        //ViewState["SpouseName"] = lblSpouse.Text;
                        String maritialStatus = Convert.ToString(dr["maritalstat"]);
                        if (maritialStatus.Equals("M"))
                        {
                            lblMaritial.Text = "Married";
                        }
                        else if (maritialStatus.Equals("B"))
                        {
                            lblMaritial.Text = "Un-Married";
                        }
                        else if (maritialStatus.Equals("W"))
                        {
                            lblMaritial.Text = "Widowed";
                        }
                        //ViewState["MaritialStatus"] = lblMaritial.Text;
                        lblGrade.Text = Convert.ToString(dr["gradecode"]) + "(" + Convert.ToDateTime(dr["doj_currgrade"]).ToString("dd/MM/yyyy") + ")";
                        //ViewState["Grade"] = lblGrade.Text;
                        lblMobileNo.Text = Convert.ToString(dr["present_mobile_res"]);
                        //ViewState["MobileNo"] = lblMobileNo.Text;
                        lblFirstAppointment.Text = Convert.ToString(dr["firstappmnt_desigdesc"]) + "(" + Convert.ToDateTime(dr["firstappmnt_date"]).ToString("dd/MM/yyyy") + ")";
                        //ViewState["FirstAppointment"] = lblFirstAppointment.Text;
                        lblDOJNMDC.Text = Convert.ToDateTime(dr["doj_nmdc"]).ToString("dd/MM/yyyy");
                        //ViewState["DOJNMDC"] = lblDOJNMDC.Text;
                        lblDOJProj.Text = Convert.ToDateTime(dr["doj_unit"]).ToString("dd/MM/yyyy");
                        //ViewState["DOJProj"] = lblDOJProj.Text;
                        lblDesignation.Text = Convert.ToString(dr["presentDesig"]);
                        //ViewState["Designation"] = lblDesignation.Text;
                        lblDepartment.Text = Convert.ToString(dr["department"]);
                        lblSection.Text = Convert.ToString(dr["section"]);
                        lblYrService.Text = dr.GetDecimal(dr.GetOrdinal("yrsofservice")).ToString() + " yrs";
                        //ViewState["Discipline"] = lblDiscipline.Text;
                        lblEmpID.Text = empId;
                        //ViewState["EmpId"] = lblEmpID.Text;
                        lblProject.Text = Convert.ToString(dr["x_proj_desc"]);
                        //ViewState["Project"] = lblProject.Text;
                        lblCategory.Text = Convert.ToString(dr["catgdesc"]);
                        //ViewState["Category"] = lblCategory.Text;
                        if (Convert.ToString(dr["gender"]).Equals("M"))
                        {
                            lblGender.Text = "Male";
                        }
                        else
                        {
                            lblGender.Text = "Female";
                        }
                        lblRetirementDt.Text = Convert.ToDateTime(dr["retirementDt"]).ToString("dd/MM/yyyy");
                        lblHometown.Text = Convert.ToString(dr["hometown"]);
                        lblQualification.Text = Convert.ToString(dr["empqualif"]);
                        //ViewState["Gender"] = lblGender.Text;
                    }
                    
                    cmd = new OracleCommand(qDependents, con);
                    dr = cmd.ExecuteReader();
                    lstDependents.Items.Clear();
                    lstDependents.Rows = 1;
                    string dependent = null;
                    while (dr.Read())
                    {
                        lstDependents.Rows += 1;

                        dependent = Convert.ToString(dr["Dependents"]);
                        if (Convert.ToString(dr["fulldependency"]) == "Y")
                        {
                            dependent += ", Full Dependent";
                        }
                        else
                        {
                            dependent += ", Partial Dependent";
                        }
                        lstDependents.Items.Add(dependent);
                    }
                    dr.Close();
                    if (lstDependents.Items.Count == 0)
                    {
                        lstDependents.Items.Add("No Dependent");
                    }

                    string qBasicDA = " select (select nvl(amount, 0)  from empsalary"+ symbolicLink + " temp join employee" + symbolicLink + "  emp on (temp.empid = emp.empid and emp.empstatus = 'A') where temp.earndedcode = 'B75' and temp.empid = '" + empId + "') basic, " +
                             " (select nvl(amount, 0)  from empsalary" + symbolicLink + "  temp join employee" + symbolicLink + "  emp on (temp.empid = emp.empid and emp.empstatus = 'A') where temp.earndedcode = 'D60' and temp.empid = '" + empId + "' ) da, " +
                             " (select nvl(amount, 0)  from empsalary" + symbolicLink + "  temp join employee" + symbolicLink + "  emp on (temp.empid = emp.empid and emp.empstatus = 'A') where temp.earndedcode = 'P70' and temp.empid = '" + empId + "' ) fpa " +
                             " from dual ";

                    cmd = new OracleCommand(qBasicDA, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        lblBasic.Text = Convert.ToString(dr["basic"]); ;
                        lblDA.Text = Convert.ToString(dr["da"]);
                        lblFPA.Text = Convert.ToString(dr["fpa"]);
                    }
                    if (lblFPA.Text == "" || lblFPA.Text == null)
                    {
                        FindControl("lblFPA").Parent.Parent.Visible = false;
                    }
                    //dr = null;
                    //  //ViewState["Dependents"] = lstDependents;
                    //---------- LEAVE DETAILS ------------------------------------------
                    string qLeaves = "select l.leavecode leavecode,nvl(l.availed,0) availed,nvl(l.balance,0) balance,nvl(l.encashed,0) encashed from empleaves" + symbolicLink + " l join employee" + symbolicLink + " emp on (l.empid = emp.empid and emp.empstatus = 'A') where l.empid = '" + empId + "' and sysdate between l.periodfrom and l.periodto order by l.leavecode,l.periodfrom ";
                    
                    cmd = new OracleCommand(qLeaves, con);
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        TableRow tRow = new TableRow();
                        TableCell cellLeaveCode = new TableCell();
                        cellLeaveCode.Text = Convert.ToString(dr["leavecode"]);
                        TableCell cellUsed = new TableCell();
                        cellUsed.Text = Convert.ToString(dr["availed"]);
                        TableCell cellBalance = new TableCell();
                        cellBalance.Text = Convert.ToString(dr["balance"]);
                        if (cellLeaveCode.Text.Equals("EL"))
                        {
                            cellUsed.Text += " (Encashed: " + Convert.ToString(dr["encashed"]) + ")";
                        }
                        tRow.Cells.Add(cellLeaveCode);
                        tRow.Cells.Add(cellUsed);
                        tRow.Cells.Add(cellBalance);
                        tRow.CssClass = "rowstyle1";
                        t_leaves.Rows.Add(tRow);
                    }
                    //dr = null;
                    //ViewState["Leaves"] = t_leaves;
                    //---------- LEAVE CLAIM STATUS ------------------------------------------
                    string qLeaveClaims = " select * from (select leavereqno,leavecode,leavefrom||' TO '||leavetill ||'('||apprvdays||')' dates,leavereason reason,leavestatus from leaveapp" + symbolicLink + " " +
                                          " join leaveappdet" + symbolicLink + " det on (leaveapp.intleaveno = det.intleaveno) " +
                                          " where empcode = '" + empId + "' and" +
                                          " det.leavedate between getfirstdayofyr() and getlastdayofyr() order by leavefrom desc,leaveapp.intleaveno desc,slno asc) where rownum <=5";
                    cmd = new OracleCommand(qLeaveClaims, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        TableRow tRow = new TableRow();
                        TableCell cellReqNo = new TableCell();
                        cellReqNo.Text = Convert.ToString(dr["leavereqno"]);
                        TableCell cellLvCode = new TableCell();
                        cellLvCode.Text = Convert.ToString(dr["leavecode"]);
                        TableCell cellDate = new TableCell();
                        cellDate.Text = Convert.ToString(dr["dates"]); 
                        TableCell cellReason = new TableCell();
                        cellReason.Text = Convert.ToString(dr["reason"]);
                        TableCell cellLvStatus = new TableCell();
                        cellLvStatus.Text = Convert.ToString(dr["leavestatus"]);
                        if (cellLvStatus.Text.Equals("A"))
                        {
                            cellLvStatus.Text = "Approved";
                        }
                        else if (cellLvStatus.Text.Equals("C"))
                        {
                            cellLvStatus.Text = "Cancelled";
                        }
                        else if (cellLvStatus.Text.Equals("R"))
                        {
                            cellLvStatus.Text = "Rejected";
                        }
                        else if (cellLvStatus.Text.Equals("I"))
                        {
                            cellLvStatus.Text = "In-Progress";
                        }
                        tRow.Cells.Add(cellReqNo);
                        tRow.Cells.Add(cellLvCode);
                        tRow.Cells.Add(cellDate);
                        tRow.Cells.Add(cellReason);
                        tRow.Cells.Add(cellLvStatus);
                        tRow.CssClass = "rowstyle1";
                        t_lvClaims.Rows.Add(tRow);
                    }
                    //dr = null;
                    //ViewState["LeaveClaims"] = t_lvClaims;
                    //---------- PF STATUS ---------------------------------------------------
                    string qPF = " select sum(nvl(pf.vpf_opbal,0)) vpfOpenBal,sum(nvl(pf.cpf_opbal,0)) cpfOpenBal,sum(nvl(pf.nmdc_opbal,0)) nmdcOpenBal,sum(nvl(pf.loan_opbal,0)) loanOpenBal, sum(nvl(pf.vpf, 0)) curVpf,sum(nvl(pf.cpf, 0)) curCpf,sum(nvl(pf.nmdc, 0)) curNmdc,sum(nvl(pf.loanamount, 0)) curLoan, sum(nvl(pf.LOANREFUNDAMT,0)) loanRefund " +
                         " from pf_monthlysumm"+ symbolicLink + " pf join employee"+ symbolicLink + "  emp on (pf.empid = emp.empid and emp.empstatus = 'A')" +
                         " where pf.empid = '" + empId + "' and pf.status = 2 and pf.fyearcode = getfinyear(sysdate) ";


                    cmd = new OracleCommand(qPF, con);
                    dr = cmd.ExecuteReader();

                    long vpfOpen = 0, cpfOpen = 0, nmdcOpen = 0, loanOpen = 0;
                    long curVpf = 0, curCpf = 0, curNmdc = 0, curLoan = 0, netAmt = 0, loanPaid = 0;
                    while (dr.Read())
                    {
                        TableRow tRow = new TableRow();
                        vpfOpen = Convert.ToInt64(dr["vpfOpenBal"]);
                        TableCell cellVPF = new TableCell();
                        curVpf = Convert.ToInt64(dr["curVpf"]);
                        curVpf = vpfOpen + curVpf;
                        cellVPF.Text = curVpf.ToString();
                        cpfOpen = Convert.ToInt64(dr["cpfOpenBal"]);
                        TableCell cellCPF = new TableCell();
                        curCpf = Convert.ToInt64(dr["curCpf"]);
                        curCpf = cpfOpen + curCpf;
                        cellCPF.Text = curCpf.ToString();
                        nmdcOpen = Convert.ToInt64(dr["nmdcOpenBal"]);
                        TableCell cellNMDC = new TableCell();
                        curNmdc = Convert.ToInt64(dr["curNmdc"]);
                        curNmdc = nmdcOpen + curNmdc;
                        cellNMDC.Text = curNmdc.ToString();
                        loanOpen = Convert.ToInt64(dr["loanOpenBal"]);
                        TableCell cellLnAmt = new TableCell();
                        curLoan = Convert.ToInt64(dr["curLoan"]);
                        loanPaid = Convert.ToInt64(dr["loanRefund"]);
                        curLoan = loanOpen + curLoan - loanPaid;
                        cellLnAmt.Text = curLoan.ToString();
                        TableCell cellNetAmt = new TableCell();
                        netAmt = netAmt + curVpf + curCpf + curNmdc - curLoan;
                        cellNetAmt.Text = netAmt.ToString();
                        tRow.Cells.Add(cellVPF);
                        tRow.Cells.Add(cellCPF);
                        tRow.Cells.Add(cellNMDC);
                        tRow.Cells.Add(cellLnAmt);
                        tRow.Cells.Add(cellNetAmt);
                        tRow.CssClass = "rowstyle1";
                        t_pF.Rows.Add(tRow);
                    }
                    //dr = null;
                    //---------- TA CLAIM STATUS ------------------------------------------
                    string qTA = " select * from (select trim(tabillno) tabillno, billdate, nvl(totamtapplied, 0) amtapplied, nvl(totamtapproved, 0) amtapproved, trim(p.purposedescription) purpose, trim(location.locname) as startplace, loc.locname as endplace, fas2ta02_claimsheader.status " +
                         " from fas2ta02_claimsheader" + symbolicLink + " join location" + symbolicLink + " on (startplace = location.loccode) join location" + symbolicLink + " loc on(endplace = loc.loccode) join fas2ta_purposecodes" + symbolicLink + " p on(p.purposecode = purpose and p.status = 2) where empcode ='" + empId + "'" +
                         " order by INTBILLNO desc, billdate desc) where rownum <=5 ";

                    cmd = new OracleCommand(qTA, con);
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        TableRow tRow = new TableRow();
                        TableCell celltabillno = new TableCell();
                        celltabillno.Text = Convert.ToString(dr["tabillno"]);
                        TableCell cellbilldate = new TableCell(); 
                        cellbilldate.Text = Convert.ToDateTime(dr["billdate"]).ToString("dd/MM/yyyy");
                        TableCell cellamtapplied = new TableCell();
                        cellamtapplied.Text = Convert.ToString(dr["amtapplied"]);
                        TableCell cellamtapproved = new TableCell();
                        cellamtapproved.Text = Convert.ToString(dr["amtapproved"]);
                        TableCell cellpurpose = new TableCell();
                        cellpurpose.Text = Convert.ToString(dr["purpose"]);
                        TableCell cellstartplace = new TableCell();
                        cellstartplace.Text = Convert.ToString(dr["startplace"]);
                        TableCell cellendplace = new TableCell();
                        cellendplace.Text = Convert.ToString(dr["endplace"]);
                        TableCell cellstatus = new TableCell();
                        cellstatus.Text = Convert.ToString(dr["status"]);

                        int tempStatus = Convert.ToInt16(dr["status"]);
                        if (tempStatus == 2)
                        {
                            cellstatus.Text = "Approved";
                        }

                        else if (tempStatus == 1)
                        {
                            cellstatus.Text = "In-Progress";

                        }
                        else if (tempStatus == 3)
                        {
                            cellstatus.Text = "Cancelled";

                        }
                        tRow.Cells.Add(celltabillno);
                        tRow.Cells.Add(cellbilldate);
                        tRow.Cells.Add(cellpurpose);
                        tRow.Cells.Add(cellstartplace);
                        tRow.Cells.Add(cellendplace);
                        tRow.Cells.Add(cellamtapplied);
                        tRow.Cells.Add(cellamtapproved);
                        tRow.Cells.Add(cellstatus);
                        tRow.CssClass = "rowstyle1";
                        t_ta.Rows.Add(tRow);
                    }
                    //dr = null;
                    //------------ TRANSFER DETAILS ---------------------------------------

                    cmd = new OracleCommand(qTransfers, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        TableRow tRow = new TableRow();
                        TableCell cellPrNo = new TableCell();
                        cellPrNo.Text = Convert.ToString(dr["promotionno"]);
                        TableCell cellPrDate = new TableCell();
                        if (!(dr["promotiondate"] is DBNull))
                        {
                            cellPrDate.Text = Convert.ToDateTime(dr["promotiondate"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            cellPrDate.Text = "";
                        }

                        TableCell cellJoinDate = new TableCell();
                        if (!(dr["joineddate"] is DBNull))
                        {
                            cellJoinDate.Text = Convert.ToDateTime(dr["joineddate"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            cellJoinDate.Text = "";
                        }

                        TableCell cellOldUnit = new TableCell();
                        cellOldUnit.Text = Convert.ToString(dr["ou"]);
                        TableCell cellNewUnit = new TableCell();
                        cellNewUnit.Text = Convert.ToString(dr["nu"]);
                        TableCell cellDesig = new TableCell();
                        cellDesig.Text = Convert.ToString(dr["desig"]);

                        tRow.Cells.Add(cellPrNo);
                        tRow.Cells.Add(cellPrDate);
                        tRow.Cells.Add(cellJoinDate);
                        tRow.Cells.Add(cellOldUnit);
                        tRow.Cells.Add(cellNewUnit);
                        tRow.Cells.Add(cellDesig);
                        tRow.CssClass = "rowstyle1";
                        t_transfer.Rows.Add(tRow);
                    }
                    //dr = null;
                    //-------------- EXPERIENCE DETAILS ---------------------------------------------------                 
                    cmd = new OracleCommand(qExperiences, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        TableRow tRow = new TableRow();
                        TableCell cellCompany = new TableCell();
                        cellCompany.Text = Convert.ToString(dr["compname"]);
                        TableCell cellEndDate = new TableCell();
                        if (!(dr["enddate"] is DBNull))
                        {
                            cellEndDate.Text = Convert.ToDateTime(dr["enddate"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            cellEndDate.Text = "";
                        }
                        TableCell cellStartDate = new TableCell();
                        if (!(dr["startdate"] is DBNull))
                        {
                            cellStartDate.Text = Convert.ToDateTime(dr["startdate"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            cellStartDate.Text = "";
                        }
                        TableCell cellDesig = new TableCell();
                        cellDesig.Text = Convert.ToString(dr["designation"]);
                        TableCell cellExperience = new TableCell();
                        cellExperience.Text = Convert.ToString(dr["experience"]);
                        TableCell cellUnit = new TableCell();
                        cellUnit.Text = Convert.ToString(dr["unit"]);
                        TableCell cellGrade = new TableCell();
                        cellGrade.Text = Convert.ToString(dr["grade"]);
                        tRow.Cells.Add(cellCompany);
                        tRow.Cells.Add(cellDesig);
                        tRow.Cells.Add(cellGrade);
                        tRow.Cells.Add(cellStartDate);
                        tRow.Cells.Add(cellEndDate);
                        //tRow.Cells.Add(cellExperience);
                        tRow.Cells.Add(cellUnit);
                        tRow.CssClass = "rowstyle1";
                        t_workExp.Rows.Add(tRow);
                    }

                    //-------------- PENSION DETAILS ---------------------------------------------------                 
                    cmd = new OracleCommand(qPension, con);

                    /* TableCell cellMessage = new TableCell();
                     cellMessage.Text = "(Pension amount without Interest Value)";
                     cellMessage.ColumnSpan = 6;
                     headRow.Cells.Add(cellMessage);
                     headRow.CssClass = "rowstyle1 pension";
                     tblPension.Rows.Add(headRow);
                     */
                    dr = cmd.ExecuteReader();                   
                   
                    while (dr.Read())
                    {
                        TableCell cellDesc = new TableCell();
                        TableCell cellFromDt = new TableCell();
                        TableCell cellToDt = new TableCell();
                        TableCell cellOpBal = new TableCell();
                        TableCell cellYrContri = new TableCell();
                        TableCell cellInterest = new TableCell();
                        TableCell cellFundMgtChrg = new TableCell();
                        TableCell cellServTx = new TableCell();
                        TableCell cellClosingBal = new TableCell();
                        TableRow tRow = new TableRow();
                        cellDesc.Text = Convert.ToString(dr["PENSDESC"]).ToLower();                       
                        if (!(dr["FROMDATE"] is DBNull))
                        {
                            cellFromDt.Text = Convert.ToDateTime(dr["FROMDATE"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            cellFromDt.Text = "";
                        }
                       
                        if (!(dr["TODATE"] is DBNull))
                        {
                            cellToDt.Text = Convert.ToDateTime(dr["TODATE"]).ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            cellToDt.Text = "";
                        }
                        
                        if (!(dr["OPBAL"] is DBNull))
                        {
                            cellOpBal.Text = Convert.ToString(dr["OPBAL"]);
                        }
                        else
                        {
                            cellOpBal.Text = "";
                        }
                       
                        cellYrContri.Text = Convert.ToString(dr["PENSION_CONT_YR"]);
                     
                        cellInterest.Text = Convert.ToString(dr["INTEREST_ACCRURED"]);
                     
                        cellFundMgtChrg.Text = Convert.ToString(dr["FUNDMGMTCHRGS"]);
                      
                        cellServTx.Text = Convert.ToString(dr["SERVTAX"]);
                        
                        cellClosingBal.Text = Convert.ToString(dr["CLOSBAL"]);
                        tRow.Cells.Add(cellDesc);
                        tRow.Cells.Add(cellFromDt);
                        tRow.Cells.Add(cellToDt);
                        tRow.Cells.Add(cellOpBal);
                        tRow.Cells.Add(cellYrContri);
                        tRow.Cells.Add(cellInterest);
                        tRow.Cells.Add(cellFundMgtChrg);
                        tRow.Cells.Add(cellServTx);
                        tRow.Cells.Add(cellClosingBal);

                        tRow.CssClass = "rowstyle1";                        
                        tblPension.Rows.Add(tRow);
                    }
                    //---------- NOMINEE DETAILS ------------------------------------------
                    cmd = new OracleCommand(qNominee, con);
                    dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        TableRow tRow = new TableRow();
                        
                        TableCell cellScheme = new TableCell();
                        cellScheme.Text = Convert.ToString(dr["SCHEMEDESC"]);
                        cellScheme.HorizontalAlign = HorizontalAlign.Left; //left align for scheme only
                        cellScheme.Style.Add("padding-left","10%");
                        TableCell cellNominee = new TableCell();
                        cellNominee.Text = Convert.ToString(dr["NOMINEE"]);
                        TableCell cellRelation = new TableCell();
                        cellRelation.Text = Convert.ToString(dr["RELATION"]);
                        TableCell cellShare = new TableCell();
                        cellShare.Text = Convert.ToString(dr["PERCENTAGE"]);
                        
                        tRow.Cells.Add(cellScheme);
                        tRow.Cells.Add(cellNominee);
                        tRow.Cells.Add(cellRelation);
                        tRow.Cells.Add(cellShare);
                        tRow.CssClass = "rowstyle1";
                        tblNominees.Rows.Add(tRow);
                    }
                    dr.Close();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                if(CurUser == "ADMIN" || CurUser == "admin")
                {
                   ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Attention!", "alert('Data doesn't exist for "+txtSelectEmployee.Text+"')", true);
                    txtSelectEmployee.Text = "";
                }
                else
                {
                    Response.Write("Cannot get the data at this moment ...<br>");
                    Logger.log(ex, empId);
                    Response.Redirect("~/LoginPg.aspx");
                }
                
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }


        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string uName = this.Session["user"].ToString();
            String message;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.onload=function(){");
            sb.Append("alert('");

            if (FileUpload.HasFile)//UploadFile.HasFile// If the user has selected a file
            {
                int fileSize = FileUpload.PostedFile.ContentLength;
                string extension = System.IO.Path.GetExtension(FileUpload.FileName).ToLower();
                if (extension != ".jpg" && extension != ".jpeg")
                {
                    message = "Only jpg or jpeg files are accepted.";
                }
                else if (fileSize > 1048576)
                { //max size 1 MB
                    message = "File size exceeded max limit(1MB).";
                }
                else
                {
                    if (isFileExist(extension)) //delete file if already exist
                    {
                        File.Delete(Server.MapPath("~/Uploads/profile_" + uName + "" + extension));
                    }

                    FileUpload.SaveAs(Server.MapPath("~/Uploads/profile_" + uName + "" + extension));
                    message = "File Uploaded Successfully !";
                }
            }
            else
            {
                message = "Select a file first by clicking on photo";
            }
            sb.Append(message);
            sb.Append("')};");
            sb.Append("</script>");
            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
        }

        private void pushData()
        {
            lblName.Text = ViewState["Name"].ToString();
            lblDOB.Text = ViewState["DOB"].ToString();
            lblFather.Text = ViewState["FatherName"].ToString();
            lblMother.Text = ViewState["MotherName"].ToString();
            lblSpouse.Text = ViewState["SpouseName"].ToString();
            lblMaritial.Text = ViewState["MaritialStatus"].ToString();
            lblGrade.Text = ViewState["Grade"].ToString();
            lblMobileNo.Text = ViewState["MobileNo"].ToString();
            lblFirstAppointment.Text = ViewState["FirstAppointment"].ToString();
            lblDOJNMDC.Text = ViewState["DOJNMDC"].ToString();
            lblDOJProj.Text = ViewState["DOJProj"].ToString();
            lblDesignation.Text = ViewState["Designation"].ToString();
            lblDepartment.Text = ViewState["department"].ToString();
            lblEmpID.Text = ViewState["EmpId"].ToString();
            lblProject.Text = ViewState["Project"].ToString();
            lblCategory.Text = ViewState["Category"].ToString();
            lblGender.Text = ViewState["Gender"].ToString();
            //lstDependents = (ListBox) ViewState["Dependents"];
            t_leaves = (Table)ViewState["Leaves"];
            t_lvClaims = (Table)ViewState["LeaveClaims"];
            t_pF = (Table)ViewState["PF"];
            t_ta = (Table)ViewState["TAClaims"];
        }

        private Boolean isFileExist(string extension)
        {
            return File.Exists(Server.MapPath("Uploads/profile_" + Session["user"].ToString() + extension));

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            if (Session["user"] != null)
            {
                Session.Clear();
                Session.Abandon();
                Response.Redirect("~/LoginPg.aspx");
            }
        }

        private void CreateImage(string userId)
        {

            string qEmpPhoto = "Select emppic from employee where empid = '" + userId + "'";
            OracleConnection con = null;
            try
            {

                strcon = ConfigurationManager.ConnectionStrings["HrmsConnection"].ConnectionString;
                if (strcon == null || string.IsNullOrEmpty(strcon))
                {
                    throw new Exception("Couldn't find connection string in web.config.");
                }

                con = new OracleConnection(strcon);
                con.Open();
                using (con)
                {

                    OracleCommand cmd = new OracleCommand(qEmpPhoto, con);
                    byte[] buf = (byte[])cmd.ExecuteScalar();
                    Response.BinaryWrite(buf);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                Logger.log(ex, userId);

            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }


            }
        }
    }
}