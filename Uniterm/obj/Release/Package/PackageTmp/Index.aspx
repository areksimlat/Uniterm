<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Uniterm.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" type="text/css" href="style.css" />

    <script type="text/javascript">
        function ShowOrHideDiv(divname) {
            var div = document.getElementById(divname);

            if (div.style.display == "block") {
                div.style.display = "none";
            }
            else {
                div.style.display = "block";
            }
            return false;
        }    
    </script>

    <title>Uniterm</title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="mainBlock">
            <div class="toolbarBlock">
                <ul>
                    <li>
                        <asp:LinkButton ID="ButtonNew" runat="server" CssClass="linkButtonStyle" 
                            OnClick="ButtonNew_Click">
                            <asp:Image runat="server" ImageUrl="Icons/new_icon.png"/>
                            <asp:Label runat="server" Text="Nowy"></asp:Label>
                        </asp:LinkButton>
                    </li>

                    <li></li>                      

                    <li>
                        <asp:LinkButton runat="server" CssClass="linkButtonStyle" 
                            OnClientClick="return ShowOrHideDiv('opSekwPanel')">
                            <asp:Image runat="server" ImageUrl="Icons/op_sekw_icon.png"/>
                            <asp:Label runat="server" Text="Sekwen"></asp:Label>
                        </asp:LinkButton>
                    </li>

                    <li>
                        <asp:LinkButton runat="server" CssClass="linkButtonStyle"
                            OnClientClick="return ShowOrHideDiv('opZrownPanel')">
                            <asp:Image runat="server" ImageUrl="Icons/op_zrown_icon.png"/>
                            <asp:Label runat="server" Text="Zrównol"></asp:Label>
                        </asp:LinkButton>
                    </li>

                    <li>
                        <asp:LinkButton runat="server" CssClass="linkButtonStyle"
                            OnClientClick="return ShowOrHideDiv('swapPanel')">
                            <asp:Image runat="server" ImageUrl="Icons/swap_icon.png"/>
                            <asp:Label runat="server" Text="Zamień"></asp:Label>
                        </asp:LinkButton>
                    </li>

                    <li style="width:30px"></li>

                    <li>
                        <asp:LinkButton runat="server" CssClass="linkButtonStyle" 
                            OnClick="ButtonRefresh_Click">
                            <asp:Image runat="server" ImageUrl="Icons/refresh_icon.png"/>
                            <asp:Label runat="server" Text="Odśwież"></asp:Label>
                        </asp:LinkButton>
                    </li>

                    <li>
                        <asp:LinkButton runat="server" CssClass="linkButtonStyle"
                            OnClick="ButtonClear_Click">
                            <asp:Image runat="server" ImageUrl="Icons/clear_icon.png"/>
                            <asp:Label runat="server" Text="Wyczyść"></asp:Label>
                        </asp:LinkButton>
                    </li>

                    <li style="width:30px"></li>

                    <li>
                        <div style="width:210px">
                            <label style="display:block;height:40px;text-align:center;font-family:'Century Gothic';color:white">Czcionka</label>

                            <div style="display:inline-block">
                                <asp:DropDownList ID="ddListFont" runat="server" style="width:150px; margin-right:5px" 
                                    AutoPostBack="true">
                                </asp:DropDownList>

                                <asp:DropDownList ID="ddListFontSize" runat="server" style="width:50px"
                                    AutoPostBack="true">
                                </asp:DropDownList>
                            </div>
                        </div>                                              
                    </li>
                </ul>
            </div>

            <div id="opSekwPanel" class="formBlock">
                <h3>Operator pionowego sekwencjonowania</h3>                 
                  
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Wyrażenie A:" CssClass="label1"></asp:Label> 
                        </td>
                        <td>
                            <asp:TextBox ID="tbSekwA" runat="server"></asp:TextBox>
                        </td>
                    </tr>                

                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Wyrażenie B:" CssClass="label1"></asp:Label>   
                        </td>
                        <td>
                            <asp:TextBox ID="tbSekwB" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Operacja:" CssClass="label1"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButton ID="rbSekwSemicolon" runat="server" GroupName="Group1" Text=";" Checked="true" CssClass="label1"/>
                            <asp:RadioButton ID="rbSekwComma" runat="server" GroupName="Group1" Text="," CssClass="label1"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="ButtonSekwAdd" runat="server" Text="Dodaj" 
                                CssClass="button1" OnClick="ButtonSekwAdd_Click"/>
                        </td>
                    </tr>
                </table>           
            </div>         
            
            <div id="opZrownPanel" class="formBlock"> 
                <h3>Operator pionowego zrównoleglenia</h3>  
                
                <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Wyrażenie A:" CssClass="label1"></asp:Label> 
                        </td>
                        <td>
                            <asp:TextBox ID="tbZrownA" runat="server"></asp:TextBox>
                        </td>
                    </tr>                

                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Wyrażenie B:" CssClass="label1"></asp:Label>   
                        </td>
                        <td>
                            <asp:TextBox ID="tbZrownB" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" Text="Operacja:" CssClass="label1"></asp:Label>
                        </td>
                        <td>
                            <asp:RadioButton ID="rbZrownSemicolon" runat="server" GroupName="Group2" Text=";" Checked="true" CssClass="label1"/>
                            <asp:RadioButton ID="rbZrownComma" runat="server" GroupName="Group2" Text="," CssClass="label1"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="ButtonZrownAdd" runat="server" Text="Dodaj" 
                                CssClass="button1" OnClick="ButtonZrownAdd_Click"/>
                        </td>
                    </tr>
                </table>
            </div>     

            <div id="swapPanel" class="formBlock">
                <h3>Zamień</h3>

                <table>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbSwapA" runat="server" GroupName="Group3" Text="A" CssClass="label1" Checked="true"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RadioButton ID="rbSwapB" runat="server" GroupName="Group3" Text="B" CssClass="label1"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="ButtonSwap" runat="server" Text="Zamień" CssClass="button1" OnClick="ButtonSwap_Click"/>
                        </td>
                    </tr>
                </table>
            </div>

            <div class="canvasBlock">
                <asp:Panel ID="Panel1" runat="server" CssClass="canvasPanel" ScrollBars="Auto">
                    <asp:Image ID="Image1" runat="server"/>
                </asp:Panel>                
            </div>
        </div>
    </form>
</body>
</html>
