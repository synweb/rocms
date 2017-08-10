/*
 * Copyright 2007-2011 JetBrains s.r.o.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace JetBrains.Annotations
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PathReferenceAttribute : Attribute
    {
        public PathReferenceAttribute()
        {
        }

        public PathReferenceAttribute([PathReference] string basePath)
        {
            BasePath = basePath;
        }

        public string BasePath { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcModelTypeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
    public sealed class AspMvcControllerAttribute : Attribute
    {
        public AspMvcControllerAttribute()
        {
        }

        public AspMvcControllerAttribute(string anonymousProperty)
        {
            AnonymousProperty = anonymousProperty;
        }

        public string AnonymousProperty { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcMasterAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
    public sealed class AspMvcViewAttribute : PathReferenceAttribute
    {
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcAreaAttribute : PathReferenceAttribute
    {
        public AspMvcAreaAttribute()
        {
        }

        public AspMvcAreaAttribute(string anonymousProperty)
        {
            AnonymousProperty = anonymousProperty;
        }

        public string AnonymousProperty { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
    public sealed class AspMvcActionAttribute : Attribute
    {
        public AspMvcActionAttribute()
        {
        }

        public AspMvcActionAttribute(string anonymousProperty)
        {
            AnonymousProperty = anonymousProperty;
        }

        public string AnonymousProperty { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class AspMvcTemplateAttribute : Attribute
    {
    }
}