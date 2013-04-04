using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace redinc_reboot.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcConfirmModal BeginConfirmModal(this HtmlHelper htmlHelper,
                                                          string header = "Are you sure?",
                                                          string id = "confirmModal")
        {
            var writer = htmlHelper.ViewContext.Writer;

            writer.Write(
                "<div id='" + id + "' class='modal hide fade' tabindex='-1' role='dialog' aria-labelledby='" + id + "Label' aria-hidden='true'>"
                + " <div class='modal-header'>"
                + "     <button type='button' class='close' data-dismiss='modal' aria-hidden='true'>×</button>"
                + "     <h3 id='" + id + "Label'>" + header + "</h3>"
                + " </div>"
                + " <div class='modal-body'>");

            return new MvcConfirmModal(htmlHelper.ViewContext, id);
        }
    }

    public class MvcConfirmModal : IDisposable
    {
        private readonly TextWriter _writer;
        private readonly string _id;
        public MvcConfirmModal(ViewContext viewContext, string id)
        {
            _writer = viewContext.Writer;
            _id = id;
        }

        public void Dispose()
        {
            this._writer.Write(
                "</div>"
                + "<div class='modal-footer'>"
                + " <button class='btn' data-dismiss='modal' aria-hidden='true'>Cancel</button>"
                + " <button id='" + _id + "Button' class='btn btn-primary'>Confirm</button>"
                + "</div>"
                + "</div>");
        }
    }
}