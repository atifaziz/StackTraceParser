#region Copyright (c) 2011 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

// ReSharper disable once CheckNamespace

namespace Tests
{
    #region Imports

    using System;
    using System.Linq;
    using NUnit.Framework;
    using StackTraceParser = Elmah.StackTraceParser;

    #endregion

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    class StackTraceTestCase : TestCaseAttribute
    {
        public StackTraceTestCase(string stackTrace, int index, string frame) :
            base(stackTrace, index, frame) {}

        public StackTraceTestCase(string stackTrace, int index,
                 string frame,
                 string type, string method, string parameterList, string parameters,
                 string file, string line) :
            base(stackTrace, index, frame, type, method, parameterList, parameters, file, line) {}
    }

    [TestFixture]
    public sealed class StackTraceParserTests
    {
        [Test]
        public void ParseWithSimpleOverloadFailsWithNullSelector()
        {
            var e = Assert.Throws<ArgumentNullException>(() => StackTraceParser.Parse<object>(string.Empty, null));
            Assert.That(e.ParamName, Is.EqualTo("selector"));
        }

        [Test]
        public void ParseFailsWithNullTokenSelector()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                StackTraceParser.Parse<object, object, object, object, object, object>(
                    string.Empty,
                    null,
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; }));
            Assert.That(e.ParamName, Is.EqualTo("tokenSelector"));
        }

        [Test]
        public void ParseFailsWithNullMethodSelector()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                StackTraceParser.Parse<object, object, object, object, object, object>(
                    string.Empty,
                    delegate { return null; },
                    null,
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; }));
            Assert.That(e.ParamName, Is.EqualTo("methodSelector"));
        }

        [Test]
        public void ParseFailsWithNullParameterSelector()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                StackTraceParser.Parse<object, object, object, object, object, object>(
                    string.Empty,
                    delegate { return null; },
                    delegate { return null; },
                    null,
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; }));
            Assert.That(e.ParamName, Is.EqualTo("parameterSelector"));
        }

        [Test]
        public void ParseFailsWithNullParametersSelector()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                StackTraceParser.Parse<object, object, object, object, object, object>(
                    string.Empty,
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    null,
                    delegate { return null; },
                    delegate { return null; }));
            Assert.That(e.ParamName, Is.EqualTo("parametersSelector"));
        }

        [Test]
        public void ParseFailsWithNullSourceLocationSelector()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                StackTraceParser.Parse<object, object, object, object, object, object>(
                    string.Empty,
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    null,
                    delegate { return null; }));
            Assert.That(e.ParamName, Is.EqualTo("sourceLocationSelector"));
        }

        [Test]
        public void ParseFailsWithNullSelector()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                StackTraceParser.Parse<object, object, object, object, object, object>(
                    string.Empty,
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    delegate { return null; },
                    null));
            Assert.That(e.ParamName, Is.EqualTo("selector"));
        }

        const string DotNetStackTrace = @"
            Elmah.TestException: This is a test exception that can be safely ignored.
                at Elmah.ErrorLogPageFactory.FindHandler(String name) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 126
                at Elmah.ErrorLogPageFactory.GetHandler(HttpContext context, String requestType, String url, String pathTranslated) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 66
                at System.Web.HttpApplication.MapHttpHandler(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig)
                at System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()
                at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)";

        [StackTraceTestCase(DotNetStackTrace, 0,
            /* Frame         */ @"Elmah.ErrorLogPageFactory.FindHandler(String name) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 126",
            /* Type          */ @"Elmah.ErrorLogPageFactory",
            /* Method        */ @"FindHandler",
            /* ParameterList */ @"(String name)",
            /* Parameters    */ @"String name",
            /* File          */ @"C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs",
            /* Line          */ @"126")]
        [StackTraceTestCase(DotNetStackTrace, 1,
            /* Frame         */ @"Elmah.ErrorLogPageFactory.GetHandler(HttpContext context, String requestType, String url, String pathTranslated) in C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs:line 66",
            /* Type          */ @"Elmah.ErrorLogPageFactory",
            /* Method        */ @"GetHandler",
            /* ParameterList */ @"(HttpContext context, String requestType, String url, String pathTranslated)",
            /* Parameters    */ @"HttpContext context, String requestType, String url, String pathTranslated",
            /* File          */ @"C:\ELMAH\src\Elmah\ErrorLogPageFactory.cs",
            /* Line          */ @"66")]
        [StackTraceTestCase(DotNetStackTrace, 2,
            /* Frame         */ @"System.Web.HttpApplication.MapHttpHandler(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig)",
            /* Type          */ @"System.Web.HttpApplication",
            /* Method        */ @"MapHttpHandler",
            /* ParameterList */ @"(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig)",
            /* Parameters    */ @"HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig",
            /* File          */ "",
            /* Line          */ "")]
        [StackTraceTestCase(DotNetStackTrace, 3,
            /* Frame         */  @"System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()",
            /* Type          */  @"System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep",
            /* Method        */  @"Execute",
            /* ParameterList */  @"()",
            /* Parameters    */  "",
            /* File          */  "",
            /* Line          */  "")]
        [StackTraceTestCase(DotNetStackTrace, 4,
            /* Frame         */ @"System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)",
            /* Type          */ @"System.Web.HttpApplication",
            /* Method        */ @"ExecuteStep",
            /* ParameterList */ @"(IExecutionStep step, Boolean& completedSynchronously)",
            /* Parameters    */ @"IExecutionStep step, Boolean& completedSynchronously",
            /* File          */ "",
            /* Line          */ "")]

        public void ParseDotNetStackTrace(string stackTrace, int index,
            string frame,
            string type, string method, string parameterList, string parameters,
            string file, string line)
        {
            Parse(stackTrace, index, frame, type, method, parameterList, parameters, file, line);
        }

        const string DotNetStackLinuxTrace = @"
            Elmah.TestException: This is a test exception that can be safely ignored.
                at Elmah.ErrorLogPageFactory.FindHandler(String name) in /ELMAH/src/Elmah/ErrorLogPageFactory.cs:line 126
                at Elmah.ErrorLogPageFactory.GetHandler(HttpContext context, String requestType, String url, String pathTranslated) in /ELMAH/src/Elmah/ErrorLogPageFactory.cs:line 66
                at System.Web.HttpApplication.MapHttpHandler(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig)
                at System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()
                at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)";

        [StackTraceTestCase(DotNetStackLinuxTrace, 0,
            /* Frame         */ @"Elmah.ErrorLogPageFactory.FindHandler(String name) in /ELMAH/src/Elmah/ErrorLogPageFactory.cs:line 126",
            /* Type          */ @"Elmah.ErrorLogPageFactory",
            /* Method        */ @"FindHandler",
            /* ParameterList */ @"(String name)",
            /* Parameters    */ @"String name",
            /* File          */ @"/ELMAH/src/Elmah/ErrorLogPageFactory.cs",
            /* Line          */ @"126")]
        [StackTraceTestCase(DotNetStackLinuxTrace, 1,
            /* Frame         */ @"Elmah.ErrorLogPageFactory.GetHandler(HttpContext context, String requestType, String url, String pathTranslated) in /ELMAH/src/Elmah/ErrorLogPageFactory.cs:line 66",
            /* Type          */ @"Elmah.ErrorLogPageFactory",
            /* Method        */ @"GetHandler",
            /* ParameterList */ @"(HttpContext context, String requestType, String url, String pathTranslated)",
            /* Parameters    */ @"HttpContext context, String requestType, String url, String pathTranslated",
            /* File          */ @"/ELMAH/src/Elmah/ErrorLogPageFactory.cs",
            /* Line          */ @"66")]
        [StackTraceTestCase(DotNetStackLinuxTrace, 2,
            /* Frame         */ @"System.Web.HttpApplication.MapHttpHandler(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig)",
            /* Type          */ @"System.Web.HttpApplication",
            /* Method        */ @"MapHttpHandler",
            /* ParameterList */ @"(HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig)",
            /* Parameters    */ @"HttpContext context, String requestType, VirtualPath path, String pathTranslated, Boolean useAppConfig",
            /* File          */ "",
            /* Line          */ "")]
        [StackTraceTestCase(DotNetStackLinuxTrace, 3,
            /* Frame         */  @"System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()",
            /* Type          */  @"System.Web.HttpApplication.MapHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep",
            /* Method        */  @"Execute",
            /* ParameterList */  @"()",
            /* Parameters    */  "",
            /* File          */  "",
            /* Line          */  "")]
        [StackTraceTestCase(DotNetStackLinuxTrace, 4,
            /* Frame         */ @"System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)",
            /* Type          */ @"System.Web.HttpApplication",
            /* Method        */ @"ExecuteStep",
            /* ParameterList */ @"(IExecutionStep step, Boolean& completedSynchronously)",
            /* Parameters    */ @"IExecutionStep step, Boolean& completedSynchronously",
            /* File          */ "",
            /* Line          */ "")]

        public void ParseDotNetLinuxStackTrace(string stackTrace, int index,
                                          string frame,
                                          string type, string method, string parameterList, string parameters,
                                          string file, string line)
        {
            Parse(stackTrace, index, frame, type, method, parameterList, parameters, file, line);
        }

        // See https://github.com/elmah/Elmah/issues/320

        const string MonoStackTrace = @"
            System.Web.HttpException: The controller for path '/helloworld' was not found or does not implement IController.
                at System.Web.Mvc.DefaultControllerFactory.GetControllerInstance (System.Web.Routing.RequestContext requestContext, System.Type controllerType) [0x00000] in <filename unknown>:0
                at System.Web.Mvc.DefaultControllerFactory.CreateController (System.Web.Routing.RequestContext requestContext, System.String controllerName) [0x00000] in <filename unknown>:0
                at System.Web.Mvc.MvcHandler.ProcessRequestInit (System.Web.HttpContextBase httpContext, IController& controller, IControllerFactory& factory) [0x00000] in <filename unknown>:0
                at System.Web.Mvc.MvcHandler.BeginProcessRequest (System.Web.HttpContextBase httpContext, System.AsyncCallback callback, System.Object state) [0x00000] in <filename unknown>:0
                at System.Web.Mvc.MvcHandler.BeginProcessRequest (System.Web.HttpContext httpContext, System.AsyncCallback callback, System.Object state) [0x00000] in <filename unknown>:0
                at System.Web.Mvc.MvcHandler.System.Web.IHttpAsyncHandler.BeginProcessRequest (System.Web.HttpContext context, System.AsyncCallback cb, System.Object extraData) [0x00000] in <filename unknown>:0
                at System.Web.HttpApplication+<Pipeline>c__Iterator3.MoveNext () [0x00000] in <filename unknown>:0";
        const string Zero = "0";
        const string FilenameUnknown = "filename unknown";

        [StackTraceTestCase(MonoStackTrace, 0,
            /* Frame          */ "System.Web.Mvc.DefaultControllerFactory.GetControllerInstance (System.Web.Routing.RequestContext requestContext, System.Type controllerType) [0x00000] in <filename unknown>:0",
            /* Type           */ "System.Web.Mvc.DefaultControllerFactory",
            /* Method         */ "GetControllerInstance",
            /* ParameterList  */ "(System.Web.Routing.RequestContext requestContext, System.Type controllerType)",
            /* Parameters     */ "System.Web.Routing.RequestContext requestContext, System.Type controllerType",
            FilenameUnknown, Zero)]
        [StackTraceTestCase(MonoStackTrace, 1,
            /* Frame          */ "System.Web.Mvc.DefaultControllerFactory.CreateController (System.Web.Routing.RequestContext requestContext, System.String controllerName) [0x00000] in <filename unknown>:0",
            /* Type           */ "System.Web.Mvc.DefaultControllerFactory",
            /* Method         */ "CreateController",
            /* ParameterList  */ "(System.Web.Routing.RequestContext requestContext, System.String controllerName)",
            /* Parameters     */ "System.Web.Routing.RequestContext requestContext, System.String controllerName",
            FilenameUnknown, Zero)]
        [StackTraceTestCase(MonoStackTrace, 2,
            /* Frame          */ "System.Web.Mvc.MvcHandler.ProcessRequestInit (System.Web.HttpContextBase httpContext, IController& controller, IControllerFactory& factory) [0x00000] in <filename unknown>:0",
            /* Type           */ "System.Web.Mvc.MvcHandler",
            /* Method         */ "ProcessRequestInit",
            /* ParameterList  */ "(System.Web.HttpContextBase httpContext, IController& controller, IControllerFactory& factory)",
            /* Parameters     */ "System.Web.HttpContextBase httpContext, IController& controller, IControllerFactory& factory",
            FilenameUnknown, Zero)]
        [StackTraceTestCase(MonoStackTrace, 3,
            /* Frame          */ "System.Web.Mvc.MvcHandler.BeginProcessRequest (System.Web.HttpContextBase httpContext, System.AsyncCallback callback, System.Object state) [0x00000] in <filename unknown>:0",
            /* Type           */ "System.Web.Mvc.MvcHandler",
            /* Method         */ "BeginProcessRequest",
            /* ParameterList  */ "(System.Web.HttpContextBase httpContext, System.AsyncCallback callback, System.Object state)",
            /* Parameters     */ "System.Web.HttpContextBase httpContext, System.AsyncCallback callback, System.Object state",
            FilenameUnknown, Zero)]
        [StackTraceTestCase(MonoStackTrace, 4,
            /* Frame          */ "System.Web.Mvc.MvcHandler.BeginProcessRequest (System.Web.HttpContext httpContext, System.AsyncCallback callback, System.Object state) [0x00000] in <filename unknown>:0",
            /* Type           */ "System.Web.Mvc.MvcHandler",
            /* Method         */ "BeginProcessRequest",
            /* ParameterList  */ "(System.Web.HttpContext httpContext, System.AsyncCallback callback, System.Object state)",
            /* Parameters     */ "System.Web.HttpContext httpContext, System.AsyncCallback callback, System.Object state",
            FilenameUnknown, Zero)]
        [StackTraceTestCase(MonoStackTrace, 5,
            /* Frame          */ "System.Web.Mvc.MvcHandler.System.Web.IHttpAsyncHandler.BeginProcessRequest (System.Web.HttpContext context, System.AsyncCallback cb, System.Object extraData) [0x00000] in <filename unknown>:0",
            /* Type           */ "System.Web.Mvc.MvcHandler.System.Web.IHttpAsyncHandler",
            /* Method         */ "BeginProcessRequest",
            /* ParameterList  */ "(System.Web.HttpContext context, System.AsyncCallback cb, System.Object extraData)",
            /* Parameters     */ "System.Web.HttpContext context, System.AsyncCallback cb, System.Object extraData",
            FilenameUnknown, Zero)]
        [StackTraceTestCase(MonoStackTrace, 6,
            /* Frame          */ "System.Web.HttpApplication+<Pipeline>c__Iterator3.MoveNext () [0x00000] in <filename unknown>:0",
            /* Type           */ "System.Web.HttpApplication+<Pipeline>c__Iterator3",
            /* Method         */ "MoveNext",
            /* ParameterList  */ "()",
            /* Parameters     */ "",
            FilenameUnknown, Zero)]

        public void ParseMonoStackTrace(string stackTrace, int index,
            string frame,
            string type, string method, string parameterList, string parameters,
            string file, string line)
        {
            Parse(stackTrace, index, frame, type, method, parameterList, parameters, file, line);
        }

        // Tests bug reported in issue #2:
        // https://github.com/atifaziz/StackTraceParser/issues/2

        const string SpaceBugStackTrace = @"
            System.Web.HttpUnhandledException (0x80004005): Exception of type 'System.Web.HttpUnhandledException' was thrown. ---> System.ArgumentException: Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).
            Parameter name: 9f567029-a6c4-4232-bab0-177ab8d5a67x ---> System.FormatException: Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).
               at System.Guid.GuidResult.SetFailure(ParseFailureKind failure, String failureMessageID, Object failureMessageFormatArgument, String failureArgumentName, Exception innerException)
               at System.Guid.TryParseGuidWithDashes(String guidString, GuidResult& result)
               at System.Guid.TryParseGuid(String g, GuidStyles flags, GuidResult& result)
               at System.Guid..ctor(String g)
               at Elmah.XmlFileErrorLog.GetError(String id)
               --- End of inner exception stack trace ---
               at Elmah.XmlFileErrorLog.GetError(String id)
               at Elmah.ErrorDetailPage.OnLoad(EventArgs e)
               at System.Web.UI.Control.LoadRecursive()
               at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)
               at System.Web.UI.Page.HandleError(Exception e)
               at System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)
               at System.Web.UI.Page.ProcessRequest(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)
               at System.Web.UI.Page.ProcessRequest()
               at System.Web.UI.Page.ProcessRequestWithNoAssert(HttpContext context)
               at System.Web.UI.Page.ProcessRequest(HttpContext context)
               at System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()
               at System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)";

        [StackTraceTestCase(SpaceBugStackTrace, 00, @"System.Guid.GuidResult.SetFailure(ParseFailureKind failure, String failureMessageID, Object failureMessageFormatArgument, String failureArgumentName, Exception innerException)")]
        [StackTraceTestCase(SpaceBugStackTrace, 01, @"System.Guid.TryParseGuidWithDashes(String guidString, GuidResult& result)")]
        [StackTraceTestCase(SpaceBugStackTrace, 02, @"System.Guid.TryParseGuid(String g, GuidStyles flags, GuidResult& result)")]
        [StackTraceTestCase(SpaceBugStackTrace, 03, @"System.Guid..ctor(String g)")]
        [StackTraceTestCase(SpaceBugStackTrace, 04, @"Elmah.XmlFileErrorLog.GetError(String id)")]
        [StackTraceTestCase(SpaceBugStackTrace, 05, @"Elmah.XmlFileErrorLog.GetError(String id)")]
        [StackTraceTestCase(SpaceBugStackTrace, 06, @"Elmah.ErrorDetailPage.OnLoad(EventArgs e)")]
        [StackTraceTestCase(SpaceBugStackTrace, 07, @"System.Web.UI.Control.LoadRecursive()")]
        [StackTraceTestCase(SpaceBugStackTrace, 08, @"System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)")]
        [StackTraceTestCase(SpaceBugStackTrace, 09, @"System.Web.UI.Page.HandleError(Exception e)")]
        [StackTraceTestCase(SpaceBugStackTrace, 10, @"System.Web.UI.Page.ProcessRequestMain(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)")]
        [StackTraceTestCase(SpaceBugStackTrace, 11, @"System.Web.UI.Page.ProcessRequest(Boolean includeStagesBeforeAsyncPoint, Boolean includeStagesAfterAsyncPoint)")]
        [StackTraceTestCase(SpaceBugStackTrace, 12, @"System.Web.UI.Page.ProcessRequest()")]
        [StackTraceTestCase(SpaceBugStackTrace, 13, @"System.Web.UI.Page.ProcessRequestWithNoAssert(HttpContext context)")]
        [StackTraceTestCase(SpaceBugStackTrace, 14, @"System.Web.UI.Page.ProcessRequest(HttpContext context)")]
        [StackTraceTestCase(SpaceBugStackTrace, 15, @"System.Web.HttpApplication.CallHandlerExecutionStep.System.Web.HttpApplication.IExecutionStep.Execute()")]
        [StackTraceTestCase(SpaceBugStackTrace, 16, @"System.Web.HttpApplication.ExecuteStep(IExecutionStep step, Boolean& completedSynchronously)")]
        public void ParseSpaceBugStackTrace(string stackTrace, int index, string frame)
        {
            Parse(stackTrace, index, frame);
        }

        // Tests bug reported in issue #4:
        // https://github.com/atifaziz/StackTraceParser/issues/4

        const string RunawayBugStackTrace = @"
            NHibernate.Exceptions.GenericADOException: Failed to execute multi query[SQL: select codingques0_.Id as Id2_0_, choicename1_.TextID as TextID130_1_, choicename1_.LanguageID as LanguageID130_1_, codingques0_.VersionStamp as VersionS2_2_0_, codingques0_.CreatedBy as CreatedBy2_0_, codingques0_.CreatedDateTime as CreatedD4_2_0_, codingques0_.LastUpdateBy as LastUpda5_2_0_, codingques0_.LastUpdateDateTime as LastUpda6_2_0_, codingques0_.CodingQuestionID as CodingQu7_2_0_, codingques0_.TreeID as TreeID2_0_, codingques0_.CodingSpec as CodingSpec2_0_, codingques0_.ClassificationKeywordID as Classif10_2_0_, codingques0_.ChoiceTextLTID as ChoiceT11_2_0_, codingques0_.PositionIndex as Positio12_2_0_, codingques0_.CodingValue as CodingV13_2_0_, codingques0_.ChoiceNumber as ChoiceN14_2_0_, codingques0_.[TreeID].GetLevel() as formula1_0_, (select case when exists(select 1 from Survey.CodingQuestionChoice a where a.CodingQuestionID = codingques0_.CodingQuestionID and a.TreeID.GetAncestor(1) = codingques0_.TreeID) then 1 else 0 end) as formula2_0_, choicename1_.LocalizedText as Localize3_130_1_, choicename1_.TextID as TextID0__, choicename1_.LanguageID as LanguageID0__ from Survey.CodingQuestionChoice codingques0_ left outer join Administration.LocalizedTextSequenced choicename1_ on codingques0_.ChoiceTextLTID=choicename1_.TextID where codingques0_.CodingQuestionID=?;
            select codingques0_.Id as Id2_0_, codingques1_.ID as ID3_1_, codingques0_.VersionStamp as VersionS2_2_0_, codingques0_.CreatedBy as CreatedBy2_0_, codingques0_.CreatedDateTime as CreatedD4_2_0_, codingques0_.LastUpdateBy as LastUpda5_2_0_, codingques0_.LastUpdateDateTime as LastUpda6_2_0_, codingques0_.CodingQuestionID as CodingQu7_2_0_, codingques0_.TreeID as TreeID2_0_, codingques0_.CodingSpec as CodingSpec2_0_, codingques0_.ClassificationKeywordID as Classif10_2_0_, codingques0_.ChoiceTextLTID as ChoiceT11_2_0_, codingques0_.PositionIndex as Positio12_2_0_, codingques0_.CodingValue as CodingV13_2_0_, codingques0_.ChoiceNumber as ChoiceN14_2_0_, codingques0_.[TreeID].GetLevel() as formula1_0_, (select case when exists(select 1 from Survey.CodingQuestionChoice a where a.CodingQuestionID = codingques0_.CodingQuestionID and a.TreeID.GetAncestor(1) = codingques0_.TreeID) then 1 else 0 end) as formula2_0_, codingques1_.VersionStamp as VersionS2_3_1_, codingques1_.CodingQuestionChoiceID as CodingQu3_3_1_, codingques1_.StringPath as StringPath3_1_, codingques1_.StartCodingValue as StartCod5_3_1_, codingques1_.EndCodingValue as EndCodin6_3_1_, codingques1_.CodingSpec as CodingSpec3_1_, codingques1_.CreatedBy as CreatedBy3_1_, codingques1_.CreatedDateTime as CreatedD9_3_1_, codingques1_.LastUpdateBy as LastUpd10_3_1_, codingques1_.LastUpdateDateTime as LastUpd11_3_1_, codingques1_.CodingQuestionChoiceID as CodingQu3_0__, codingques1_.ID as ID0__ from Survey.CodingQuestionChoice codingques0_ left outer join Survey.CodingQuestionChoiceCodingRange codingques1_ on codingques0_.Id=codingques1_.CodingQuestionChoiceID where codingques0_.CodingQuestionID=?;
            ] ---> System.IO.FileLoadException: Could not load file or assembly 'Microsoft.SqlServer.Types, Version=10.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91' or one of its dependencies. The located assembly's manifest definition does not match the assembly reference. (Exception from HRESULT: 0x80131040)
               at System.Reflection.RuntimeAssembly._nLoad(AssemblyName fileName, String codeBase, Evidence assemblySecurity, RuntimeAssembly locationHint, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean throwOnFileNotFound, Boolean forIntrospection, Boolean suppressSecurityChecks)
               at System.Reflection.RuntimeAssembly.InternalLoadAssemblyName(AssemblyName assemblyRef, Evidence assemblySecurity, RuntimeAssembly reqAssembly, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean throwOnFileNotFound, Boolean forIntrospection, Boolean suppressSecurityChecks)
               at System.Reflection.Assembly.Load(AssemblyName assemblyRef)
               at System.Data.SqlClient.SqlConnection.ResolveTypeAssembly(AssemblyName asmRef, Boolean throwOnError)
               at System.TypeNameParser.ResolveAssembly(String asmName, Func`2 assemblyResolver, Boolean throwOnError, StackCrawlMark& stackMark)
               at System.TypeNameParser.ConstructType(Func`2 assemblyResolver, Func`4 typeResolver, Boolean throwOnError, Boolean ignoreCase, StackCrawlMark& stackMark)
               at System.TypeNameParser.GetType(String typeName, Func`2 assemblyResolver, Func`4 typeResolver, Boolean throwOnError, Boolean ignoreCase, StackCrawlMark& stackMark)
               at System.Type.GetType(String typeName, Func`2 assemblyResolver, Func`4 typeResolver, Boolean throwOnError)
               at System.Data.SqlClient.SqlConnection.CheckGetExtendedUDTInfo(SqlMetaDataPriv metaData, Boolean fThrow)
               at System.Data.SqlClient.SqlDataReader.GetValueFromSqlBufferInternal(SqlBuffer data, _SqlMetaData metaData)
               at System.Data.SqlClient.SqlDataReader.GetValue(Int32 i)
               at NHibernate.Type.AbstractStringType.Get(IDataReader rs, Int32 index)
               at NHibernate.Type.NullableType.NullSafeGet(IDataReader rs, String name)
               at NHibernate.Persister.Entity.AbstractEntityPersister.Hydrate(IDataReader rs, Object id, Object obj, ILoadable rootLoadable, String[][] suffixedPropertyColumns, Boolean allProperties, ISessionImplementor session)
               at NHibernate.Loader.Loader.LoadFromResultSet(IDataReader rs, Int32 i, Object obj, String instanceClass, EntityKey key, String rowIdAlias, LockMode lockMode, ILoadable rootPersister, ISessionImplementor session)
               at NHibernate.Loader.Loader.InstanceNotYetLoaded(IDataReader dr, Int32 i, ILoadable persister, EntityKey key, LockMode lockMode, String rowIdAlias, EntityKey optionalObjectKey, Object optionalObject, IList hydratedObjects, ISessionImplementor session)
               at NHibernate.Loader.Loader.GetRow(IDataReader rs, ILoadable[] persisters, EntityKey[] keys, Object optionalObject, EntityKey optionalObjectKey, LockMode[] lockModes, IList hydratedObjects, ISessionImplementor session)
               at NHibernate.Loader.Loader.GetRowFromResultSet(IDataReader resultSet, ISessionImplementor session, QueryParameters queryParameters, LockMode[] lockModeArray, EntityKey optionalObjectKey, IList hydratedObjects, EntityKey[] keys, Boolean returnProxies, IResultTransformer forcedResultTransformer)
               at NHibernate.Impl.MultiQueryImpl.DoList()";

        [StackTraceTestCase(RunawayBugStackTrace, 00, @"System.Reflection.RuntimeAssembly._nLoad(AssemblyName fileName, String codeBase, Evidence assemblySecurity, RuntimeAssembly locationHint, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean throwOnFileNotFound, Boolean forIntrospection, Boolean suppressSecurityChecks)")]
        [StackTraceTestCase(RunawayBugStackTrace, 01, @"System.Reflection.RuntimeAssembly.InternalLoadAssemblyName(AssemblyName assemblyRef, Evidence assemblySecurity, RuntimeAssembly reqAssembly, StackCrawlMark& stackMark, IntPtr pPrivHostBinder, Boolean throwOnFileNotFound, Boolean forIntrospection, Boolean suppressSecurityChecks)")]
        [StackTraceTestCase(RunawayBugStackTrace, 02, @"System.Reflection.Assembly.Load(AssemblyName assemblyRef)")]
        [StackTraceTestCase(RunawayBugStackTrace, 03, @"System.Data.SqlClient.SqlConnection.ResolveTypeAssembly(AssemblyName asmRef, Boolean throwOnError)")]
        [StackTraceTestCase(RunawayBugStackTrace, 04, @"System.TypeNameParser.ResolveAssembly(String asmName, Func`2 assemblyResolver, Boolean throwOnError, StackCrawlMark& stackMark)")]
        [StackTraceTestCase(RunawayBugStackTrace, 05, @"System.TypeNameParser.ConstructType(Func`2 assemblyResolver, Func`4 typeResolver, Boolean throwOnError, Boolean ignoreCase, StackCrawlMark& stackMark)")]
        [StackTraceTestCase(RunawayBugStackTrace, 06, @"System.TypeNameParser.GetType(String typeName, Func`2 assemblyResolver, Func`4 typeResolver, Boolean throwOnError, Boolean ignoreCase, StackCrawlMark& stackMark)")]
        [StackTraceTestCase(RunawayBugStackTrace, 07, @"System.Type.GetType(String typeName, Func`2 assemblyResolver, Func`4 typeResolver, Boolean throwOnError)")]
        [StackTraceTestCase(RunawayBugStackTrace, 08, @"System.Data.SqlClient.SqlConnection.CheckGetExtendedUDTInfo(SqlMetaDataPriv metaData, Boolean fThrow)")]
        [StackTraceTestCase(RunawayBugStackTrace, 09, @"System.Data.SqlClient.SqlDataReader.GetValueFromSqlBufferInternal(SqlBuffer data, _SqlMetaData metaData)")]
        [StackTraceTestCase(RunawayBugStackTrace, 10, @"System.Data.SqlClient.SqlDataReader.GetValue(Int32 i)")]
        [StackTraceTestCase(RunawayBugStackTrace, 11, @"NHibernate.Type.AbstractStringType.Get(IDataReader rs, Int32 index)")]
        [StackTraceTestCase(RunawayBugStackTrace, 12, @"NHibernate.Type.NullableType.NullSafeGet(IDataReader rs, String name)")]
        [StackTraceTestCase(RunawayBugStackTrace, 13, @"NHibernate.Persister.Entity.AbstractEntityPersister.Hydrate(IDataReader rs, Object id, Object obj, ILoadable rootLoadable, String[][] suffixedPropertyColumns, Boolean allProperties, ISessionImplementor session)")]
        [StackTraceTestCase(RunawayBugStackTrace, 14, @"NHibernate.Loader.Loader.LoadFromResultSet(IDataReader rs, Int32 i, Object obj, String instanceClass, EntityKey key, String rowIdAlias, LockMode lockMode, ILoadable rootPersister, ISessionImplementor session)")]
        [StackTraceTestCase(RunawayBugStackTrace, 15, @"NHibernate.Loader.Loader.InstanceNotYetLoaded(IDataReader dr, Int32 i, ILoadable persister, EntityKey key, LockMode lockMode, String rowIdAlias, EntityKey optionalObjectKey, Object optionalObject, IList hydratedObjects, ISessionImplementor session)")]
        [StackTraceTestCase(RunawayBugStackTrace, 16, @"NHibernate.Loader.Loader.GetRow(IDataReader rs, ILoadable[] persisters, EntityKey[] keys, Object optionalObject, EntityKey optionalObjectKey, LockMode[] lockModes, IList hydratedObjects, ISessionImplementor session)")]
        [StackTraceTestCase(RunawayBugStackTrace, 17, @"NHibernate.Loader.Loader.GetRowFromResultSet(IDataReader resultSet, ISessionImplementor session, QueryParameters queryParameters, LockMode[] lockModeArray, EntityKey optionalObjectKey, IList hydratedObjects, EntityKey[] keys, Boolean returnProxies, IResultTransformer forcedResultTransformer)")]
        [StackTraceTestCase(RunawayBugStackTrace, 18, @"NHibernate.Impl.MultiQueryImpl.DoList()")]
        public void ParseRunawayBugStackTrace(string stackTrace, int index, string frame)
        {
            Parse(stackTrace, index, frame);
        }

        static void Parse(string stackTrace, int index,
            string frame,
            string type, string method, string parameterList, string parameters,
            string file, string line)
        {
            var actuals = StackTraceParser.Parse(stackTrace,
                 (f, t, m, pl, ps, fn, ln) => new
                 {
                     Frame         = f,
                     Type          = t,
                     Method        = m,
                     ParameterList = pl,
                     Parameters    = string.Join(", ", from e in ps select e.Key + " " + e.Value),
                     File          = fn,
                     Line          = ln,
                 });

            var actual = actuals.ElementAt(index);

            Assert.That(frame        , Is.EqualTo(actual.Frame)        , "Frame");
            Assert.That(type         , Is.EqualTo(actual.Type)         , "Type");
            Assert.That(method       , Is.EqualTo(actual.Method)       , "Method");
            Assert.That(parameterList, Is.EqualTo(actual.ParameterList), "ParameterList");
            Assert.That(parameters   , Is.EqualTo(actual.Parameters)   , "Parameters");
            Assert.That(file         , Is.EqualTo(actual.File)         , "File");
            Assert.That(line         , Is.EqualTo(actual.Line)         , "Line");
        }

        static void Parse(string stackTrace, int index, string frame)
        {
            var actuals = StackTraceParser.Parse(stackTrace,
                 (f, t, m, pl, ps, fn, ln) => f);

            var actual = actuals.ElementAt(index);

            Assert.That(frame, Is.EqualTo(actual));
        }
    }
}
