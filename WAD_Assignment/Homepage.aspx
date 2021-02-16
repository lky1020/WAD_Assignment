<%@ Page Language="C#" MasterPageFile="~/WAD.Master" AutoEventWireup="true" CodeBehind="Homepage.aspx.cs" Inherits="WAD_Assignment.Homepage" %>

<asp:Content ID="Homepage" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="banner" id="banner">

        <div class="content">
            <h2>Always Choose The Best</h2>
            <p>
                In the road to capture all the best moments in the life.
                The best moments in life always surround us, helping us, supporting us, and giving us strength.
            </p>
            <a href="#" class='btn'>Our ArtWorks</a>
        </div>

        <video src="img/video/sky.mp4" muted loop autoplay></video>

    </section>

    <section class="about" id="about">
        <div class="row">
            <div class="col50">
                <h2 class="titleText"><span>A</span>bout Us</h2>
                <p> &emsp;&emsp;&emsp;
                    Quad-Core ArtWork Sales and Display Gallery
                    is a company that provides a platform for all
                    artists around the world to launch their artworks, 
                    explore the beauty of artworks and collect them.
                    Quad-Core ArtWork Sales and Display Gallery intend
                    to collect and capture all the beauty moments in
                    the life to promote to all the world. 

                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    Quad-Core ArtWork Sales and Display Gallery (ASG)
                    is founder by 4 students which is Lim Kah Yee, Joan Hau, 
                    Cheong Yin Lam & Lee Ling. Currently, we are studying in
                    the software engineering course (RSF) of Tunku Abdul Rahman 
                    University College (TARUC).
                </p>

                <br/><br/>

                <p> &emsp;&emsp;&emsp;
                    This Quad-Core ASG. website is one of our assignment in RSF2 Year 2 
                    Sem 3. This website is used to apply all the things that we learn 
                    about the Asp.Net website. In this website, it is about the Artworks
                    Sales and Display gallery which is to promote Artwork and also offers 
                    the opportunity for Art Lover to purchase them.
                </p>
            </div>

            <div class="col50">
                <div class="imgBox">
                    <img src="img/homepage/aboutUs/aboutUs.jpg" alt="About Us">
                </div>
            </div>
        </div>
    </section>

    <section class="artworks" id="artworks">
        <div class="title">
            <h2 class="titleText"><span>A</span>rtWorks</h2>

            <p> 
                The best ArtWorks that launched by Artists. Captured the 
                best moment in our life.
            </p>
        </div>

        <div class="content">
            <div class="box">
                <div class="imgBox">
                    <img src="img/homepage/artWork/africa_elephant.jpg" alt="Africa Elephant">
                </div>
                <div class="text">
                    <h3>Africa Elephant</h3>
                </div>
            </div>

            <div class="box">
                <div class="imgBox">
                    <img src="img/homepage/artWork/sky_boat.jpg" alt="Sky Boat">
                </div>
                <div class="text">
                    <h3>Sky Boat</h3>
                </div>
            </div>

            <div class="box">
                <div class="imgBox">
                    <img src="img/homepage/artWork/beautiful_women.jpg" alt="Beautiful Women">
                </div>
                <div class="text">
                    <h3>Beautiful Women</h3>
                </div>
            </div>

            <div class="box">
                <div class="imgBox">
                    <img src="img/homepage/artWork/boat.jpg" alt="Boat">
                </div>
                <div class="text">
                    <h3>Boat</h3>
                </div>
            </div>

            <div class="box">
                <div class="imgBox">
                    <img src="img/homepage/artWork/forest.jpg" alt="Forest">
                </div>
                <div class="text">
                    <h3>Forest</h3>
                </div>
            </div>

            <div class="box">
                <div class="imgBox">
                    <img src="img/homepage/artWork/window.jpg" alt="Window">
                </div>
                <div class="text">
                    <h3>Window</h3>
                </div>
            </div>
        </div>

        <div class="title">
            <a href="#" class="btn">View All</a>
        </div>
    </section>
  
    <section class="contact" id="contact">
        <div class="title">
            <h2 class="titleText"><span>C</span>ontact Us</h2>

            <p> 
                Feel free to Contact Us. 
            </p>
        </div>

        <asp:ScriptManager ID="scriptmanagerContact" runat="server"></asp:ScriptManager>  
        <div class="contactForm">
            <asp:UpdatePanel ID="updatepnl" runat="server">  
                <ContentTemplate> 
                    <h3>Send Message</h3>
                    <div class="inputBox">
                        <asp:TextBox ID="txtContactName" CssClass="contactInput" runat="server" placeholder="Your Name" AutoPostBack="true"></asp:TextBox>
                        <asp:Label ID="lblContactName" CssClass="validationStyle" runat="server"></asp:Label>
                    </div>
                    <div class="inputBox">
                        <asp:TextBox ID="txtContactEmail" CssClass="contactInput" runat="server" placeholder="Your Email" AutoPostBack="true"></asp:TextBox>
                        <asp:Label ID="lblContactEmail" CssClass="validationStyle" runat="server"></asp:Label>
                    </div>
                    <div class="inputBox">
                        <asp:TextBox ID="txtContactComment" TextMode="MultiLine" CssClass="contactInput" runat="server" placeholder="Your Comment" AutoPostBack="true"></asp:TextBox>
                        <asp:Label ID="lblContactComment" CssClass="validationStyle" runat="server"></asp:Label>
                    </div>
                    <div class="inputBox">
                        <asp:Button ID="btnContactSubmit" CssClass="contactSubmit" runat="server" Text="Submit" OnClientClick="changeSubmitText()"  OnClick="btnContactSubmit_Click"/>
                    </div>
                </ContentTemplate>  
            </asp:UpdatePanel> 
        </div>

    </section>

    <script type="text/javascript">
        function changeSubmitText() {
            document.getElementById('<%=btnContactSubmit.ClientID%>').value = "Sending";
            document.getElementById('<%=btnContactSubmit.ClientID%>').style.cursor = "default";
        }

    </script>

</asp:Content>
