<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RITCHARD_Web._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeaderContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="page-header">
        Start Talking
    </div>
    <div class="form-group">
        <label for="txtInput">
            Say something to Ritchard</label>
        <asp:TextBox ID="txtInput" runat="server" CssClass="form-control hastooltip studentsearchbox typeahead tt-input "
            data-toggle="tooltip" data-placement="top" data-original-title="Start typing..."
            type="text" placeholder="Start typing..."></asp:TextBox>
    </div>
    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-lg btn-primary" Text="Talk" OnClick="btnSubmit_Click" />
    <div id="working" runat="server" visible="false">
        <div class="page-header">
            Figuring it out...
        </div>
        <div class="col-md-8">
            <div class="panel panel-default">
                <div class="panel panel-heading">Words Found</div>
                <div class="panel panel-body">

                    <asp:Repeater ID="repeater" runat="server">
                        <HeaderTemplate>
                            <table class="table table-condensed table-striped table-hover">
                                <thead>
                                    <tr>
                                        <th>#</th>
                                        <th>Word</th>
                                        <th>Part(s) of Speech</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="<%# DataBinder.Eval(Container.DataItem, "class")%>">
                                <td><%# DataBinder.Eval(Container.DataItem, "wordNumber")%></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "word")%></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "pos")%></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </tbody>
		            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
