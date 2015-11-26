﻿using System;
using System.IO;
using System.Net;
using System.Text;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;

namespace Hubs1.Core.ViewModels
{
    public class BaseViewModel
        : MvxViewModel

    {
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(() => IsLoading);
            }
        }

        public void ReportError(string error)
        {
            Mvx.Resolve<IErrorReporter>().ReportError(error);
        }

        protected void GeneralAsyncLoad(string url, Action<Stream> responseStreamHandler,string method="GET", string data="")
        {
            try
            {
                IsLoading = true;
                MvxTrace.Trace("Fetching {0}", url);
                var request = WebRequest.Create(url);
                Encoding encoding = Encoding.UTF8;
                if (request != null)
                {
                    request.Method = method;
                    
                    request.ContentType = "application/json";
                    if (method.ToUpper() == "POST")
                    {
                        byte[] buffer = encoding.GetBytes(data);
                        request.BeginGetRequestStream((requestResult) =>
                        {
                            try
                            {
                                using (var stream = request.EndGetRequestStream(requestResult))
                                {
                                    stream.Write(buffer, 0, buffer.Length);
                                    request.BeginGetResponse(
                                        (result) => GeneralProcessResponse(request, result, responseStreamHandler), null);
                                }

                            }
                            catch (Exception exception)
                            {
                                IsLoading = false;
                                ReportError("Sorry - problem seen " + exception.Message);
                            }
                        }, null);
                    }
                    else
                    {
                        request.BeginGetResponse(
                                       (result) => GeneralProcessResponse(request, result, responseStreamHandler), null);
                    }
                    
                }
            }
            catch (Exception exception)
            {
                IsLoading = false;
                ReportError("Sorry - problem seen " + exception.Message);
            }
        }

        private void GeneralProcessResponse(WebRequest request, IAsyncResult result, Action<Stream> responseStreamHandler)
        {
            IsLoading = false;
            try
            {
                var response = request.EndGetResponse(result);
                using (var stream = response.GetResponseStream())
                {
                    responseStreamHandler(stream);
                }
            }
            catch (Exception exception)
            {
                ReportError("Sorry - problem seen " + exception.Message);
            }
        }
    }
}