namespace MvcApplication61
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Web;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using System.Web.Mvc;

    using CacheManager;

    using WebGrease.Css.Extensions;

    using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

    #endregion

    /// <summary>The output cache.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class OutputCacheManager : ActionFilterAttribute
    {
        #region Constructors and Destructors

        /// <summary>Initializes a new instance of the <see cref="OutputCacheManager"/> class.</summary>
        public OutputCacheManager()
        {
            this.CacheManager = new MemoryCacheManager();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the cache manager.</summary>
        public ICacheManager CacheManager { get; set; }

        /// <summary>Gets or sets the duration.</summary>
        public int Duration { get; set; }

        /// <summary>Gets the calculate duration.</summary>
        private int CalculateDuration
        {
            get
            {
                return this.Duration <= 0 ? 600 : this.Duration;
            }
        }
        #endregion

        #region Public Methods and Operators

        /// <summary>The on action executed.</summary>
        /// <param name="actionExecutedContext">The action executed context.</param>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                var key = this.ComputeKeyName(actionExecutedContext.ActionContext);
                var cachedOutput = this.CacheManager.GetUnSliding<string>(key);

                if (cachedOutput != null)
                {
                    base.OnActionExecuted(actionExecutedContext);
                    return;
                }

                string response = string.Empty;

                if (actionExecutedContext.Response != null)
                {
                    var content = actionExecutedContext.Response.Content as ObjectContent;
                    if (content != null)
                    {
                        response = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
                    }
                }
                else
                {
                    return;
                }

                this.CacheManager.Set(key, response, this.CalculateDuration / 60);

                HttpContext.Current.Response.Write(response);
            }
            catch (Exception err)
            {
                Debug.WriteLine(err);

                base.OnActionExecuted(actionExecutedContext);
            }
        }

        /// <summary>The on action executing.</summary>
        /// <param name="actionContext">The action context.</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                var key = this.ComputeKeyName(actionContext);
                var cachedOutput = this.CacheManager.GetUnSliding<string>(key);
                if (cachedOutput != null)
                {
                    var responseMessage = new HttpResponseMessage { Content = new StringContent(cachedOutput) };
                    responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    actionContext.Response = responseMessage;
                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
            }
        }

        #endregion

        #region Methods

        /// <summary>The compute key name.</summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns>The <see cref="string"/>.</returns>
        private string ComputeKeyName(HttpActionContext filterContext)
        {
            var keyBuilder = new StringBuilder();

            foreach (var pair in filterContext.ControllerContext.RouteData.Values)
            {
                keyBuilder.AppendFormat("rd{0}_{1}_", pair.Key.GetHashCode(), pair.Value.GetHashCode());
            }

            foreach (var pair in filterContext.ActionArguments)
            {
                keyBuilder.AppendFormat("ap{0}_{1}_", pair.Key.GetHashCode(), pair.Value.GetHashCode());
            }

            return keyBuilder.ToString();
        }

        #endregion
    }
}