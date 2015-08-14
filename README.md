# Stack Trace Parser

StackTraceParser is a single C# function that can parse a stack trace text
output (e.g. typically returned by [`Environment.StackTrace`][envst] or
[`Exception.StackTrace`][exst]) back into a sequence of stack trace frames,
including the following components:

- Type
- Method
- Parameter types and names
- File and line information, if present

It is available as a [NuGet *source* package][srcpkg] that directly embeds into
a C# project.

## Usage

`StackTraceParser` has a single function called `Parse` that takes the source
text to parse and a number of functions to project/construct each component of
a stack frame as it is parsed:

    public static IEnumerable<TFrame> Parse<TToken, TMethod, TParameters, TParameter, TSourceLocation, TFrame>(
        string text,
        Func<int, int, string, TToken> tokenSelector,
        Func<TToken, TToken, TMethod> methodSelector,
        Func<TToken, TToken, TParameter> parameterSelector,
        Func<TToken, IEnumerable<TParameter>, TParameters> parametersSelector,
        Func<TToken, TToken, TSourceLocation> sourceLocationSelector,
        Func<TToken, TMethod, TParameters, TSourceLocation, TFrame> selector)

Here is one example of how you would call it:

    var result = StackTraceParser.Parse(
        Environment.StackTrace,
        (idx, len, txt) => new // the token is the smallest unit, made of:
        {
            Index  = idx,      // - the index of the token text start
            Length = len,      // - the length of the token text
            Text   = txt,      // - the actual token text
        },
        (type, method) => new  // the method and its declaring type
        {
            Type   = type,
            Method = method,
        },
        (type, name) => new    // this is called back for each parameter with:
        {
            Type = type,       // - the parameter type
            Name = name,       // - the parameter name
        },
        (pl, ps) => new        // the parameter list and sequence of parameters
        {
            List = pl,         // - spans all parameters, including parentheses
            Parameters = ps,   // - sequence of individual parameters
        },
        (file, line) => new    // source file and line info
        {                      // called back if present
            File = file,
            Line = line,
        },
        (f, tm, p, fl) => new  // finally, put all the components of a frame
        {                      // together! The result of the parsing function
            Frame = f,         // is a sequence of this.
            tm.Type,
            tm.Method,
            ParameterList = p.List,
            p.Parameters,
            fl.File,
            fl.Line,
        });

Suppose [`Environment.StackTrace`][envst] returns (produced by running
`Environment.StackTrace` as an expression in [LINQPad][linqpad]):

    at System.Environment.GetStackTrace(Exception e, Boolean needFileInfo)
    at System.Environment.get_StackTrace()
    at UserQuery.RunUserAuthoredQuery() in c:\Users\johndoe\AppData\Local\Temp\LINQPad\_piwdiese\query_dhwxhm.cs:line 33
    at LINQPad.ExecutionModel.ClrQueryRunner.Run()
    at LINQPad.ExecutionModel.Server.RunQuery(QueryRunner runner)
    at LINQPad.ExecutionModel.Server.StartQuery(QueryRunner runner)
    at LINQPad.ExecutionModel.Server.<>c__DisplayClass36.<ExecuteClrQuery>b__35()
    at LINQPad.ExecutionModel.Server.SingleThreadExecuter.Work()
    at System.Threading.ThreadHelper.ThreadStart_Context(Object state)
    at System.Threading.ExecutionContext.RunInternal(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
    at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state, Boolean preserveSyncCtx)
    at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
    at System.Threading.ThreadHelper.ThreadStart()

Parsing it using the `StackTraceParser.Parse` example call shown earlier
will yield the following object graph:

> [Output][egout]

If all you care about is the text of each component then you can use a simpler
overload of the `Parse` method:

    var result = StackTraceParser.Parse(
        Environment.StackTrace,
        (f, t, m, pl, ps, fn, ln) => new
        {
            Frame         = f, 
            Type          = t,
            Method        = m,
            ParameterList = pl,
            Parameters    = ps,
            File          = fn,
            Line          = ln,
        });

## Background

`StackTraceParser` was born as part of the [ELMAH][elmah] project and used to
[color the stack traces][elmaheg], as can be seen from the screenshot below:

![ELMAH](http://www.hanselman.com/blog/content/binary/Windows-Live-Writer/NuGet-Package-of-the-Week-7---ELMAH-Erro_B9F2/Error_%20System.Web.HttpException%20%5B30158b95-0112-4081-91ab-c5ec7848a12c%5D%20-%20Windows%20Internet%20Explorer%20(74)_2.png)

See the [`ErrorDetailPage` source code][errdp] from the ELMAH repo for a real
example of [how the output of `StackTraceParser` was used for marking up the
stack trace in HTML][elmaheg].

  [envst]: https://msdn.microsoft.com/en-us/library/system.environment.stacktrace(v=vs.110).aspx
  [exst]: https://msdn.microsoft.com/en-us/library/system.exception.stacktrace(v=vs.110).aspx
  [srcpkg]: https://www.nuget.org/packages/StackTraceParser.Source
  [elmah]: https://elmah.github.io/
  [elmaheg]: https://bitbucket.org/project-elmah/main/src/2a6b0b5916a6b4913ca5af4c22c4e4fc69f1260d/src/Elmah.AspNet/ErrorDetailPage.cs?at=default#ErrorDetailPage.cs-45
  [errdp]: https://bitbucket.org/project-elmah/main/src/2a6b0b5916a6b4913ca5af4c22c4e4fc69f1260d/src/Elmah.AspNet/ErrorDetailPage.cs?at=default
  [linqpad]: https://www.linqpad.net/
  [egout]: https://atifaziz.github.io/projects/stack-trace-parser/linqpad-example-output.html
