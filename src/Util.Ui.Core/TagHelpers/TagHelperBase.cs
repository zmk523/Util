﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Util.Logs;
using Util.Logs.Extensions;
using Util.Ui.Components;
using Util.Ui.Renders;

namespace Util.Ui.TagHelpers {
    /// <summary>
    /// TagHelper
    /// </summary>
    public abstract class TagHelperBase : TagHelper {
        /// <summary>
        /// 标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 渲染
        /// </summary>
        public override async Task ProcessAsync( TagHelperContext tagHelperContext, TagHelperOutput output ) {
            ProcessBefore( tagHelperContext, output );
            var content = await output.GetChildContentAsync();
            var context = new Context( tagHelperContext, output, content );
            var render = GetRender( context );
            output.SuppressOutput();
            output.PostElement.SetHtmlContent( render );
            ProcessAfter();
        }

        /// <summary>
        /// 处理前操作
        /// </summary>
        /// <param name="tagHelperContext">TagHelper上下文</param>
        /// <param name="output">TagHelper输出</param>
        protected virtual void ProcessBefore( TagHelperContext tagHelperContext, TagHelperOutput output ) {
        }

        /// <summary>
        /// 获取渲染器
        /// </summary>
        /// <param name="context">上下文</param>
        protected abstract IRender GetRender( Context context );

        /// <summary>
        /// 处理后操作
        /// </summary>
        protected virtual void ProcessAfter() {
        }

        /// <summary>
        /// 写日志
        /// </summary>
        protected void WriteLog( IRender render,string caption ) {
            var log = GetLog();
            if( log.IsTraceEnabled == false )
                return;
            log.Class( GetType().FullName )
                .Caption( caption )
                .Content( render.ToString() )
                .Trace();
        }

        /// <summary>
        /// 获取日志操作
        /// </summary>
        private ILog GetLog() {
            try {
                return Log.GetLog( ComponentBase.TraceLogName );
            }
            catch {
                return Log.Null;
            }
        }
    }
}
