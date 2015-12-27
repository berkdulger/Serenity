﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Serenity.CodeGenerator.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    public partial class EntityRow : RazorGenerator.Templating.RazorTemplateBase
    {
#line hidden
 public dynamic Model { get; set; } 
        public override void Execute()
        {


WriteLiteral("\r\n");



                                                   
    var dotModule = Model.Module == null ? "" : ("." + Model.Module);
    var moduleDot = Model.Module == null ? "" : (Model.Module + ".");
    var schemaDot = Model.Schema == null ? "" : ("[" + Model.Schema + "].");
     
    Func<string, string, string> jf = (x, y) =>
    {
        if (x.ToLowerInvariant() == y.ToLowerInvariant())
            return y;
        else
            return x + y;
    };


WriteLiteral("namespace ");


      Write(Model.RootNamespace);


                            Write(dotModule);

WriteLiteral(@".Entities
{
    using Newtonsoft.Json;
    using Serenity;
    using Serenity.ComponentModel;
    using Serenity.Data;
    using Serenity.Data.Mapping;
    using System;
    using System.ComponentModel;
    using System.IO;

    [ConnectionKey(""");


               Write(Model.ConnectionKey);

WriteLiteral("\"), DisplayName(\"");


                                                    Write(Model.Tablename);

WriteLiteral("\"), InstanceName(\"");


                                                                                      Write(Model.Tablename);

WriteLiteral("\"), TwoLevelCached]\r\n    [ReadPermission(\"");


                Write(Model.Permission);

WriteLiteral("\")]\r\n    [ModifyPermission(\"");


                  Write(Model.Permission);

WriteLiteral("\")]\r\n    public sealed class ");


                   Write(Model.RowClassName);

WriteLiteral(" : ");


                                         Write(Model.RowBaseClass);

WriteLiteral(", IIdRow");


                                                                     Write(Model.IsLookup ? ", IDbLookupRow" : "");


                                                                                                              Write(Model.NameField == null ? "" : ", INameRow");


                                                                                                                                                                WriteLiteral("\r\n    {");

      foreach (var x in Model.Fields) {
    var attrs = new List<string>();
    attrs.Add("DisplayName(\"" + x.Title + "\")");

    if (x.Ident != x.Name)
    {
        attrs.Add("Column(\"" + x.Name + "\")");
    }

    if ((x.Size ?? 0) != 0) {
        attrs.Add("Size(" + x.Size + ")");
    }
    if (x.Scale != 0) {
        attrs.Add("Scale(" + x.Scale + ")");
    }
    if (!String.IsNullOrEmpty(x.Flags)) {
        attrs.Add(x.Flags);
    }       
    if (!String.IsNullOrEmpty(x.PKTable)) {
        attrs.Add("ForeignKey(\"" + (string.IsNullOrEmpty(x.PKSchema) ? x.PKTable : ("[" + x.PKSchema + "].[" + x.PKTable + "]")) + "\", \"" + x.PKColumn + "\")");
        attrs.Add("LeftJoin(\"j" + x.ForeignJoinAlias + "\")");
    }
    if (Model.NameField == x.Ident) {
        attrs.Add("QuickSearch");
    }
    if (x.TextualField != null) {
        attrs.Add("TextualField(\"" + x.TextualField + "\")");
    }
    var attrString = String.Join(", ", attrs.ToArray());

WriteLiteral("\r\n");


 if (!String.IsNullOrEmpty(attrString)) {

WriteLiteral("        [");


          Write(attrString);

WriteLiteral("]\r\n");


       }
WriteLiteral("        public ");


                  Write(x.Type);


                          Write(x.IsValueType ? "?" : "");

WriteLiteral(" ");


                                                     Write(x.Ident);

WriteLiteral("\r\n        {\r\n            get { return Fields.");


                            Write(x.Ident);

WriteLiteral("[this]; }\r\n            set { Fields.");


                     Write(x.Ident);

WriteLiteral("[this] = value; }\r\n        }\r\n");


       }


 foreach (var x in Model.Joins){foreach (var y in x.Fields){

WriteLiteral("\r\n        [DisplayName(\"");


                 Write(y.Title);

WriteLiteral("\"), Expression(\"");


                                          Write("j" + x.Name + ".[" + y.Name + "]");

WriteLiteral("\")]\r\n        public ");


          Write(y.Type);


                  Write(y.IsValueType ? "?" : "");

WriteLiteral(" ");


                                              Write(jf(x.Name, y.Ident));

WriteLiteral("\r\n        {\r\n            get { return Fields.");


                            Write(jf(x.Name, y.Ident));

WriteLiteral("[this]; }\r\n            set { Fields.");


                     Write(jf(x.Name, y.Ident));

WriteLiteral("[this] = value; }\r\n        }\r\n");


       }}

WriteLiteral("\r\n        IIdField IIdRow.IdField\r\n        {\r\n            get { return Fields.");


                            Write(Model.Identity);

WriteLiteral("; }\r\n        }\r\n");


 if (Model.NameField != null) {

WriteLiteral("\r\n        StringField INameRow.NameField\r\n        {\r\n            get { return Fie" +
"lds.");


                           Write(Model.NameField);

WriteLiteral("; }\r\n        }\r\n");


       }

WriteLiteral("\r\n        public static readonly RowFields Fields = new RowFields().Init();\r\n\r\n  " +
"      public ");


           Write(Model.RowClassName);

WriteLiteral("()\r\n            : base(Fields)\r\n        {\r\n        }\r\n\r\n        public class RowF" +
"ields : ");


                             Write(Model.FieldsBaseClass);


                                                         WriteLiteral("\r\n        {");

          foreach (var x in Model.Fields) {

WriteLiteral("\r\n            public readonly ");


                        Write(x.Type);

WriteLiteral("Field ");


                                       Write(x.Ident);

WriteLiteral(";");


                                                             }


 foreach (var x in Model.Joins) {
WriteLiteral("\r\n");


       foreach (var y in x.Fields) {

WriteLiteral("\r\n            public readonly ");


                        Write(y.Type);

WriteLiteral("Field ");


                                       Write(jf(x.Name, y.Ident));

WriteLiteral(";");


                                                                         }}

WriteLiteral("\r\n\r\n            public RowFields()\r\n                : base(\"");


                    Write(String.IsNullOrEmpty(schemaDot) ? Model.Tablename : schemaDot + "[" + Model.Tablename + "]");

WriteLiteral("\"");


                                                                                                                   Write(string.IsNullOrEmpty(Model.FieldPrefix) ? "" : (", \"" + Model.FieldPrefix + "\""));

WriteLiteral(")\r\n            {\r\n                LocalTextPrefix = \"");


                               Write(moduleDot);


                                           Write(Model.ClassName);

WriteLiteral("\";\r\n            }\r\n        }\r\n    }\r\n}");


        }
    }
}
#pragma warning restore 1591
