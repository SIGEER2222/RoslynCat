using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.JSInterop;
using RoslynCat.Helpers;
using RoslynCat.Interface;
using RoslynCat.Roslyn;
using RoslynCat.SQL;
using System.Text.Json;

namespace RoslynCat.Pages
{
    public partial class Index
    {
        public List<Diagnostic> Diagnostics { get; set; }
        [Inject] IGistService GistService { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Inject] Roslyn.CompletionProvider CompletionProvider { get; set; }
        [Inject] CodeSampleRepository CodeSample { get; set; }
        [Parameter] public string gistId { get; set; }

        public List<CodeSampleGroupAndTitle> Options;

        private string shareMessage = string.Empty;
        string baseUri;

        protected override async void OnInitialized() {
            baseUri = NavigationManager.BaseUri;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender) {
                JsRuntimeExt.Shared = JS;
                Initialize();
            }
        }
        private async Task Initialize() {
            if (gistId is object) {
                code = await GistService.GetGistContentAsync(gistId);
            }
            else {
                code = await JsRuntimeExt.Shared.GetOldCode() ?? code;
            }
            Result = "等待编译……";
            await JsRuntimeExt.Shared.CreateMonacoEditorAsync(editorId,code);
            await JsRuntimeExt.Shared.CreateMonacoEditorAsync(resultId,Result);
            RunCode();
            await JsRuntimeExt.Shared.InvokeVoidAsync("monacoInterop.registerMonacoProviders",DotNetObjectReference.Create(this));
            //Options = await CodeSample.GetGroupAndTitleList();
            //StateHasChanged();
        }

        [JSInvokable("FormatCode")]
        public async Task<string> FormatCode(string code) => await CompletionProvider.FormatCode(code);

        [JSInvokable("HoverInfoProvide")]
        public async Task<string> HoverInfoProvide(string code,int position) {
            IResponse respone = await Provider(code,position,RequestType.Hover);
            HoverInfoResult result = respone as HoverInfoResult;
            return JsonSerializer.Serialize(result);
        }
        [JSInvokable("ProvideCompletionItems")]
        public async Task<string> ProvideCompletionItems(string code,int position) {
            IResponse respone = await Provider(code,position,RequestType.Complete);
            CompletionResult result = respone as CompletionResult;
            return JsonSerializer.Serialize(result.Suggestions);
        }
        [JSInvokable("GetModelMarkers")]
        public async Task<string> GetModelMarkers(string code,int position) {
            IResponse respone = await Provider(code,position,RequestType.CodeCheck);
            CodeCheckResult result = respone as CodeCheckResult;
            return JsonSerializer.Serialize(result.codeChecks);
        }

        [JSInvokable("AutoRunCode")]
        public async Task<string> AutoRunCode(string code) {
            string inputValue = GetConsoleValue()??string.Empty;
            return await CompletionProvider.RunCode(code,inputValue);
        }

        protected async Task<IResponse> Provider(string code,int position,RequestType request) {
            SourceInfo sourceInfo = new SourceInfo(code,string.Empty,position);
            sourceInfo.Type = request;
            await CompletionProvider.CreateProviderAsync(sourceInfo);
            IResponse respone = await CompletionProvider.GetResultAsync();
            return respone;
        }

        protected async Task RunCode() {
            //Options = await CodeSample.GetGroupAndTitleList();
            string inputValue = GetConsoleValue()??string.Empty;
            code = await JsRuntimeExt.Shared.GetValue(editorId);
            Result = await CompletionProvider.RunCode(code,inputValue);
            await JsRuntimeExt.Shared.SetValue(resultId,Result);
        }

        private async Task SetCode(int id) {
            CodeSample sample = await CodeSample.GetById(id);
            Console.WriteLine(sample.Code);
            await JsRuntimeExt.Shared.SetValue(editorId,sample.Code);
        }


        protected async Task CodeSharing() {
            code = await JsRuntimeExt.Shared.GetValue(editorId);
            if (string.IsNullOrEmpty(code)) return;
            await GistService.CreateGistAsync(code);
            if (GistService.GistId is not null) {
                shareMessage = $"{baseUri}codeshare/{GistService.GistId}";
                await JsRuntimeExt.Shared.CopyUrl(shareMessage);
            }
            else {
                shareMessage = string.Empty;
            }
            show = true;
        }

        private void OnMyParameterChanged() {
            JsRuntimeExt.Shared.CreateMonacoEditorAsync(resultId,Result);
        }

        private void OnDiagnosticsUpdated(object sender,List<Diagnostic> diagnostics) {
            Diagnostics = diagnostics;
            InvokeAsync(() => { this.StateHasChanged(); });
            //_ = JS.SetMonacoDiagnosticsAsync(_editorId, diagnostics);
        }

        //protected async Task AskGPT() {
        //    string ask =  await JsRuntimeExt.Shared.GetValue(editorId);
        //    Result = "˼���С�����ȴ�";
        //    askGpt = "����˼���������ظ����";
        //    StateHasChanged();
        //    await JsRuntimeExt.Shared.SetValue(resultId,Result);
        //    Result = await new ChatGPT().Reply(ask);
        //    await JsRuntimeExt.Shared.SetValue(resultId,Result);
        //    askGpt = "����ChatGPT?";
        //}
    }

}