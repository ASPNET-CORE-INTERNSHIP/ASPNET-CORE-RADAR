#pragma checksum "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "bfdd1cf29c8737135b0b26ed1640422571de28af"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_AdminRadarList_RadarList), @"mvc.1.0.view", @"/Views/AdminRadarList/RadarList.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\_ViewImports.cshtml"
using ASPNETAOP;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\_ViewImports.cshtml"
using ASPNETAOP.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bfdd1cf29c8737135b0b26ed1640422571de28af", @"/Views/AdminRadarList/RadarList.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8114726577ba02b0c53af550574d2018d71987ed", @"/Views/_ViewImports.cshtml")]
    public class Views_AdminRadarList_RadarList : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ASPNETAOP.Models.AdminRadarList>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml"
  
    ViewData["Title"] = "RadarList";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Users</h1>\r\n\r\n<table class=\"table table-bordered table-responsive table-hover\">\r\n    <tr>\r\n        <th>Radar</th>\r\n        <th>Receiver</th>\r\n        <th>Transmitter</th>\r\n        <th>Location</th>\r\n    </tr>\r\n");
#nullable restore
#line 16 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml"
     foreach (var radar in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>");
#nullable restore
#line 19 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml"
           Write(radar.radar);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 20 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml"
           Write(radar.receiver);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 21 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml"
           Write(radar.transmitter);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n            <td>");
#nullable restore
#line 22 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml"
           Write(radar.location);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        </tr>\r\n");
#nullable restore
#line 24 "C:\Users\Cenk\OneDrive\Desktop\Test2\ASPNET-CORE-RADAR-main\Views\AdminRadarList\RadarList.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</table>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ASPNETAOP.Models.AdminRadarList>> Html { get; private set; }
    }
}
#pragma warning restore 1591
