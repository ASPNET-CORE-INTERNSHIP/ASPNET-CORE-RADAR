#pragma checksum "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "81a08a4c17e2ac47aa1d3dabda1113d17de07c34"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_AntennaScan_Preliminary), @"mvc.1.0.view", @"/Views/AntennaScan/Preliminary.cshtml")]
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
#line 1 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\_ViewImports.cshtml"
using ASPNETAOP;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\_ViewImports.cshtml"
using ASPNETAOP.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"81a08a4c17e2ac47aa1d3dabda1113d17de07c34", @"/Views/AntennaScan/Preliminary.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8114726577ba02b0c53af550574d2018d71987ed", @"/Views/_ViewImports.cshtml")]
    public class Views_AntennaScan_Preliminary : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<ASPNETAOP.Models.Antenna.AntennaList>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "NewAntennaScanParam", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
  
    ViewData["Title"] = "NewAntennaScan";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Build Relationship</h1>\r\n\r\n<hr />\r\n<div class=\"row\">\r\n    <div class=\"col-md-9\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "81a08a4c17e2ac47aa1d3dabda1113d17de07c344443", async() => {
                WriteLiteral("\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("div", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "81a08a4c17e2ac47aa1d3dabda1113d17de07c344713", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationSummaryTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper);
#nullable restore
#line 13 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary = global::Microsoft.AspNetCore.Mvc.Rendering.ValidationSummary.ModelOnly;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-summary", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationSummaryTagHelper.ValidationSummary, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
            <table class=""table"">
                <tr>
                    <th>Name</th>
                    <th>Type</th>
                    <th>Horizontal Beamwidth</th>
                    <th>Vertical Beamwidth</th>
                    <th>Polarization</th>
                    <th>Number of Feed</th>
                    <th>Horizontal Dimension</th>
                    <th>Vertical Dimension</th>
                    <th>Location</th>
                    <th>Select</th>
                </tr>
");
#nullable restore
#line 27 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                 for (int i = 0; i < Model.antennas.Count; i++)
                {
                    var ID = "antennas[" + i + "].ID";
                    var name = "antennas[" + i + "].name";
                    var type = "antennas[" + i + "].type";
                    var horizontal_beamwidth = "antennas[" + i + "].horizontal_beamwidth";
                    var vertical_beamwidth = "antennas[" + i + "].vertical_beamwidth";
                    var polarization = "antennas[" + i + "].polarization";
                    var number_of_feed = "antennas[" + i + "].number_of_feed";
                    var horizontal_dimension = "antennas[" + i + "].horizontal_dimension";
                    var vertical_dimension = "antennas[" + i + "].vertical_dimension";
                    var location = "antennas[" + i + "].location";

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 1712, "\"", 1720, 1);
#nullable restore
#line 39 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1717, ID, 1717, 3, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 1721, "\"", 1731, 1);
#nullable restore
#line 39 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1728, ID, 1728, 3, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 1732, "\"", 1761, 1);
#nullable restore
#line 39 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1740, Model.antennas[i].ID, 1740, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 1807, "\"", 1817, 1);
#nullable restore
#line 40 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1812, name, 1812, 5, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 1818, "\"", 1830, 1);
#nullable restore
#line 40 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1825, name, 1825, 5, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 1831, "\"", 1862, 1);
#nullable restore
#line 40 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1839, Model.antennas[i].name, 1839, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 1908, "\"", 1918, 1);
#nullable restore
#line 41 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1913, type, 1913, 5, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 1919, "\"", 1931, 1);
#nullable restore
#line 41 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1926, type, 1926, 5, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 1932, "\"", 1963, 1);
#nullable restore
#line 41 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 1940, Model.antennas[i].type, 1940, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 2009, "\"", 2035, 1);
#nullable restore
#line 42 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2014, horizontal_beamwidth, 2014, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 2036, "\"", 2064, 1);
#nullable restore
#line 42 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2043, horizontal_beamwidth, 2043, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2065, "\"", 2112, 1);
#nullable restore
#line 42 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2073, Model.antennas[i].horizontal_beamwidth, 2073, 39, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 2158, "\"", 2182, 1);
#nullable restore
#line 43 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2163, vertical_beamwidth, 2163, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 2183, "\"", 2209, 1);
#nullable restore
#line 43 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2190, vertical_beamwidth, 2190, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2210, "\"", 2255, 1);
#nullable restore
#line 43 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2218, Model.antennas[i].vertical_beamwidth, 2218, 37, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 2301, "\"", 2319, 1);
#nullable restore
#line 44 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2306, polarization, 2306, 13, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 2320, "\"", 2340, 1);
#nullable restore
#line 44 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2327, polarization, 2327, 13, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2341, "\"", 2380, 1);
#nullable restore
#line 44 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2349, Model.antennas[i].polarization, 2349, 31, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 2426, "\"", 2446, 1);
#nullable restore
#line 45 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2431, number_of_feed, 2431, 15, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 2447, "\"", 2469, 1);
#nullable restore
#line 45 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2454, number_of_feed, 2454, 15, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2470, "\"", 2511, 1);
#nullable restore
#line 45 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2478, Model.antennas[i].number_of_feed, 2478, 33, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 2557, "\"", 2583, 1);
#nullable restore
#line 46 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2562, horizontal_dimension, 2562, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 2584, "\"", 2612, 1);
#nullable restore
#line 46 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2591, horizontal_dimension, 2591, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2613, "\"", 2660, 1);
#nullable restore
#line 46 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2621, Model.antennas[i].horizontal_dimension, 2621, 39, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 2706, "\"", 2730, 1);
#nullable restore
#line 47 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2711, vertical_dimension, 2711, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 2731, "\"", 2757, 1);
#nullable restore
#line 47 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2738, vertical_dimension, 2738, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2758, "\"", 2803, 1);
#nullable restore
#line 47 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2766, Model.antennas[i].vertical_dimension, 2766, 37, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                    <input type=\"hidden\"");
                BeginWriteAttribute("id", " id=\"", 2849, "\"", 2863, 1);
#nullable restore
#line 48 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2854, location, 2854, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("name", " name=\"", 2864, "\"", 2880, 1);
#nullable restore
#line 48 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2871, location, 2871, 9, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 2881, "\"", 2916, 1);
#nullable restore
#line 48 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 2889, Model.antennas[i].location, 2889, 27, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n");
                WriteLiteral("                    <tr>\r\n                        <td>");
#nullable restore
#line 51 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].name);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 52 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].type);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 53 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].horizontal_beamwidth);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 54 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].vertical_beamwidth);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 55 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].polarization);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 56 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].number_of_feed);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 57 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].horizontal_dimension);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 58 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].vertical_dimension);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 59 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Model.antennas[i].location);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 60 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                       Write(Html.CheckBoxFor(m => Model.antennas[i].IsChecked));

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    </tr>\r\n");
#nullable restore
#line 62 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("            </table>\r\n            <div class=\"form-group\">\r\n                <input type=\"submit\" value=\"Add\" class=\"btn btn-primary\" />\r\n            </div>\r\n            <b>");
#nullable restore
#line 67 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
          Write(ViewData["message"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</b>\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n");
#nullable restore
#line 71 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
  
    if (Model.ComeFromAdd)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div>\r\n            <input type=\"button\" style=\"background-color:lawngreen; color:white; width:150px; height:40px;\" title=\"Done\" value=\"DONE!\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4109, "\"", 4171, 3);
            WriteAttributeValue("", 4119, "location.href=\'", 4119, 15, true);
#nullable restore
#line 75 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 4134, Url.Action("GoBack", "AntennaScan"), 4134, 36, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4170, "\'", 4170, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n        </div>\r\n");
#nullable restore
#line 77 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <table>\r\n            <tr>\r\n                <input type=\"button\" style=\"background-color:mediumblue; color:white; width:150px; height:40px;\" title=\"AddSubmode\" value=\"Add Submode\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4403, "\"", 4470, 3);
            WriteAttributeValue("", 4413, "location.href=\'", 4413, 15, true);
#nullable restore
#line 82 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 4428, Url.Action("GoToSubmode", "AntennaScan"), 4428, 41, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4469, "\'", 4469, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n                &nbsp;\r\n                <input type=\"button\" style=\"background-color:darkblue; color:white; width:150px; height:40px;\" title=\"AddMode\" value=\"Add Mode\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4643, "\"", 4707, 3);
            WriteAttributeValue("", 4653, "location.href=\'", 4653, 15, true);
#nullable restore
#line 84 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 4668, Url.Action("GoToMode", "AntennaScan"), 4668, 38, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4706, "\'", 4706, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n                &nbsp;\r\n                <input type=\"button\" style=\"background-color:orangered; color:white; width:150px; height:40px;\" title=\"Done\" value=\"DONE!\"");
            BeginWriteAttribute("onclick", " onclick=\"", 4875, "\"", 4935, 3);
            WriteAttributeValue("", 4885, "location.href=\'", 4885, 15, true);
#nullable restore
#line 86 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
WriteAttributeValue("", 4900, Url.Action("Done", "AntennaScan"), 4900, 34, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 4934, "\'", 4934, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n            </tr>\r\n        </table>\r\n");
#nullable restore
#line 89 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
    }

#line default
#line hidden
#nullable disable
            DefineSection("Scripts", async() => {
                WriteLiteral("\r\n");
#nullable restore
#line 92 "C:\Users\ENA\source\repos\ASPNET-CORE-RADAR\Views\AntennaScan\Preliminary.cshtml"
      await Html.RenderPartialAsync("_ValidationScriptsPartial");

#line default
#line hidden
#nullable disable
            }
            );
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ASPNETAOP.Models.Antenna.AntennaList> Html { get; private set; }
    }
}
#pragma warning restore 1591
