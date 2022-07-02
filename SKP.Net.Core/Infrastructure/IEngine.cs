using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKP.Net.Core.Infrastructure
{
    /// <summary>
    /// Service provide by this class to handle Engine
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Configure service
        /// </summary>
        /// <param name="service"></param>
        /// <param name="configuration"></param>
        void ConfigureServices(IServiceCollection service, IConfiguration configuration);

        /// <summary>
        /// Configure http request pipeline
        /// </summary>
        /// <param name="application"></param>
        void ConfigureRequestPipeline(IApplicationBuilder application);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        /// Resolve unregistered
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object ResolveUnregistered(Type type);
    }
}
