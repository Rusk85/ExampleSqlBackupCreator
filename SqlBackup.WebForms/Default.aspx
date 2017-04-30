<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SqlBackup.WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h2>Clicking the button starts backing up the Database</h2>
        <p>

            <asp:TextBox
                runat="server" ID="tbConStr"
                placeholder="Connection String goes here (no validation)"
                Columns="129"/>
        </p>
        <div style="margin-left: 17.5em">
            <p>
                <asp:Button
                    id="btnStart"
                    runat="server"
                    Text="Start Backup"/>
            </p>
        </div>
    </div>
</asp:Content>
