<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<script runat="server">
    //private void Page_Init(object sender, System.EventArgs e)
    //{
    //    Context.Handler = this.Page;
    //}
    void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string strWhichReport = Session["ReportType"].ToString();
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Reporting/Rpt" + strWhichReport + ".rdlc");
            ReportDataSource rdc = null;
            switch (strWhichReport)
            {
                case "ActualVsExpected":
                    if (Session["EmployeeTargets"] != null)
                    {
                        List<NIMBOLE.Models.Models.ActualVsExpectedModel> lstLeadModel = (List<NIMBOLE.Models.Models.ActualVsExpectedModel>)Session["EmployeeTargets"];
                        rdc = new ReportDataSource("DataSet1", lstLeadModel);
                        List<ReportParameter> rptParameters = new List<ReportParameter>();
                        rptParameters.Add(new ReportParameter("EmployeeId", Session["EmployeeId"].ToString()));
                        rptParameters.Add(new ReportParameter("FinYear", Session["FinYear"].ToString()));
                        ReportViewer1.LocalReport.SetParameters(rptParameters);

                        //Session["EmployeeTargets"] = null;
                        //Session["EmployeeId"] = null;
                        //Session["FinYear"] = null;

                        ReportViewer1.LocalReport.DataSources.Add(rdc);
                        ReportViewer1.DataBind();
                        ReportViewer1.LocalReport.Refresh();
                    }
                    break;
                case "SalesFunnel":
                    if (Session["SalesFunnel"] != null)
                    {
                        List<NIMBOLE.Models.Models.SalesFunelModel> lstSalesModel = (List<NIMBOLE.Models.Models.SalesFunelModel>)Session["SalesFunnel"];
                        List<NIMBOLE.Models.Models.LeadModel> lstLeadsModel = (List<NIMBOLE.Models.Models.LeadModel>)Session["LeadsByMilestone"];

                        rdc = new ReportDataSource("DataSet1", lstSalesModel);
                        ReportViewer1.LocalReport.DataSources.Add(rdc);
                        ReportViewer1.DataBind();
                        ReportViewer1.LocalReport.Refresh();
                    }
                    break;
            }
        }
        else
        {
            ReportViewer1.LocalReport.Refresh();
        }
    }
</script>

<form id="frmASPX" runat="server">
    <div>
        <asp:scriptmanager id="ScriptManager1" runat="server"></asp:scriptmanager>
        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                if (args.get_error() != undefined) {
                    args.set_errorHandled(true);
                }
            }
        </script>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" ClientIDMode="Predictable" SizeToReportContent="True" AsyncRendering="false" ShowExportControls="True" PageCountMode="Actual" InteractivityPostBackMode="AlwaysAsynchronous"></rsweb:ReportViewer>
    </div>
</form>
