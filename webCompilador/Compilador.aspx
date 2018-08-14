<%@ Page Title="Pseudosoft" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Compilador.aspx.cs" Inherits="webCompilador.Compilador" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <!DOCTYPE html>

    <html lang="es">
        <head>
            <meta charset="utf-8">
            <title>Pseudosoft</title>
            <meta name="viewport" content="width=device-width, initial-scale=1">
            <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        </head>

        <body>
            <div class="navbar navbar-default navbar-fixed-top">
                <div class="container">
                    <div class="navbar-header">
                        <a href="index.html" class="navbar-brand">Pseudosoft</a>
                        <button class="navbar-toggle" type="button" data-toggle="collapse" data-target="#navbar-main">
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                            <span class="icon-bar"></span>
                        </button>
                    </div>
                    <div class="navbar-collapse collapse" id="navbar-main">
                        <ul class="nav navbar-nav navbar-right">
                            <li><a href="#" target="_blank">Help</a></li>
                            <li><a href="#" target="_blank">About</a></li>
                        </ul>
                    </div>
                </div>
            </div>

            <div class="container">
                <div class="page-header" id="banner">
                    <div class="row" id="app">
                        <div class="col-md-6">
                            <form id="formCompiler" method="post">
                                <div class="form-group">
                                    <textarea name="code" id="code" rows="22" style="width: 100%;"></textarea>
                                    <input type="hidden" name="_token" value="0MRc0wBkomNUyJValvg6nBz6dSIs4ulww7cAvNP6">
                                </div>
                                <asp:Button ID="btnCompilar" runat="server" Text="Compilar" OnClick="btnCompilar_Click" CssClass="bg-primary" />
                            </form>
                        </div>
                        <div id="term"></div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <h4>Salida</h4>
                                <asp:TextBox ID="resultado" TextMode="MultiLine" Columns="1" Rows="15" runat="server" ReadOnly="true" Width="410px" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <footer>
                    <div class="row">
                        <div class="col-lg-12">
                            <ul class="list-unstyled">
                                <li class="pull-right"><a href="#top">Regresar Arriba</a></li>
                                <!-- <li><a href="#">Blog</a></li>  -->
                                <!--  <li><a href="#">Twitter</a></li>  -->
                                <!-- <li><a href="#">GitHub</a></li>  -->
                            </ul>
                        </div>
                    </div>
                </footer>
            </div>

            <script src="js/jquery-1.10.2.min.js"></script>
            <script src="js/bootstrap.min.js"></script>
            <script src="js/custom.js"></script>
            <script src="js/app.js"></script>
            <script src="js/jquery-linedtextarea.js"></script>

            <script type="text/javascript">

                $(function() {
                    $("#code").linedtextarea(
                            /*{selectedLine: 1}*/
                    );
                });

            </script>
        </body>
    </html>
</asp:Content>
